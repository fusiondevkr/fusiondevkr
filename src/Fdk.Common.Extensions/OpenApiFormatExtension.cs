using System;

using Microsoft.OpenApi;

namespace Fdk.Common.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="OpenApiFormat"/> class.
    /// </summary>
    public static class OpenApiFormatExtension
    {
        /// <summary>
        /// Gets the content type.
        /// </summary>
        /// <param name="format"><see cref="OpenApiFormat"/> value.</param>
        /// <returns>The content type.</returns>
        public static string GetContentType(this OpenApiFormat format)
        {
            switch (format)
            {
                case OpenApiFormat.Json:
                    return "application/json";

                case OpenApiFormat.Yaml:
                    return "application/yaml";

                default:
                    throw new InvalidOperationException("Invalid Open API format");
            }
        }
    }
}
