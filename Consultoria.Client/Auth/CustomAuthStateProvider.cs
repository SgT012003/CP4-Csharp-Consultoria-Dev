using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Consultoria.Client.Auth;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public CustomAuthStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userDto = await _httpClient.GetFromJsonAsync<UserDto>("api/auth/user");
            if (userDto != null)
            {
                _currentUser = BuildClaimsPrincipal(userDto);
            }
        }
        catch
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        }

        return new AuthenticationState(_currentUser);
    }

    public void NotifyUserAuthenticated(UserDto user)
    {
        _currentUser = BuildClaimsPrincipal(user);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public void NotifyUserLoggedOut()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }

    private static ClaimsPrincipal BuildClaimsPrincipal(UserDto user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName)
        };

        foreach (var role in user.Roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var identity = new ClaimsIdentity(claims, "ServerAuth");
        return new ClaimsPrincipal(identity);
    }
}
