using Consultoria.Client;
using Consultoria.Client.Auth;
using Consultoria.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ─── HttpClient pointing to the Server ──────────────────────
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// ─── Authentication ──────────────────────────────────────────
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Client"));
});

// ─── Application Services ────────────────────────────────────
builder.Services.AddScoped<ProblemService>();
builder.Services.AddScoped<ConsultationService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
