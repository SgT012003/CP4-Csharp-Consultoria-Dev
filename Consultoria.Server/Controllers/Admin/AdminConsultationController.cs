using Consultoria.Core.Interfaces;
using Consultoria.Shared.DTOs;
using Consultoria.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Consultoria.Server.Controllers.Admin;

[ApiController]
[Route("api/admin/consultation")]
[Authorize(Roles = "Admin")]
public class AdminConsultationController : ControllerBase
{
    private readonly IConsultationService _consultationService;
    private readonly IDeveloperService _developerService;

    public AdminConsultationController(IConsultationService consultationService, IDeveloperService developerService)
    {
        _consultationService = consultationService;
        _developerService = developerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsultationRequestDto>>> GetAll()
        => Ok(await _consultationService.GetAllRequestsAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultationRequestDto>> GetById(int id)
    {
        var result = await _consultationService.GetRequestByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateConsultationRequestDto dto)
    {
        try
        {
            await _consultationService.UpdateRequestStatusAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{requestId:int}/dispatch/{developerId:int}")]
    public async Task<ActionResult<object>> Dispatch(int requestId, int developerId)
    {
        try
        {
            var whatsAppLink = await _consultationService.DispatchToDeveloperAsync(requestId, developerId);
            return Ok(new { whatsAppLink, message = "Solicitação encaminhada com sucesso." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("developers")]
    public async Task<ActionResult<IEnumerable<DeveloperDto>>> GetDevelopers()
        => Ok(await _developerService.GetAllDevelopersAsync());
}
