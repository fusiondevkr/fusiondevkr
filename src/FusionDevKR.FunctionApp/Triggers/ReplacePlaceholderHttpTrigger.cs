using System.Net;
using System.Threading.Tasks;

using FusionDevKR.FunctionApp.Extensions;
using FusionDevKR.FunctionApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace FusionDevKR.FunctionApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to replace placeholders with their respective replacements.
    /// </summary>
    public class ReplacePlaceholderHttpTrigger
    {
        /// <summary>
        /// Invokes the replace method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the replaced message.</returns>
        [FunctionName(nameof(ReplacePlaceholderHttpTrigger.ReplaceAsync))]
        [OpenApiOperation(operationId: "placeholder.replace", tags: new[] { "placeholder" }, Summary = "Replace placeholder", Description = "This endpoint replaces the placeholders with corresponding values.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The API key for the function endpoint.")]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(PlaceholderReplaceRequest), Required = true, Description = "This is the request payload for the placeholder replace request.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(PlaceholderReplaceResponse), Summary = "Response payload that the replaced message is included", Description = "Response payload that the replaced message is included")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> ReplaceAsync(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = "placeholders/replace")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<PlaceholderReplaceRequest>().ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var result = request.Replace();

            var response = new PlaceholderReplaceResponse() { Result = result };

            var res = new OkObjectResult(response);

            return res;
        }
    }
}
