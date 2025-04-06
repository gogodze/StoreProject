using dotenv.net;
using Infrastructure.Db;
using Infrastructure.Services;
using MediatR;
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
builder.Services.AddScoped<IMediator>(o => o.GetRequiredService<IMediator>());
builder.Services
    .AddDbContext<StoreDbContext>(o => o
        .UseSqlite($"DATA SOURCE = {Environment.GetEnvironmentVariable("DB__PATH")}"));
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

// Configure the HTTP request pipeline.
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseHsts();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseAuthorization();

// disable warning
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});


app.MapControllerRoute(
    "default",
    "{controller=Auth}/{action=Login}");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseDeveloperExceptionPage();
}

app.Run();