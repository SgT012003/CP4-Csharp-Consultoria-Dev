using Consultoria.Shared.DTOs;
using Consultoria.Shared.Enums;

namespace Consultoria.Core.Interfaces;

public interface IConsultationService
{
    Task<IEnumerable<ConsultationRequestDto>> GetAllRequestsAsync();
    Task<ConsultationRequestDto?> GetRequestByIdAsync(int id);
    Task<ConsultationRequestDto> CreateRequestAsync(CreateConsultationRequestDto dto);
    Task UpdateRequestStatusAsync(int id, UpdateConsultationRequestDto dto);
    Task<string> DispatchToDeveloperAsync(int requestId, int developerId);
}
