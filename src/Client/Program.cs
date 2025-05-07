using Application.Services;
using Blazored.Toast;
using Client.Components;
using Domain.Common;
using dotenv.net;
using Infrastructure.Persistence.Db;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//get environment variables
DotEnv.Fluent()
    .WithTrimValues()
    .WithOverwriteExistingVars().WithProbeForEnv(6)
    .Load();
builder.Services.AddBlazoredToast();
builder.Services
    .AddDbContext<IAppDbContext, StoreDbContext>(o =>
    {
        var dbPath = "DB__PATH".GetFromEnvRequired();
        o.UseSqlite($"DATA SOURCE = {dbPath}");
    });
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.Application.Assembly));

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = JwtGenerator.TokenValidationParameters;
    x.Events = JwtGenerator.Events;
});
builder.Services.AddAuthorization();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(cors =>
{
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
    cors.AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();