using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Microsoft.AspNetCore.Components;

namespace Cyberbezpieczenstwo.Pages;

public partial class Users
{
    [Inject] private UserController userController { get; set; }
    [Inject] private NavigationManager navManager { get; set; }
    private IEnumerable<UserDTO>? UserList { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var response = await userController.List();
        if (response != null)
        {
            UserList = response;
        }
    }

    private void Redirect()
    {
        navManager.NavigateTo("/nowy-uzytkownik");
    }
}

