using Consultoria.Shared.DTOs;

namespace Consultoria.Core.Interfaces;

public interface IProblemService
{
    Task<IEnumerable<ProblemDto>> GetAllProblemsAsync();
    Task<ProblemDto?> GetProblemByIdAsync(int id);
    Task<ProblemDto> CreateProblemAsync(ProblemDto dto);
    Task UpdateProblemAsync(ProblemDto dto);
    Task DeleteProblemAsync(int id);
}
