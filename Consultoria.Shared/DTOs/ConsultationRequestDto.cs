using Consultoria.Shared.Enums;

namespace Consultoria.Shared.DTOs;

public class ConsultationRequestDto
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public int ProblemId { get; set; }
    public string ProblemName { get; set; } = string.Empty;
    public RequestStatus Status { get; set; } = RequestStatus.Pending;
    public DateTime CreatedAt { get; set; }
    public string? AssignedDeveloperName { get; set; }
    public string? AssignedDeveloperPhone { get; set; }
    public string? AdminNotes { get; set; }
}

public class CreateConsultationRequestDto
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public int ProblemId { get; set; }
}

public class UpdateConsultationRequestDto
{
    public RequestStatus Status { get; set; }
    public string? AdminNotes { get; set; }
    public int? AssignedDeveloperId { get; set; }
}
