using Consultoria.Core.Interfaces;
using Consultoria.Infrastructure.Data;
using Consultoria.Infrastructure.Entities;
using Consultoria.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Consultoria.Infrastructure.Services;

public class ProblemService : IProblemService
{
    private readonly ApplicationDbContext _context;

    public ProblemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProblemDto>> GetAllProblemsAsync()
    {
        return await _context.Problems
            .Where(p => p.IsActive)
            .Select(p => ToDto(p))
            .ToListAsync();
    }

    public async Task<ProblemDto?> GetProblemByIdAsync(int id)
    {
        var p = await _context.Problems.FindAsync(id);
        return p == null ? null : ToDto(p);
    }

    public async Task<ProblemDto> CreateProblemAsync(ProblemDto dto)
    {
        var entity = new Problem
        {
            Name = dto.Name,
            Description = dto.Description,
            EstimatedResponseTime = dto.EstimatedResponseTime,
            Category = dto.Category,
            IconClass = dto.IconClass,
        };
        _context.Problems.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateProblemAsync(ProblemDto dto)
    {
        var entity = await _context.Problems.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Problem {dto.Id} not found");
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.EstimatedResponseTime = dto.EstimatedResponseTime;
        entity.Category = dto.Category;
        entity.IconClass = dto.IconClass;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProblemAsync(int id)
    {
        var entity = await _context.Problems.FindAsync(id)
            ?? throw new KeyNotFoundException($"Problem {id} not found");
        entity.IsActive = false; // Soft delete
        await _context.SaveChangesAsync();
    }

    private static ProblemDto ToDto(Problem p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        EstimatedResponseTime = p.EstimatedResponseTime,
        Category = p.Category,
        IconClass = p.IconClass
    };
}
