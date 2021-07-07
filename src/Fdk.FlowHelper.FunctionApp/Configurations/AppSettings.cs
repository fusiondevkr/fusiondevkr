using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;

namespace Fdk.FlowHelper.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the app settings entity.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="StorageAccountSettings"/> object.
        /// </summary>
        public virtual StorageAccountSettings StorageAccount { get; set; } = new StorageAccountSettings();

        /// <summary>
        /// Gets or sets the <see cref="ApplicationInsightsSettings"/> object.
        /// </summary>
        public virtual ApplicationInsightsSettings ApplicationInsights { get; set; } = new ApplicationInsightsSettings();

        /// <summary>
        /// Gets the <see cref="OpenApiSettings"/> object.
        /// </summary>
        public virtual OpenApiSettings OpenApi { get; } = new OpenApiSettings();
    }

    /// <summary>
    /// This represents the app settings entity for Azure Storage Account.
    /// </summary>
    public class StorageAccountSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.StorageAccountKey) ?? string.Empty;
    }

    /// <summary>
    /// This represents the app settings entity for Azure Application Insights.
    /// </summary>
    public class ApplicationInsightsSettings
    {
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public virtual string ConnectionString { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.AppInsightsKey) ?? string.Empty;
    }

    /// <summary>
    /// This represents the app settings entity for OpenAPI.
    /// </summary>
    public class OpenApiSettings
    {
        /// <summary>
        /// Gets or sets the <see cref="OpenApiVersionType"/> value.
        /// </summary>
        public virtual OpenApiVersionType Version { get; set; } = Enum.TryParse<OpenApiVersionType>(
                                                                       Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiVersionKey), ignoreCase: true, out var result)
                                                                     ? result
                                                                     : OpenApiConfigurationOptions.DefaultVersion();
        /// <summary>
        /// Gets or sets the OpenAPI document version.
        /// </summary>
        public virtual string DocumentVersion { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocVersionKey) ?? OpenApiConfigurationOptions.DefaultDocVersion();

        /// <summary>
        /// Gets or sets the OpenAPI document title.
        /// </summary>
        public virtual string DocumentTitle { get; set; } = Environment.GetEnvironmentVariable(AppSettingsKeys.OpenApiDocTitleKey) ?? OpenApiConfigurationOptions.DefaultDocTitle(typeof(AppSettings));
    }
}
