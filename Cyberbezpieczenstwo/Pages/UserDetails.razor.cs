using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using FluentValidation;
using Blazored.FluentValidation;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Cyberbezpieczenstwo.API.Modelsp;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Pages;

public partial class UserDetails
{
	[Inject] private UserController userController { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }
    [Inject] private IJSRuntime js { get; set; }

    [Inject] private PasswordLimitationController _limitationController { get; set; }
	[CascadingParameter] private Task<AuthenticationState> authenticationState { get; set; }
	[Parameter] public int Id { get; set; }
	private UserDTO User { get; set; }
	private string View { get; set; } = "details";
	private bool showInvalidPasswords { get; set; } = false;
	private bool showPasswordAlreadyUsed { get; set; } = false;
    private UserDTO? userToUpdate { get; set; }
	private string passwordToConfirm { get; set; }
	private int passwordExpiresIn { get; set; }
	private LoginValidator _loginValidator;
	private AuthenticationState authState { get; set; }

    protected override async Task OnInitializedAsync()
	{
		authState = await authenticationState;
		var limitations = await _limitationController.List();
		var array = limitations.ToArray();
		_loginValidator = new LoginValidator(array[0].IsActive, array[1].IsActive, array[2].IsActive);
		User = await userController.GetById(Id);
		userToUpdate = new UserDTO();
		userToUpdate.Login = User.Login;
		if (User.Role != 0)
			passwordExpiresIn = (User.ExpirationTime.Value.Date - DateTime.Now.Date).Days;
		if (User.Role != 0)
			passwordExpiresIn = (User.ExpirationTime - DateTime.Now).Value.Days;
	}

	private async Task Delete()
	{
		await userController.Delete(User.Id);
        await SendLog(userToUpdate.Login, "pomyślnie usunięto konto", true);
        navManager.NavigateTo("/uzytkownicy");
	}

	private async Task Update()
	{
		if (authState.User.IsInRole("Administrator"))
		{
            if (userToUpdate.Login != User.Login && userToUpdate.Login != null)
                await SendLog(userToUpdate.Login, "Administrator zmienił login", true);
            else
                userToUpdate.Login = User.Login;
            userToUpdate.Id=User.Id;
			if (!userToUpdate.HasOneUsePassword) 
			{
                if (userToUpdate.Password == null)
                    userToUpdate.Password = User.Password;
                else
                {
                    userToUpdate.Password = HashProvider.HashPassword(userToUpdate.Password);
                    await SendLog(userToUpdate.Login, "Administrator zmienił haslo", true);
                }
            }
		
			if(userToUpdate.HasOneUsePassword)
			{
                Random rd = new Random();
                int x = rd.Next(1, 100);
                double a = userToUpdate.Login.Length;
                var newPassword = Math.Exp(-a * x);

                await js.InvokeVoidAsync("alert", $"Twoje jednorazowe hasło to:{newPassword.ToString()}");
                userToUpdate.X = x; 
				userToUpdate.OneUsePassword = HashProvider.HashPassword(newPassword.ToString());
            }

            if (passwordExpiresIn > 0)
				await SendLog(userToUpdate.Login, "Administrator zmienił czas wygasniecia hasla", true);
			userToUpdate.ExpirationTime = DateTime.Now.AddDays(passwordExpiresIn).ToUniversalTime();
            if(User.Role!=userToUpdate.Role)
			{
                await SendLog(userToUpdate.Login, $"Administrator zmienił rolę na {userToUpdate.Role}", true);
            }
			
            await userController.Update(userToUpdate);
            User = await userController.GetById(userToUpdate.Id);
            View = "details";
			//navManager.NavigateTo("/", true);
		}
        
		if (!authState.User.IsInRole("Administrator")&&!HashProvider.VerifyHashedPassword(User.Password, passwordToConfirm))
		{
			showInvalidPasswords = true;
		}
		else
		{
			if (!authState.User.IsInRole("Administrator")&&User.UsedPasswords.Where(x => HashProvider.VerifyHashedPassword(x.UsedPassword, HashProvider.HashPassword(userToUpdate.Password))).Count() > 0)
			{
				showPasswordAlreadyUsed = true;
            }
            else
            {
				if(!authState.User.IsInRole("Administrator"))
					{
                    userToUpdate.Password = HashProvider.HashPassword(userToUpdate.Password);
                    userToUpdate.Id = User.Id;
                    if (User.Role != 0)
                        userToUpdate.ExpirationTime = DateTime.Now.AddDays(passwordExpiresIn).ToUniversalTime();
                    else
                        userToUpdate.Login = User.Login;
                    await userController.Update(userToUpdate);
                    User = await userController.GetById(Id);
                    View = "details";
                    navManager.NavigateTo("/uzytkownicy");
                }
            }
		}
    }

	private void ChangeView()
	{
		if (View == "details")
			View = "edit";
		else
			View = "details";
	}
    public async Task SendLog(string login, string desc, bool status)
    {
        var dto = new LogHistoryDTO();
        dto.UserName = login;
        dto.Desc = desc;
        dto.Status = status;
        await _logHistoryController.Post(dto);
    }
}

