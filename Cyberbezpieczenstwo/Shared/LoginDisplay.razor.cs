using System;
using Cyberbezpieczenstwo.API.Controllers;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.API.Modelsp;
using Cyberbezpieczenstwo.Authentication;
using Cyberbezpieczenstwo.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.Shared;

public partial class LoginDisplay
{
    [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authState { get; set; }
    [CascadingParameter] private AuthenticationState Auth { get; set; }
    [Inject] private NavigationManager navManager { get; set; }
    [Inject] private UserController userController { get; set; }
    [Inject] private SecuritySettingsController securitySettingsController { get; set; }
    [Inject] private IJSRuntime js { get; set; }
    [Inject] private LogHistoryController _logHistoryController { get; set; }
    private UserDTO? User { get; set; }

    protected override async Task OnInitializedAsync()
    {
        User = await userController.GetByLogin(Auth.User.Identity?.Name);
        if (Auth.User.Identity.IsAuthenticated)
        {

            var security = await securitySettingsController.Get();
            if (security.Value.Count() > 0)
                if (security.Value.First().IsSetAutoLogout)
                {
                    await js.InvokeVoidAsync("initilizeInactivityTimer", 
                        DotNetObjectReference.Create(this),
                        security.Value.First().SecToAutoLogout*1000);
                }
        }
    }

    [JSInvokable]
    public async Task Logout()
    {
        var auth = (AuthStateProvider)authStateProvider;
        await auth.UpdateAuthenticationState(null);
        await SendLog(User.Login, "pomyślnie Wylogowano z konta", true);
        navManager.NavigateTo("/login", true);
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

