using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Configurations;
using Fdk.FaceRecogniser.FunctionApp.Extensions;
using Fdk.FaceRecogniser.FunctionApp.Handlers;
using Fdk.FaceRecogniser.FunctionApp.Models;
using Fdk.FaceRecogniser.FunctionApp.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp
{
    /// <summary>
    /// This represents the HTTP trigger entity to identify faces.
    /// </summary>
    public class IdentifyFaceHttpTrigger
    {
        private readonly AppSettings _settings;
        private readonly IBlobService _blob;
        private readonly IFaceService _face;
        private readonly IFaceIdentificationRequestHandler _handler;
        private readonly ILogger<IdentifyFaceHttpTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentifyFaceHttpTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="blob"><see cref="IBlobService"/> instance.</param>
        /// <param name="face"><see cref="IFaceService"/> instance.</param>
        /// <param name="handler"><see cref="IFaceIdentificationRequestHandler"/> instance.</param>
        /// <param name="logger"><see cref="ILogger{IdentifyFaceHttpTrigger}"/> instance.</param>
        public IdentifyFaceHttpTrigger(AppSettings settings, IBlobService blob, IFaceService face, IFaceIdentificationRequestHandler handler, ILogger<IdentifyFaceHttpTrigger> logger)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._blob = blob ?? throw new ArgumentNullException(nameof(blob));
            this._face = face ?? throw new ArgumentNullException(nameof(face));
            this._handler = handler ?? throw new ArgumentNullException(nameof(handler));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes to identify faces.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName(nameof(IdentifyFaceHttpTrigger.Identify))]
        [OpenApiOperation(operationId: "Faces.Identify", tags: new[] { "face" }, Summary = "Identify face", Description = "This operation identifies face taken from the app.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(FaceIdentificationRequest), Description = "This defines the embedded image data with person group the image belongs.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(FaceIdentificationResponse), Summary = "Face identification result", Description = "This defines the face identification result.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: ContentTypes.ApplicationJson, bodyType: typeof(FaceIdentificationResponse), Summary = "Face identification result", Description = "This defines the face identification result is not what is expected.")]
        public async Task<IActionResult> Identify(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = "api/faces/identify")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var request = await this._handler
                                    .ProcessAsync(req.Body)
                                    .ConfigureAwait(false);

            var blobs = await this._blob
                                  .GetFacesAsync(this._handler.PersonGroup, this._settings.Blob.NumberOfPhotos)
                                  .ConfigureAwait(false);

            var uploaded = await this._blob
                                     .UploadAsync(this._handler.Body, this._handler.Filename, this._handler.ContentType)
                                     .ConfigureAwait(false);

            var faces = await this._face
                                  .DetectFacesAsync(uploaded)
                                  .ConfigureAwait(false);

            var response = default(FaceIdentificationResponse);
            if (!this.HasOneFaceDetected(faces))
            {
                await this._blob
                          .DeleteAsync(this._handler.Filename)
                          .ConfigureAwait(false);

                response = new FaceIdentificationResponse("Too many faces or no face detected") { Timestamp = DateTimeOffset.UtcNow };
                return new BadRequestObjectResult(response);
            }

            if (!this.HasEnoughPhotos(blobs))
            {
                response = new FaceIdentificationResponse($"Need {this._settings.Blob.NumberOfPhotos - blobs.Count} more photo(s).") { Timestamp = DateTimeOffset.UtcNow };;
                return new OkObjectResult(response);
            }

            var identified = await this._face
                                       .WithPersonGroup(this._handler.PersonGroup)
                                       .WithPerson(blobs)
                                       .TrainFacesAsync()
                                       .IdentifyFaceAsync(uploaded)
                                       .UpdateFaceIdentificationAsync()
                                       .ConfigureAwait(false);

            if (this.IsLessConfident(identified))
            {
                await this._blob
                          .DeleteAsync(this._handler.Filename)
                          .ConfigureAwait(false);

                response = new FaceIdentificationResponse($"Face not identified: {identified.Confidence:0.00}")
                {
                    Confidence = Convert.ToDecimal(Math.Round(identified.Confidence, 2)),
                    IsIdentified = false,
                    Timestamp = identified.Timestamp,
                };
                return new BadRequestObjectResult(response);
            }

            response = new FaceIdentificationResponse($"Face identified: {identified.Confidence:0.00}")
            {
                Confidence = Convert.ToDecimal(Math.Round(identified.Confidence, 2)),
                IsIdentified = true,
                Timestamp = identified.Timestamp,
            };
            return new OkObjectResult(response);
        }

        private bool HasOneFaceDetected(List<DetectedFace> faces)
        {
            return faces.Count == 1;
        }

        private bool HasEnoughPhotos(List<CloudBlockBlob> blobs)
        {
            return blobs.Count >= this._settings.Blob.NumberOfPhotos;
        }

        private bool IsLessConfident(FaceEntity identified)
        {
            return identified.Confidence < this._settings.Face.Confidence;
        }
    }
}
