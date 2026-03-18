using Consultoria.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Consultoria.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Problem> Problems { get; set; }
    public DbSet<ConsultationRequest> ConsultationRequests { get; set; }
    public DbSet<Developer> Developers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Problem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.EstimatedResponseTime).HasMaxLength(100);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.IconClass).HasMaxLength(100);
        });

        builder.Entity<ConsultationRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ClientName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientEmail).IsRequired().HasMaxLength(200);
            entity.HasOne(e => e.Problem)
                .WithMany(p => p.ConsultationRequests)
                .HasForeignKey(e => e.ProblemId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.AssignedDeveloper)
                .WithMany(d => d.ConsultationRequests)
                .HasForeignKey(e => e.AssignedDeveloperId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Developer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Specialization).HasMaxLength(200);
        });
    }
}
