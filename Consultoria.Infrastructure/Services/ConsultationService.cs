using Consultoria.Core.Interfaces;
using Consultoria.Infrastructure.Data;
using Consultoria.Infrastructure.Entities;
using Consultoria.Shared.DTOs;
using Consultoria.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Consultoria.Infrastructure.Services;

public class ConsultationService : IConsultationService
{
    private readonly ApplicationDbContext _context;
    private readonly IWhatsAppService _whatsAppService;

    public ConsultationService(ApplicationDbContext context, IWhatsAppService whatsAppService)
    {
        _context = context;
        _whatsAppService = whatsAppService;
    }

    public async Task<IEnumerable<ConsultationRequestDto>> GetAllRequestsAsync()
    {
        return await _context.ConsultationRequests
            .Include(r => r.Problem)
            .Include(r => r.AssignedDeveloper)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => ToDto(r))
            .ToListAsync();
    }

    public async Task<ConsultationRequestDto?> GetRequestByIdAsync(int id)
    {
        var r = await _context.ConsultationRequests
            .Include(r => r.Problem)
            .Include(r => r.AssignedDeveloper)
            .FirstOrDefaultAsync(r => r.Id == id);
        return r == null ? null : ToDto(r);
    }

    public async Task<ConsultationRequestDto> CreateRequestAsync(CreateConsultationRequestDto dto)
    {
        var problem = await _context.Problems.FindAsync(dto.ProblemId)
            ?? throw new KeyNotFoundException($"Problem {dto.ProblemId} not found");

        var entity = new ConsultationRequest
        {
            ClientName = dto.ClientName,
            ClientEmail = dto.ClientEmail,
            ProblemId = dto.ProblemId,
            Status = RequestStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.ConsultationRequests.Add(entity);
        await _context.SaveChangesAsync();

        entity.Problem = problem;
        return ToDto(entity);
    }

    public async Task UpdateRequestStatusAsync(int id, UpdateConsultationRequestDto dto)
    {
        var entity = await _context.ConsultationRequests.FindAsync(id)
            ?? throw new KeyNotFoundException($"Request {id} not found");

        entity.Status = dto.Status;
        entity.AdminNotes = dto.AdminNotes;
        entity.UpdatedAt = DateTime.UtcNow;

        if (dto.AssignedDeveloperId.HasValue)
            entity.AssignedDeveloperId = dto.AssignedDeveloperId;

        await _context.SaveChangesAsync();
    }

    public async Task<string> DispatchToDeveloperAsync(int requestId, int developerId)
    {
        var request = await _context.ConsultationRequests
            .Include(r => r.Problem)
            .FirstOrDefaultAsync(r => r.Id == requestId)
            ?? throw new KeyNotFoundException($"Request {requestId} not found");

        var developer = await _context.Developers.FindAsync(developerId)
            ?? throw new KeyNotFoundException($"Developer {developerId} not found");

        request.AssignedDeveloperId = developerId;
        request.Status = RequestStatus.Dispatched;
        request.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var message = $"Olá {developer.Name}! Você recebeu uma nova solicitação de consultoria.\n\n" +
                      $"Problema: {request.Problem.Name}\n" +
                      $"Cliente: {request.ClientName}\n" +
                      $"Email: {request.ClientEmail}\n\n" +
                      $"Por favor, entre em contato o mais breve possível.";

        return _whatsAppService.GenerateWhatsAppLink(developer.Phone, message);
    }

    private static ConsultationRequestDto ToDto(ConsultationRequest r) => new()
    {
        Id = r.Id,
        ClientName = r.ClientName,
        ClientEmail = r.ClientEmail,
        ProblemId = r.ProblemId,
        ProblemName = r.Problem?.Name ?? string.Empty,
        Status = r.Status,
        CreatedAt = r.CreatedAt,
        AdminNotes = r.AdminNotes,
        AssignedDeveloperName = r.AssignedDeveloper?.Name,
        AssignedDeveloperPhone = r.AssignedDeveloper?.Phone
    };
}
