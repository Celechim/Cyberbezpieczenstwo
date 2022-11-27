using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Data.Services;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Authentication;
using Cyberbezpieczenstwo.Data.Interfaces;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Data.Specifications;
using Cyberbezpieczenstwo.Shared;
using Cyberbezpieczenstwo.SharedKernel.Enums;
using Cyberbezpieczenstwo.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace Cyberbezpieczenstwo.Pages;

public partial class Login
{
	[Inject] private IUserService _userService { get; set; }
	[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
	[Inject] private IJSRuntime js { get; set; }
	[Inject] private UserController _userController { get; set; }
    [Inject] private PasswordLimitationController _limitationController { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }
    [Inject] private SecuritySettingsController _settingsController { get; set; }
    [Inject] private GooglereCaptchaService _googlereCaptchaService { get; set; }

    private class Model
	{
		public string Login { get; set; }
		public string Password { get; set; }
        public string? Answer { get; set; }
	}

	private class PasswordModel
	{
		public string Password { get; set; }
		public string PasswordToConfirm { get; set; }
	}

	private Model model = new Model();
	private PasswordModel passwordModel = new PasswordModel();
	private string view { get; set; } = "default";
	private bool showNotMatching { get; set; } = false;
	private UserDTO User { get; set; }
    private LoginValidator _loginValidator;
	private UserDTO UserModel = new UserDTO();
    public DateTime failChecker = DateTime.UtcNow;
    int blockTime=0;
    bool isBlocked = false;
    private bool captchaEnabled { get; set; } = false;
    private bool reCaptchaEnabled { get; set; } = false;
    private String? question { get; set; } = "";
    private String? answer { get; set; } = "";
    private bool wrongCaptcha { get; set; } = false;

    private string captchaResponse { get; set; }
    private string style { get; set; } = "display: none";

    protected override async Task OnInitializedAsync()
	{
        
        var limitations = await _limitationController.List();
        var settings = await _settingsController.Get();
        if (settings is not null)
        {
            captchaEnabled = settings.Value.First().CaptchaEnabled;
            reCaptchaEnabled = settings.Value.First().ReCaptchaEnabled;
            if (reCaptchaEnabled)
                style = "display: block";

            var captcha = Captcha.GenerateCaptchaQuestion();
            question = captcha.Question;
            answer = captcha.Answer;
        }
        var array = limitations.ToArray();
        _loginValidator = new LoginValidator(array[0].IsActive, array[1].IsActive, array[2].IsActive);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await js.InvokeAsync<int>("googleRecaptcha", DotNetObjectReference.Create(this), "recaptcha", "6LfrhwAjAAAAAJK-4l7STjlLnegbpvkZ0UMvcjS-");
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable, EditorBrowsable(EditorBrowsableState.Never)]
    public void CallbackOnSuccess(string response)
    {
        captchaResponse = "success";
    }

    [JSInvokable, EditorBrowsable(EditorBrowsableState.Never)]
    public void CallbackOnExpired(string response)
    {
        captchaResponse = "fail";
    }

    private void ShowResponse()
    {
        captchaResponse = $"Odpowiedź: {captchaResponse}";
    }

    private async Task Authenticate()
	{
        if (captchaEnabled && model.Answer != answer)
        {
            wrongCaptcha = true;
            return;
        }

        if (reCaptchaEnabled && captchaResponse != "success")
        {
            wrongCaptcha = true;
            return;
        }

		User = await _userController.GetByLogin(model.Login);
		if(User == null)
		{
            await js.InvokeVoidAsync("alert", "Login lub Hasło niepoprawny");
			return;
        }
        if(blockTime<=0)
        failChecker = await _logHistoryController.DateOfLastFail(User.Login);
        blockTime = (int)failChecker.Subtract(DateTime.UtcNow).TotalSeconds;
        if (blockTime > 0)
        {
            await js.InvokeVoidAsync("alert", $"Za dużo nieudanych logowań, spróbuj ponownie za {blockTime}");
            return;
        }
        
        if (!User.HasOneUsePassword && !HashProvider.VerifyHashedPassword(User.Password, model.Password))
		{
            await js.InvokeVoidAsync("alert", "Login lub Hasło niepoprawny");
            await SendLog(User.Login, "Niepoprawne hasło", false);            
            return;
		}

		
        if (User.HasOneUsePassword&& !HashProvider.VerifyHashedPassword(User.OneUsePassword, model.Password))
		{
            await js.InvokeVoidAsync("alert", "Login lub Hasło niepoprawny");
            await SendLog(User.Login, "Niepoprawne hasło", false);

            return;
        }
        if (User.HasOneUsePassword && HashProvider.VerifyHashedPassword(User.OneUsePassword, model.Password))
        {
            await SendLog(User.Login, "Wykorzystano jednorazowe hasło", true);
            view = "newPassword";
            return;
        }
        if (User.IsBlocked)
        {
            await js.InvokeVoidAsync("alert", "Twoje konto zostało zablokowane");
            await SendLog(User.Login, "Zablokowane konto", false);

            return;
        }
        if (User.Role != 0 && (User.ExpirationTime.Value < DateTime.Now || User.UsedPasswords.Count() == 1))
		{
			view = "newPassword";
		}
		else
		{
			var auth = (AuthStateProvider)authStateProvider;
			await auth.UpdateAuthenticationState(new User
			{
				Login = User.Login,
				Role = (UserRole)User.Role,
			});
            await SendLog(User.Login, "Pomyślnie zalogowano", true);
            Thread.Sleep(50);
            navManager.NavigateTo("/", true);
            return;
		}
	}

	private async Task ChangePassword()
	{
        if (UserModel.Password != passwordModel.PasswordToConfirm)
		{
            await SendLog(User.Login, "Hasła nie są takie same", false);
            showNotMatching = true;
		}
		else
		{
			User.Password = HashProvider.HashPassword(UserModel.Password);
			await _userController.Update(User);
            User.OneUsePassword = String.Empty;
            User.HasOneUsePassword = false;
            var auth = (AuthStateProvider)authStateProvider;
            await auth.UpdateAuthenticationState(new User
            {
                Login = User.Login,
                Role = (UserRole)User.Role,
            });
            await SendLog(User.Login, "Pomyślnie zmieniono hasło", true);
            navManager.NavigateTo("/", true);
        }
	}
    public async Task SendLog(string login, string desc, bool status)
    {
        var dto = new LogHistoryDTO();
        dto.UserName = login;
        dto.Desc = desc;
        dto.Status = status;
        await _logHistoryController.Post(dto);
    }

    private void RollQuestion()
    {
        var captcha = Captcha.GenerateCaptchaQuestion();
        question = captcha.Question;
        answer = captcha.Answer;
    }
}

