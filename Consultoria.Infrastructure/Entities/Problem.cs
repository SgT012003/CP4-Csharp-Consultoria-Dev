using Consultoria.Shared.Enums;

namespace Consultoria.Infrastructure.Entities;

public class Problem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EstimatedResponseTime { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IconClass { get; set; } = "bi-code-slash";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();
}
