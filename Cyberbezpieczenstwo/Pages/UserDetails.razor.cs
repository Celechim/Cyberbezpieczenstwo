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

namespace Cyberbezpieczenstwo.Pages;

public partial class UserDetails
{
	[Inject] private UserController userController { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
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
		passwordExpiresIn = (User.ExpirationTime.Value.Date - DateTime.Now.Date).Days;
		if (User.Role != 0)
			passwordExpiresIn = (User.ExpirationTime - DateTime.Now).Value.Days;
	}

	private async Task Delete()
	{
		await userController.Delete(User.Id);
		navManager.NavigateTo("/uzytkownicy");
	}

	private async Task Update()
	{
		if (!HashProvider.VerifyHashedPassword(User.Password, passwordToConfirm))
		{
			showInvalidPasswords = true;
		}
		else
		{
			if (User.UsedPasswords.Where(x => HashProvider.VerifyHashedPassword(x.UsedPassword, HashProvider.HashPassword(userToUpdate.Password))).Count() > 0)
			{
				showPasswordAlreadyUsed = true;
            }
            else
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
}

