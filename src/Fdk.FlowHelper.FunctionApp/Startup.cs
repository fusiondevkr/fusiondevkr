using System.Text.RegularExpressions;

using Fdk.FlowHelper.FunctionApp.Configurations;
using Fdk.FlowHelper.FunctionApp.Triggers;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Fdk.FlowHelper.FunctionApp.Startup))]
namespace Fdk.FlowHelper.FunctionApp
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
            var regex = new Regex(@"^[가-힣]+$");

            services.AddSingleton(regex);
        }
    }
}
