using FusionDevKR.FunctionApp.Configurations;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FusionDevKR.FunctionApp.Startup))]
namespace FusionDevKR.FunctionApp
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
            this.ConfigureClients(builder.Services);
            this.ConfigureServices(builder.Services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            services.AddSingleton<AppSettings>();
        }

        private void ConfigureClients(IServiceCollection services)
        {
        }

        private void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
