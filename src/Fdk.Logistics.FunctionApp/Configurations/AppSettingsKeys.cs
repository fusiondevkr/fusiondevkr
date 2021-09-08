namespace Fdk.Logistics.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the constants entity for app settings.
    /// </summary>
    public static class AppSettingsKeys
    {
        /// <summary>
        /// Identifies Azure Storage Account connection string key.
        /// </summary>
        public const string StorageAccountKey = "AzureWebJobsStorage";

        /// <summary>
        /// Identifies Azure Application Insights connectionstring key.
        /// </summary>
        public const string AppInsightsKey = "APPLICATIONINSIGHTS_CONNECTION_STRING";

        /// <summary>
        /// Identifies the OpenApi key.
        /// </summary>
        public const string OpenApiKey = "OpenApi";

        /// <summary>
        /// Identifies the OpenAPI version key.
        /// </summary>
        public const string OpenApiVersionKey = "OpenApi__Version";

        /// <summary>
        /// Identifies the OpenAPI document version key.
        /// </summary>
        public const string OpenApiDocVersionKey = "OpenApi__DocVersion";

        /// <summary>
        /// Identifies the OpenAPI document title key.
        /// </summary>
        public const string OpenApiDocTitleKey = "OpenApi__DocTitle";
    }
}
