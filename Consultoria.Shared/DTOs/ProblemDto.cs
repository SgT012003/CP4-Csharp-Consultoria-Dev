namespace Consultoria.Shared.DTOs;

public class ProblemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EstimatedResponseTime { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string IconClass { get; set; } = "bi-code-slash";
}
