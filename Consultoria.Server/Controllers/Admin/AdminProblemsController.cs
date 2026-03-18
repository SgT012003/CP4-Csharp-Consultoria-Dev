using Consultoria.Core.Interfaces;
using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultoria.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/problems")]
[Authorize(Roles = "Admin")]
public class AdminProblemsController : ControllerBase
{
    private readonly IProblemService _problemService;

    public AdminProblemsController(IProblemService problemService)
    {
        _problemService = problemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProblemDto>>> GetAll()
        => Ok(await _problemService.GetAllProblemsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProblemDto>> GetById(int id)
    {
        var problem = await _problemService.GetProblemByIdAsync(id);
        if (problem == null) return NotFound();
        return Ok(problem);
    }

    [HttpPost]
    public async Task<ActionResult<ProblemDto>> Create([FromBody] ProblemDto dto)
    {
        var result = await _problemService.CreateProblemAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProblemDto dto)
    {
        dto.Id = id;
        try
        {
            await _problemService.UpdateProblemAsync(dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _problemService.DeleteProblemAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
