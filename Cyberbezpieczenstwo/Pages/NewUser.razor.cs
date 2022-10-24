using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Shared;
using Microsoft.AspNetCore.Components;

namespace Cyberbezpieczenstwo.Pages;

public partial class NewUser
{
	[Inject] private UserController userController { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
	private UserDTO? newUser { get; set; }

	protected override async Task OnInitializedAsync()
	{
		newUser = new UserDTO();
	}

	private async Task CreateUser()
	{
		newUser.Password = HashProvider.HashPassword(newUser.Password);
        newUser.Role = 1;
		newUser.IsBlocked = false;
		newUser.UsedPasswords = new List<UsedPasswordDTO>()
		{
			new UsedPasswordDTO(0, newUser, newUser.Password)
		};
		newUser.ExpirationTime = newUser.ExpirationTime.Value.ToUniversalTime();
		await userController.Post(newUser);
		navManager.NavigateTo("/uzytkownicy");
	}
}

