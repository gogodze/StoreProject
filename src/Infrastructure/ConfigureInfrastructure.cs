using Application;
using Application.Services;
using Domain.Common;
using EntityFrameworkCore.DataProtection.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public sealed class ConfigureInfrastructure : ConfigurationBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = JwtGenerator.TokenValidationParameters;
            x.Events = JwtGenerator.Events;
        });
        services.AddAuthorization();
        services.AddHttpContextAccessor();


        services.AddDataProtectionServices("StoreProject")
            .PersistKeysToFileSystem(new DirectoryInfo
                ("DATAPROTECTION__KEYS__PATH".GetFromEnvRequired()));

        services
            .AddDbContext<IAppDbContext, StoreDbContext>(o =>
            {
                o.AddDataProtectionInterceptors();
                var dbPath = "DB__PATH".GetFromEnvRequired();
                o.UseSqlite($"DATA SOURCE = {dbPath}");
            });

        services.AddScoped<ICookieService, CookieService>();
        services.AddScoped<ICurrentUserAccessor, HttpContextCurrentUserAccessor>();
        services.AddRazorComponents()
            .AddInteractiveServerComponents();
    }
}