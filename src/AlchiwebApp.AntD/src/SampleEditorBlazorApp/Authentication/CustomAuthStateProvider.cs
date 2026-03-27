using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Authentication;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService  _localStorage;
    private readonly ILogger<CustomAuthStateProvider> _logger;
    
    public CustomAuthStateProvider(ILocalStorageService localStorage, ILogger<CustomAuthStateProvider> logger)
    {
        _localStorage = localStorage;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = (await _localStorage.GetItemAsync<string>("token"));
        var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : GetClaimsIdentity(token);
        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _localStorage.SetItemAsync("token", token);
        var identity = GetClaimsIdentity(token);
        var user = new ClaimsPrincipal(identity);
        var authenticationState = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }

    private ClaimsIdentity GetClaimsIdentity(string token)
    {
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var claims = jwtToken.Claims.ToList();

        var identity = new ClaimsIdentity(claims, "jwt", "unique_name", "role");
        return identity;
    }
    
    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync("token");
        var authenticationState = new AuthenticationState(new ClaimsPrincipal());
        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
    }
}
