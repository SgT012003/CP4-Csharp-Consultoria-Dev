using Consultoria.Infrastructure.Data;
using Consultoria.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Consultoria.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Apply any pending migrations automatically
        await context.Database.MigrateAsync();

        // Seed roles
        string[] roles = { "Admin", "Client" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Seed admin user
        const string adminEmail = "admin@consultoria.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Tech Líder Admin",
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(admin, "Admin@123!");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Seed sample developers
        if (!context.Developers.Any())
        {
            context.Developers.AddRange(
                new Developer { Name = "Diogo Julio", Phone = "5511999990001", Email = "djulio@consultoria.com", Specialization = "Backend .NET", IsAvailable = true },
                new Developer { Name = "Matheus Zottis", Phone = "5511999990002", Email = "mzottis@consultoria.com", Specialization = "Frontend Blazor", IsAvailable = true },
                new Developer { Name = "Jonata Rafael", Phone = "5511999990003", Email = "jrafael@consultoria.com", Specialization = "DevOps", IsAvailable = false },
                new Developer { Name = "Victor Didoff ", Phone = "5511999990004", Email = "vdidoff@consultoria.com", Specialization = "DevOps & Docker", IsAvailable = true },
                new Developer { Name = "Vinicius Silva", Phone = "5511999990005", Email = "vsilva@consultoria.com", Specialization = "DevOps & Frontend Blazor", IsAvailable = true }
            );
            await context.SaveChangesAsync();
        }

        // Seed sample problems
        if (!context.Problems.Any())
        {
            context.Problems.AddRange(
                new Problem
                {
                    Name = "Performance em APIs",
                    Description = "Análise e otimização de endpoints lentos em aplicações ASP.NET Core. Identificamos gargalos, implementamos caching, otimizamos queries e configuramos profiling.",
                    EstimatedResponseTime = "2-4 horas",
                    Category = "Backend",
                    IconClass = "bi-speedometer2"
                },
                new Problem
                {
                    Name = "Arquitetura de Microsserviços",
                    Description = "Consultoria para migração de monolito para microsserviços ou revisão de arquitetura existente. Includes Docker, Kubernetes e service mesh.",
                    EstimatedResponseTime = "1-3 dias",
                    Category = "Arquitetura",
                    IconClass = "bi-diagram-3"
                },
                new Problem
                {
                    Name = "Segurança e Autenticação",
                    Description = "Implementação de autenticação JWT, OAuth2, claims-based authorization e revisão de vulnerabilidades OWASP em aplicações web.",
                    EstimatedResponseTime = "4-8 horas",
                    Category = "Segurança",
                    IconClass = "bi-shield-lock"
                },
                new Problem
                {
                    Name = "CI/CD e DevOps",
                    Description = "Configuração de pipelines GitHub Actions / Azure DevOps, containerização com Docker e orquestração com Kubernetes ou Docker Compose.",
                    EstimatedResponseTime = "1-2 dias",
                    Category = "DevOps",
                    IconClass = "bi-gear-wide-connected"
                },
                new Problem
                {
                    Name = "Banco de Dados e ORM",
                    Description = "Otimização de queries, design de schema, configuração do Entity Framework Core, migrations e estratégias de indexação para SQL Server.",
                    EstimatedResponseTime = "3-6 horas",
                    Category = "Database",
                    IconClass = "bi-database"
                },
                new Problem
                {
                    Name = "UI/UX com Blazor",
                    Description = "Desenvolvimento de componentes Blazor reutilizáveis, integração com JavaScript, gestão de estado e design responsivo com Bootstrap.",
                    EstimatedResponseTime = "2-5 horas",
                    Category = "Frontend",
                    IconClass = "bi-palette"
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
