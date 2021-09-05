using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    /// This represents the HTTP trigger entity to reorder first name and surname depending on the language identified.
    /// </summary>
    public class ReorderNameHttpTrigger
    {
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReorderNameHttpTrigger"/> class.
        /// </summary>
        /// <param name="regex"><see cref="Regex"/> instance.</param>
        public ReorderNameHttpTrigger(Regex regex)
        {
            this._regex = regex.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the replace method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the replaced message.</returns>
        [FunctionName(nameof(ReorderNameHttpTrigger.ReorderAsync))]
        [OpenApiOperation(operationId: "names.reorder", tags: new[] { "name" }, Summary = "Reorder first name and surname", Description = "This endpoint reorders the first name and surname depending on the language identified.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The API key for the function endpoint.")]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(NameReorderRequest), Required = true, Example = typeof(NameReorderRequestExample), Description = "This is the request payload for the name reorder request.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(NameReorderResponse), Example = typeof(NameReorderResponseExample), Summary = "Response payload that the result is included", Description = "Response payload that the result is included")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> ReorderAsync(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = "names/reorder")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<NameReorderRequest>().ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var result = request.Reorder(this._regex);

            var response = new NameReorderResponse() { Result = result };

            var res = new OkObjectResult(response);

            return res;
        }
    }
}
