using System.Net;
using System.Threading.Tasks;

using Fdk.Common.Extensions;
using Fdk.Logistics.FunctionApp.Examples;
using Fdk.Logistics.FunctionApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Fdk.Logistics.FunctionApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to invoke check-in flow.
    /// </summary>
    public class QueueDeliveryHttpTrigger
    {
        /// <summary>
        /// Invokes the run method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the check-in message.</returns>
        [FunctionName(nameof(QueueDeliveryHttpTrigger.RunAsync))]
        [OpenApiOperation(operationId: "Queue", tags: new[] { "shipping" }, Summary = "Run shipping queue", Description = "This endpoint queues the shipping details.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiSecurity(schemeName: "function_key", schemeType: SecuritySchemeType.ApiKey, Name = "x-functions-key", In = OpenApiSecurityLocationType.Header, Description = "The API key for the function endpoint.")]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(DeliveryQueueRequest), Required = true, Example = typeof(DeliveryQueueRequestExample), Description = "This is the request payload for the shipping queue.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(DeliveryQueueResponse), Example = typeof(DeliveryQueueResponseExample), Summary = "Response payload including the shipping status result.", Description = "Response payload that includes the shipping status result.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, HttpVerbs.POST, Route = "shipping/queue")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<DeliveryQueueRequest>().ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var response = new DeliveryQueueResponse()
            {
                StatusCode = (int)HttpStatusCode.Created,
                Message = "배송 정보 큐를 생성했습니다.",
            };
            var res = new OkObjectResult(response);

            return await Task.FromResult(res).ConfigureAwait(false);
        }
    }
}
