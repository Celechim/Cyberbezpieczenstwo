using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Pages;

public partial class NewUser
{
	[Inject] private UserController userController { get; set; }
	[Inject] private NavigationManager navManager { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    private UserDTO? newUser { get; set; }

	protected override async Task OnInitializedAsync()
	{
		newUser = new UserDTO();
        newUser.ExpirationTime = DateTime.UtcNow.AddDays(10);
        newUser.Role = 1;
	}

	private async Task CreateUser()
	{
		if (newUser.HasOneUsePassword)
		{
            Random rd = new Random();
            int x = rd.Next(1,100);
			double a = newUser.Login.Length;
            var newPassword = Math.Exp(-a * x);
			
            await js.InvokeVoidAsync("alert", $"Twoje jednorazowe hasło to:{newPassword}");
            newUser.X = x;
            newUser.OneUsePassword=HashProvider.HashPassword(newPassword.ToString());
            newUser.Password = String.Empty;

            newUser.Role = 1;
            newUser.IsBlocked = false;
            newUser.UsedPasswords = new List<UsedPasswordDTO>();
            newUser.ExpirationTime = newUser.ExpirationTime.Value.ToUniversalTime();
            await userController.Post(newUser);
            await SendLog(newUser.Login, "pomyślnie założono konto", true);

            navManager.NavigateTo("/uzytkownicy");

        }
        else
        { 
            newUser.Password = HashProvider.HashPassword(newUser.Password);
		
            //newUser.Role = 1;
		    newUser.IsBlocked = false;
		    newUser.UsedPasswords = new List<UsedPasswordDTO>()
		    {
			    new UsedPasswordDTO(0, newUser, newUser.Password)
		    };
		    newUser.ExpirationTime = newUser.ExpirationTime.Value.ToUniversalTime();
		    await userController.Post(newUser);
            await SendLog(newUser.Login, "pomyślnie założono konto", true);
            newUser.OneUsePassword = String.Empty;
            navManager.NavigateTo("/uzytkownicy");
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
}

