using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Cyberbezpieczenstwo;
using Cyberbezpieczenstwo.Shared;
using Cyberbezpieczenstwo.Authentication;
using Cyberbezpieczenstwo.API.Controllers;
using NuGet.Configuration;
using Cyberbezpieczenstwo.Data.Models;
using Cyberbezpieczenstwo.API.Models;
using Cyberbezpieczenstwo.Data.Specifications;

namespace Cyberbezpieczenstwo.Shared;

public partial class MainLayout
{
    [Inject] private AuthenticationStateProvider authStateProvider { get; set; }
    [CascadingParameter] private Task<AuthenticationState> authState { get; set; }
    [Inject] private SecuritySettingsController _securitySettingsController { get; set; }
    [Inject] private UserController userController { get; set; }
    public AuthenticationState Auth { get; set; }
    private List<SecuritySettings> securitySettings { get; set; } = new List<SecuritySettings>();
    private IEnumerable<SecuritySettings> Settings { get; set; }
    protected override async Task OnInitializedAsync()
    {
        Auth = await authState;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
    }

    [JSInvokable]
    public async Task Logout()
    {
        var auth = (AuthStateProvider)authStateProvider;
        await auth.UpdateAuthenticationState(null);
        navManager.NavigateTo("/login", true);
    }
}