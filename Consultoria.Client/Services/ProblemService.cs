using Consultoria.Shared.DTOs;
using System.Net.Http.Json;

namespace Consultoria.Client.Services;

public class ProblemService
{
    private readonly HttpClient _httpClient;

    public ProblemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProblemDto>> GetAllProblemsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ProblemDto>>("api/client/problems") ?? new();
        }
        catch { return new(); }
    }

    public async Task<ProblemDto?> GetProblemByIdAsync(int id)
    {
        try { return await _httpClient.GetFromJsonAsync<ProblemDto>($"api/client/problems/{id}"); }
        catch { return null; }
    }
}
