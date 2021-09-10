using System;
using System.Reflection;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace Fdk.M365Register.ApiApp.Configurations
{
    /// <summary>
    /// This represents the options entity for OpenAPI metadata configuration.
    /// </summary>
    public class OpenApiConfigurationOptions : DefaultOpenApiConfigurationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenApiConfigurationOptions"/> class.
        /// </summary>
        public OpenApiConfigurationOptions()
            : base()
        {
            this.Info = new OpenApiInfo()
            {
                Version = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocVersionKey) ?? OpenApiConfigurationOptions.DefaultDocVersion(),
                Title = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocTitleKey) ?? OpenApiConfigurationOptions.DefaultDocTitle(typeof(OpenApiConfigurationOptions)),
                Description = "Interface that Fusion Dev Korea apps use for M365 account registration.",
                TermsOfService = new Uri("https://github.com/fusiondevkr/fusiondevkr"),
                Contact = new OpenApiContact()
                {
                    Name = "Fusion Dev Korea",
                    Url = new Uri("https://github.com/fusiondevkr/fusiondevkr/issues"),
                },
                License = new OpenApiLicense()
                {
                    Name = "MIT",
                    Url = new Uri("http://opensource.org/licenses/MIT"),
                }
            };

            this.OpenApiVersion = Enum.TryParse<OpenApiVersionType>(
                                        Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiVersionKey), ignoreCase: true, out var result)
                                      ? result
                                      : OpenApiConfigurationOptions.DefaultVersion();
        }

        /// <summary>
        /// Gets the default OpenAPI document version.
        /// </summary>
        /// <returns>Returns the default OpenAPI document version.</returns>
        public static string DefaultDocVersion()
        {
            return "1.0.0";
        }

        /// <summary>
        /// Gets the default OpenAPI document title - assembly name.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> instance.</param>
        /// <returns>Returns the default OpenAPI document title - assembly name.</returns>
        public static string DefaultDocTitle(Type type)
        {
            var assembly = Assembly.GetAssembly(type);

            return assembly.GetName().Name;
        }

        /// <summary>
        /// Gets the default OpenAPI version.
        /// </summary>
        /// <returns>Returns the default OpenAPI version.</returns>
        public static OpenApiVersionType DefaultVersion()
        {
            return OpenApiVersionType.V2;
        }
    }
}
