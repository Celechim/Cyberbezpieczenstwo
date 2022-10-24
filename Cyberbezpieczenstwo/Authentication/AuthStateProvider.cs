using System;
using System.Security.Claims;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Cyberbezpieczenstwo.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
	private readonly ProtectedSessionStorage _sessionStorage;
	private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

	public AuthStateProvider(ProtectedSessionStorage sessionStorage)
	{
		_sessionStorage = sessionStorage;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var userSessionStorageResult = await _sessionStorage.GetAsync<User>("UserSession");
			var userSession = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

			if (userSession == null)
				return await Task.FromResult(new AuthenticationState(_anonymous));

			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
			{
				new Claim(ClaimTypes.Name, userSession.Login),
				new Claim(ClaimTypes.Role, userSession.Role.ToString())
			}, "CustomAuth"));
			return await Task.FromResult(new AuthenticationState(claimsPrincipal));
		}
		catch
		{
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
	}

	public async Task UpdateAuthenticationState(User user)
	{
		ClaimsPrincipal claimsPrincipal;

		if(user != null)
		{
			await _sessionStorage.SetAsync("UserSession", user);
			claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			}));
		}
		else
		{
			await _sessionStorage.DeleteAsync("UserSession");
			claimsPrincipal = _anonymous;
		}

		NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
	}
}

