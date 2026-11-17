using Application.Services;
using Blazored.Toast;
using Client.Auth;
using Client.Components;
using Domain.Common;
using Domain.ValueObjects;
using dotenv.net;
using EntityFrameworkCore.DataProtection.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//get environment variables
DotEnv.Fluent()
    .WithTrimValues()
    .WithOverwriteExistingVars().WithProbeForEnv(6)
    .Load();
builder.Services.AddBlazoredToast();
builder.Services.AddDataProtectionServices("StoreProject")
    .PersistKeysToFileSystem(new DirectoryInfo
        ("DATAPROTECTION__KEYS__PATH".GetFromEnvRequired()));

builder.Services
    .AddDbContext<IAppDbContext, StoreDbContext>(o =>
    {
        o.AddDataProtectionInterceptors();
        var dbPath = "DB__PATH".GetFromEnvRequired();
        o.UseSqlite($"DATA SOURCE = {dbPath}");
    });


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.Application.Assembly));

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.MapInboundClaims = false;
    x.TokenValidationParameters = JwtGenerator.TokenValidationParameters;
    x.Events = JwtGenerator.Events;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", p => p.RequireRole(nameof(Role.Administrator)));
    options.AddPolicy("Staff", p => p.RequireRole(
        nameof(Role.Administrator),
        nameof(Role.ProductManager),
        nameof(Role.Support)));
});
builder.Services.AddCors(opt => opt.AddDefaultPolicy(cors =>
{
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
    cors.AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddAuthorizationCore();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapAuthEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await AdminBootstrap.EnsureAdministratorAsync(app.Services);

app.Run();