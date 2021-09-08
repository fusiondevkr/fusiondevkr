using System.Net;
using System.Threading.Tasks;

using Fdk.Common.Extensions;
using Fdk.FlowHelper.FunctionApp.Examples;
using Fdk.FlowHelper.FunctionApp.Extensions;
using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Fdk.FlowHelper.FunctionApp.Triggers
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
        [OpenApiOperation(operationId: "placeholders.replace", tags: new[] { "placeholder" }, Summary = "Replace placeholders", Description = "This endpoint replaces the placeholders with corresponding values.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The API key for the function endpoint.")]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(PlaceholderReplaceRequest), Required = true, Example = typeof(PlaceholderReplaceRequestExample), Description = "This is the request payload for the placeholder replace request.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(PlaceholderReplaceResponse), Example = typeof(PlaceholderReplaceResponseExample), Summary = "Response payload that the replaced message is included", Description = "Response payload that the replaced message is included")]
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
