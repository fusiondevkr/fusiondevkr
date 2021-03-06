using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Configurations;
using Fdk.FaceRecogniser.FunctionApp.Models;
using Fdk.FaceRecogniser.FunctionApp.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Fdk.FaceRecogniser.FunctionApp
{
    /// <summary>
    /// This represents the HTTP trigger entity to identify faces.
    /// </summary>
    public class GetFaceHttpTrigger
    {
        private readonly AppSettings _settings;
        private readonly IFaceService _face;
        private readonly ILogger<GetFaceHttpTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFaceHttpTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="face"><see cref="IFaceService"/> instance.</param>
        /// <param name="logger"><see cref="ILogger{GetFaceHttpTrigger}"/> instance.</param>
        public GetFaceHttpTrigger(AppSettings settings, IFaceService face, ILogger<GetFaceHttpTrigger> logger)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._face = face ?? throw new ArgumentNullException(nameof(face));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes to identify faces.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName(nameof(GetFaceHttpTrigger.GetLatestFaceFromEacyPersonGroup))]
        [OpenApiOperation(operationId: "Faces.GetLatestFaceFromEacyPersonGroup", tags: new[] { "face" }, Summary = "Get the latest face from all person groups", Description = "This operation gets the latest faces from all person groups.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(FaceCollectionResponse), Summary = "The collection of the face details", Description = "This defines the collection of the face details.")]
        public async Task<IActionResult> GetLatestFaceFromEacyPersonGroup(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.GET, Route = "api/faces")] HttpRequest req)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var faces = await this._face
                                  .GetAllFacesAsync()
                                  .ConfigureAwait(false);

            var collection = new FaceCollectionResponse(faces);

            return new OkObjectResult(collection);
        }

        /// <summary>
        /// Invokes to identify faces.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="personGroup">The person group name.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName(nameof(GetFaceHttpTrigger.GetFacesOfPersonGroup))]
        [OpenApiOperation(operationId: "Faces.GetFacesOfPersonGroup", tags: new[] { "face" }, Summary = "Get the latest face from all person groups", Description = "This operation gets the latest faces from all person groups.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter("personGroup", Type = typeof(string), Required = true, In = ParameterLocation.Path, Summary = "The person group name", Description = "This is the person group name.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(FaceCollectionResponse), Summary = "The collection of the face details", Description = "This defines the collection of the face details.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Summary = "No person group found", Description = "This is returned when no person group found.")]
        public async Task<IActionResult> GetFacesOfPersonGroup(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.GET, Route = "api/faces/{personGroup}")] HttpRequest req,
            string personGroup)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            if (personGroup.IsNullOrWhiteSpace())
            {
                return new NotFoundResult();
            }

            var faces = await this._face
                                  .GetAllFacesAsync(personGroup)
                                  .ConfigureAwait(false);

            if (!faces.Any())
            {
                return new NotFoundResult();
            }

            var collection = new FaceCollectionResponse(faces);

            return new OkObjectResult(collection);
        }
    }
}
