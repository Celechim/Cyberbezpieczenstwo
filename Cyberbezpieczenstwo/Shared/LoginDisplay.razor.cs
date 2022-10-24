using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Cyberbezpieczenstwo.Shared;

public partial class LoginDisplay
{
    [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authState { get; set; }
    [Inject] private NavigationManager navManager { get; set; }
    [Inject] private UserController userController { get; set; }
    private UserDTO? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var auth = await authState;
        User = await userController.GetByLogin(auth.User.Identity?.Name);
    }

    private async Task Logout()
    {
        var auth = (AuthStateProvider)authStateProvider;
        await auth.UpdateAuthenticationState(null);
        navManager.NavigateTo("/login", true);
    }
}

