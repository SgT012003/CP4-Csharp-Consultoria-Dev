using Consultoria.Core.Interfaces;
using Consultoria.Infrastructure.Data;
using Consultoria.Infrastructure.Entities;
using Consultoria.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Consultoria.Infrastructure.Services;

public class DeveloperService : IDeveloperService
{
    private readonly ApplicationDbContext _context;

    public DeveloperService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DeveloperDto>> GetAllDevelopersAsync()
    {
        return await _context.Developers
            .Select(d => ToDto(d))
            .ToListAsync();
    }

    public async Task<DeveloperDto?> GetDeveloperByIdAsync(int id)
    {
        var d = await _context.Developers.FindAsync(id);
        return d == null ? null : ToDto(d);
    }

    public async Task<DeveloperDto> CreateDeveloperAsync(DeveloperDto dto)
    {
        var entity = new Developer
        {
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email,
            Specialization = dto.Specialization,
            IsAvailable = dto.IsAvailable
        };
        _context.Developers.Add(entity);
        await _context.SaveChangesAsync();
        dto.Id = entity.Id;
        return dto;
    }

    public async Task UpdateDeveloperAsync(DeveloperDto dto)
    {
        var entity = await _context.Developers.FindAsync(dto.Id)
            ?? throw new KeyNotFoundException($"Developer {dto.Id} not found");
        entity.Name = dto.Name;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.Specialization = dto.Specialization;
        entity.IsAvailable = dto.IsAvailable;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDeveloperAsync(int id)
    {
        var entity = await _context.Developers.FindAsync(id)
            ?? throw new KeyNotFoundException($"Developer {id} not found");
        _context.Developers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static DeveloperDto ToDto(Developer d) => new()
    {
        Id = d.Id,
        Name = d.Name,
        Phone = d.Phone,
        Email = d.Email,
        Specialization = d.Specialization,
        IsAvailable = d.IsAvailable
    };
}
