using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
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

namespace Cyberbezpieczenstwo.Pages;

public partial class Login
{
	[Inject] private IUserService _userService { get; set; }
	[Inject] private AuthenticationStateProvider authStateProvider { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
	[Inject] private IJSRuntime js { get; set; }
	[Inject] private UserController _userController { get; set; }
    [Inject] private PasswordLimitationController _limitationController { get; set; }

    private class Model
	{
		public string Login { get; set; }
		public string Password { get; set; }
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

    protected override async Task OnInitializedAsync()
	{
        var limitations = await _limitationController.List();
        var array = limitations.ToArray();
        _loginValidator = new LoginValidator(array[0].IsActive, array[1].IsActive, array[2].IsActive);
    }

	private async Task Authenticate()
	{
		User = await _userController.GetByLogin(model.Login);
		if (User == null || !HashProvider.VerifyHashedPassword(User.Password, model.Password))
		{
			await js.InvokeVoidAsync("alert", "Login lub Hasło niepoprawny");
			return;
		}

		if (User.IsBlocked)
		{
            await js.InvokeVoidAsync("alert", "Twoje konto zostało zablokowane");
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

			navManager.NavigateTo("/", true);
		}
	}

	private async Task ChangePassword()
	{
		if (UserModel.Password != passwordModel.PasswordToConfirm)
		{
			showNotMatching = true;
		}
		else
		{
			User.Password = HashProvider.HashPassword(UserModel.Password);
			await _userController.Update(User);

            var auth = (AuthStateProvider)authStateProvider;
            await auth.UpdateAuthenticationState(new User
            {
                Login = User.Login,
                Role = (UserRole)User.Role,
            });

            navManager.NavigateTo("/", true);
        }
	}
}

