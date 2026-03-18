using Consultoria.Shared.Enums;

namespace Consultoria.Infrastructure.Entities;

public class ConsultationRequest
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public int ProblemId { get; set; }
    public Problem Problem { get; set; } = null!;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? AdminNotes { get; set; }
    public int? AssignedDeveloperId { get; set; }
    public Developer? AssignedDeveloper { get; set; }
}
