using Fdk.M365Register.ApiApp.Configurations;
using Fdk.M365Register.ApiApp.Triggers;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Fdk.M365Register.ApiApp.Startup))]
namespace Fdk.M365Register.ApiApp
{
    /// <summary>
    /// This represents the startup entity for the runtime initialisation.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureAppSettings(builder.Services);
            this.ConfigureHttpClient(builder.Services);
            this.ConfigureClients(builder.Services);
            this.ConfigureServices(builder.Services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();
        }

        private void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient<RegistrationFlowHttpTrigger>();
        }

        private void ConfigureClients(IServiceCollection services)
        {
        }

        private void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
