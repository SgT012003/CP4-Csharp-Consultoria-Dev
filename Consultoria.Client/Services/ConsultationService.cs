using Consultoria.Shared.DTOs;
using System.Net.Http.Json;

namespace Consultoria.Client.Services;

public class ConsultationService
{
    private readonly HttpClient _httpClient;

    public ConsultationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string Message)> SubmitRequestAsync(CreateConsultationRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/client/consultation", dto);
            if (response.IsSuccessStatusCode)
                return (true, "Solicitação enviada com sucesso!");
            var error = await response.Content.ReadAsStringAsync();
            return (false, "Erro ao enviar solicitação.");
        }
        catch { return (false, "Erro de conexão. Tente novamente."); }
    }

    public async Task<List<ConsultationRequestDto>> GetAllRequestsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ConsultationRequestDto>>("api/admin/consultation") ?? new();
        }
        catch { return new(); }
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateConsultationRequestDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/admin/consultation/{id}/status", dto);
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public async Task<string?> DispatchAsync(int requestId, int developerId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/admin/consultation/{requestId}/dispatch/{developerId}", null);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DispatchResult>();
                return result?.WhatsAppLink;
            }
            return null;
        }
        catch { return null; }
    }

    public async Task<List<DeveloperDto>> GetDevelopersAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<DeveloperDto>>("api/admin/consultation/developers") ?? new();
        }
        catch { return new(); }
    }

    private record DispatchResult(string WhatsAppLink, string Message);
}
