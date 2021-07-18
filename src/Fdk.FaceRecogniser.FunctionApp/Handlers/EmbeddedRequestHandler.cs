using System;
using System.IO;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Models;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Fdk.FaceRecogniser.FunctionApp.Handlers
{
    public interface IEmbeddedRequestHandler
    {
        /// <summary>
        /// Gets or sets the raw form of the payload.
        /// </summary>
        string RawData { get; set; }

        /// <summary>
        /// Gets or sets the person group from the request payload.
        /// </summary>
        string PersonGroup { get; set; }

        /// <summary>
        /// Gets or sets the content type of the embedded request payload.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the embedded image data.
        /// </summary>
        byte[] Body { get; set; }

        /// <summary>
        /// Gets or sets the filename of the embedded image data.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// Processes the embedded request payload for further processing.
        /// </summary>
        /// <returns>Returns the <see cref="IEmbeddedRequestHandler"/> instance.</returns>
        Task<IEmbeddedRequestHandler> ProcessAsync(Stream stream);
    }

    /// <summary>
    /// This represents the entity for the embedded request from the front-end.
    /// </summary>
    public class EmbeddedRequestHandler : IEmbeddedRequestHandler
    {
        private readonly ILogger<IEmbeddedRequestHandler> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedRequestHandler"/> class.
        /// </summary>
        /// <param name="payload"><see cref="Stream"/> instance.</param>
        public EmbeddedRequestHandler(ILogger<IEmbeddedRequestHandler> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public virtual string RawData { get; set; }

        /// <inheritdoc />
        public virtual string PersonGroup { get; set; }

        /// <inheritdoc />
        public virtual string ContentType { get; set; }

        /// <inheritdoc />
        public virtual byte[] Body { get; set; }

        /// <inheritdoc />
        public virtual string Filename { get; set; }

        /// <inheritdoc />
        public async Task<IEmbeddedRequestHandler> ProcessAsync(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var payload = default(string);
            var request = default(EmbeddedRequest);
            using (var reader = new StreamReader(stream))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
                request = JsonConvert.DeserializeObject<EmbeddedRequest>(payload);
            }

            this.RawData = payload;

            var personGroup = request.PersonGroup;
            this.PersonGroup = personGroup;

            var image = request.Image;

            var segments = image.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var contentType = segments[0].Split(new[] { ":", ";" }, StringSplitOptions.RemoveEmptyEntries)[1];
            this.ContentType = contentType;

            var encoded = segments[1];
            var bytes = Convert.FromBase64String(encoded);
            this.Body = bytes;

            var filename = $"{personGroup}/{Guid.NewGuid().ToString()}.png";
            this.Filename = filename;

            return this;
        }
    }
}
