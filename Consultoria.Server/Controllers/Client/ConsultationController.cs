using Consultoria.Core.Interfaces;
using Consultoria.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Consultoria.Server.Controllers.Client;

[ApiController]
[Route("api/client/consultation")]
public class ConsultationController : ControllerBase
{
    private readonly IConsultationService _consultationService;

    public ConsultationController(IConsultationService consultationService)
    {
        _consultationService = consultationService;
    }

    /// <summary>Submit a new consultation request (public — no auth required)</summary>
    [HttpPost]
    public async Task<ActionResult<ConsultationRequestDto>> Submit([FromBody] CreateConsultationRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.ClientName) || string.IsNullOrWhiteSpace(dto.ClientEmail))
            return BadRequest(new { message = "Nome e email são obrigatórios." });

        try
        {
            var result = await _consultationService.CreateRequestAsync(dto);
            return CreatedAtAction(nameof(Submit), new { id = result.Id }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
