using Application.Interfaces;
using dotenv.net;
using Infrastructure.Db;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


//get environment variables
var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
DotEnv.Fluent()
    .WithTrimValues()
    .WithEnvFiles($"{solutionDir}/.env")
    .WithOverwriteExistingVars()
    .Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services
    .AddDbContext<StoreDbContext>(o => o
        .UseSqlite($"DATA SOURCE = {Environment.GetEnvironmentVariable("DB__PATH")}"));
builder.Services.AddScoped<IAppDbContext, StoreDbContext>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.Application.Assembly));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
    x.TokenValidationParameters = JwtGenerator.TokenValidationParameters);
builder.Services.AddAuthorization();
builder.Services.AddCors(opt => opt.AddDefaultPolicy(cors =>
{
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
    cors.AllowAnyHeader();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// disable warning
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.Run();