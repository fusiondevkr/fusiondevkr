using System;
using Microsoft.OpenApi;

namespace Fdk.FaceRecogniser.FunctionApp.Handlers
{
    /// <summary>
    /// This provides interfaces to the <see cref="OpenApiDocumentHandler" /> class.
    /// </summary>
    public interface IOpenApiDocumentHandler
    {
        /// <summary>
        /// Gets the <see cref="OpenApiSpecVersion"/> instance.
        /// </summary>
        /// <param name="version">Open API version.</param>
        /// <returns>Returns the <see cref="OpenApiSpecVersion"/> instance.</returns>
        OpenApiSpecVersion GetVersion(string version);

        /// <summary>
        /// Gets the <see cref="OpenApiFormat"/> instance.
        /// </summary>
        /// <param name="format">Open API document format - either YAML or JSON.</param>
        /// <returns>Returns the <see cref="OpenApiFormat"/> instance.</returns>
        OpenApiFormat GetFormat(string format);
    }

    /// <summary>
    /// This represents the handler entity for Open API document.
    /// </summary>
    public class OpenApiDocumentHandler : IOpenApiDocumentHandler
    {
        /// <inheritdoc />
        public OpenApiSpecVersion GetVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            if (version.Equals("v2", StringComparison.CurrentCultureIgnoreCase))
            {
                return OpenApiSpecVersion.OpenApi2_0;
            }

            if (version.Equals("v3", StringComparison.CurrentCultureIgnoreCase))
            {
                return OpenApiSpecVersion.OpenApi3_0;
            }

            throw new InvalidOperationException("Invalid Open API version");
        }

        /// <inheritdoc />
        public OpenApiFormat GetFormat(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentNullException(nameof(format));
            }

            return Enum.TryParse<OpenApiFormat>(format, true, out OpenApiFormat result)
                       ? result
                       : throw new InvalidOperationException("Invalid Open API format");
        }
    }
}
