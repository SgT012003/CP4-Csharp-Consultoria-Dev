using Consultoria.Client.Auth;
using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace Consultoria.Client.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthService(HttpClient httpClient, AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _authStateProvider = (CustomAuthStateProvider)authStateProvider;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", dto);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>()
            ?? new AuthResponseDto { Success = false, Message = "Erro inesperado." };

        if (result.Success && result.User != null)
            _authStateProvider.NotifyUserAuthenticated(result.User);

        return result;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", dto);
        var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>()
            ?? new AuthResponseDto { Success = false, Message = "Erro inesperado." };

        if (result.Success && result.User != null)
            _authStateProvider.NotifyUserAuthenticated(result.User);

        return result;
    }

    public async Task LogoutAsync()
    {
        await _httpClient.PostAsync("api/auth/logout", null);
        _authStateProvider.NotifyUserLoggedOut();
    }
}
