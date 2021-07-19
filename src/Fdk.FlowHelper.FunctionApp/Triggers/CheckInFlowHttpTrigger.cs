using System;
using System.Net;
using System.Net.Http;
using System.Text;
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

using Newtonsoft.Json;

namespace Fdk.FlowHelper.FunctionApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to replace placeholders with their respective replacements.
    /// </summary>
    public class CheckInFlowHttpTrigger
    {
        private static readonly HttpClient http = new HttpClient();

        /// <summary>
        /// Invokes the run method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the check-in message.</returns>
        [FunctionName(nameof(CheckInFlowHttpTrigger.RunAsync))]
        [OpenApiOperation(operationId: "checkin.run", tags: new[] { "checkin" }, Summary = "Run check-in", Description = "This endpoint runs the check-in workflow.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The API key for the function endpoint.")]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(CheckInRequest), Required = true, Example = typeof(CheckInRequestExample), Description = "This is the request payload for the check-in.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(CheckInResponse), Example = typeof(CheckInResponseExample), Summary = "Response payload including the check-in result.", Description = "Response payload that includes the check-in result.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = "checkins/run")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<CheckInRequest>().ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var response = default(CheckInResponse);
            var res = default(OkObjectResult);
            try
            {
                using (var content = new StringContent(request.CheckInDetails.ToJson(), Encoding.UTF8, ContentTypes.ApplicationJson))
                using (var message = await http.PostAsync(request.WorkflowUrl, content).ConfigureAwait(false))
                {
                    message.EnsureSuccessStatusCode();

                    var serialised = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonConvert.DeserializeObject<CheckInResponse>(serialised);
                }

                res = new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                response = new CheckInResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                };

                res = new OkObjectResult(response);
            }

            return res;
        }
    }
}
