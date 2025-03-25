using System.Reflection;
using dotenv.net;
using Infrastructure.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent;
DotEnv.Fluent()
    .WithTrimValues()
    .WithEnvFiles($"{solutionDir}/.env")
    .WithOverwriteExistingVars()
    .Load();

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMediator>(o => o.GetRequiredService<IMediator>());
builder.Services
    .AddDbContext<StoreDbContext>(o => o
        .UseSqlite($"DATA SOURCE = {Environment.GetEnvironmentVariable("DB__PATH")} "));
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1",
    new OpenApiInfo { Title = "Store API", Version = "v1" }));

builder.Services.AddCors(opt => opt.AddDefaultPolicy(cors =>
{
    cors.AllowAnyMethod();
    cors.AllowAnyOrigin();
    cors.AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.MapControllers();
app.UseHttpsRedirection();

app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; 
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store API V1");
    });
}

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.MapControllerRoute(
    "default",
    "{controller=Product}/{action=Index}/{id?}");

app.Run();