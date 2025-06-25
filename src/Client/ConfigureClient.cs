using Application;
using Blazored.Toast;

namespace Client;

public class ConfigureClient : ConfigurationBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddBlazoredToast();
    }
}