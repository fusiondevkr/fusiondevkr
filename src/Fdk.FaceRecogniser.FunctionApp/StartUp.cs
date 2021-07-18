using System;

using Fdk.FaceRecogniser.FunctionApp.Configurations;
using Fdk.FaceRecogniser.FunctionApp.Handlers;
using Fdk.FaceRecogniser.FunctionApp.Services;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

[assembly: FunctionsStartup(typeof(Fdk.FaceRecogniser.FunctionApp.StartUp))]
namespace Fdk.FaceRecogniser.FunctionApp
{
    /// <summary>
    /// This represents the entity as an IoC container.
    /// </summary>
    public class StartUp : FunctionsStartup
    {

        /// <inheritdoc/>
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.ConfigureAppSettings(builder.Services);
            this.ConfigureHttpClient(builder.Services);
            this.ConfigureStorage(builder.Services);
            this.ConfigureFaceClient(builder.Services);
            this.ConfigureServices(builder.Services);
            this.ConfigureHandlers(builder.Services);
        }

        private void ConfigureAppSettings(IServiceCollection services)
        {
            // var settings = new AppSettings();

            services.AddSingleton<AppSettings>();
        }

        private void ConfigureHttpClient(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        private void ConfigureStorage(IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable(AppSettingsKeys.StorageAccountKey);
            var storage = CloudStorageAccount.Parse(connectionString);
            var blob = storage.CreateCloudBlobClient();
            var table = storage.CreateCloudTableClient();

            services.AddSingleton<CloudBlobClient>(blob);
            services.AddSingleton<CloudTableClient>(table);
        }

        private void ConfigureFaceClient(IServiceCollection services)
        {
            services.AddSingleton<IFaceClient>(p => {
                var settings = p.GetService<AppSettings>();

                var authKey = settings.Face.AuthKey;
                var endpoint = settings.Face.Endpoint;
                var credentials = new ApiKeyServiceClientCredentials(authKey);
                var face = new FaceClient(credentials) { Endpoint = endpoint };

                return face;
            });
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBlobService, BlobService>();
            services.AddTransient<IFaceService, FaceService>();
        }

        private void ConfigureHandlers(IServiceCollection services)
        {
            services.AddTransient<IFaceIdentificationRequestHandler, FaceIdentificationRequestHandler>();
        }
    }
}
