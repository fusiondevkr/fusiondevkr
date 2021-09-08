using Fdk.CheckPointHelper.FunctionApp.Configurations;
using Fdk.CheckPointHelper.FunctionApp.Triggers;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Fdk.CheckPointHelper.FunctionApp.Startup))]
namespace Fdk.CheckPointHelper.FunctionApp
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
            services.AddHttpClient<CheckInFlowHttpTrigger>();
        }

        private void ConfigureClients(IServiceCollection services)
        {
        }

        private void ConfigureServices(IServiceCollection services)
        {
        }
    }
}
