using Consultoria.Core.Interfaces;
using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Consultoria.Server.Controllers.Client;

[ApiController]
[Route("api/client/problems")]
public class ProblemsController : ControllerBase
{
    private readonly IProblemService _problemService;

    public ProblemsController(IProblemService problemService)
    {
        _problemService = problemService;
    }

    /// <summary>Get all active problems (public endpoint)</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProblemDto>>> GetAll()
    {
        var problems = await _problemService.GetAllProblemsAsync();
        return Ok(problems);
    }

    /// <summary>Get a specific problem by ID (public endpoint)</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProblemDto>> GetById(int id)
    {
        var problem = await _problemService.GetProblemByIdAsync(id);
        if (problem == null) return NotFound();
        return Ok(problem);
    }
}
