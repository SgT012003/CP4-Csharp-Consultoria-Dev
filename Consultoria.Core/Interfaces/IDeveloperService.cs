using Consultoria.Shared.DTOs;

namespace Consultoria.Core.Interfaces;

public interface IDeveloperService
{
    Task<IEnumerable<DeveloperDto>> GetAllDevelopersAsync();
    Task<DeveloperDto?> GetDeveloperByIdAsync(int id);
    Task<DeveloperDto> CreateDeveloperAsync(DeveloperDto dto);
    Task UpdateDeveloperAsync(DeveloperDto dto);
    Task DeleteDeveloperAsync(int id);
}
