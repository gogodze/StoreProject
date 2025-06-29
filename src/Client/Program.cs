using Application;
using Client.Components;
using dotenv.net;
using FluentValidation;

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
ValidatorOptions.Global.LanguageManager.Enabled = true;

DotEnv.Fluent()
    .WithTrimValues()
    .WithOverwriteExistingVars().WithProbeForEnv(6)
    .Load();

var builder = WebApplication.CreateBuilder(args);

ConfigurationBase.ConfigureServicesFromAssemblies(builder.Services, [
    nameof(Domain), nameof(Application), nameof(Infrastructure), nameof(Client),
]);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();