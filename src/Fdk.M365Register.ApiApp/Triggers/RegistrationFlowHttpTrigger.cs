using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Fdk.M365Register.ApiApp.Configurations;
using Fdk.M365Register.ApiApp.Examples;
using Fdk.M365Register.ApiApp.Models;
using Fdk.Common.Extensions;

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

namespace Fdk.M365Register.ApiApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to invoke registration flow.
    /// </summary>
    public class RegistrationFlowHttpTrigger
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFlowHttpTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="httpClient"><see cref="HttpClient"/> instance.</param>
        public RegistrationFlowHttpTrigger(AppSettings settings, HttpClient httpClient)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = httpClient.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the run method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the check-in message.</returns>
        [FunctionName(nameof(RegistrationFlowHttpTrigger.RegisterUserAsync))]
        [OpenApiOperation(operationId: "registerUser", tags: new[] { "registration" }, Summary = "Run registration", Description = "This endpoint runs the registration workflow.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(RegistrationRequest), Required = true, Example = typeof(RegistrationRequestExample), Description = "This is the request payload for the registration.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(RegistrationResponse), Example = typeof(RegistrationResponseExample), Summary = "Response payload including the registration result.", Description = "Response payload that includes the registration result.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> RegisterUserAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.POST, Route = "m365/register")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<RegistrationRequest>().ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var response = default(RegistrationResponse);
            var res = default(IActionResult);
            try
            {
                using (var content = new StringContent(request.ToJson(), Encoding.UTF8, ContentTypes.ApplicationJson))
                using (var message = await this._http.PostAsync(this._settings.Workflows.RegistrationUrl, content).ConfigureAwait(false))
                {
                    message.EnsureSuccessStatusCode();

                    var serialised = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
                    response = JsonConvert.DeserializeObject<RegistrationResponse>(serialised);
                }

                if (this._settings.Workflows.IncludeCheckIn)
                {
                    var checkin = new CheckInRequest() { Upn = request.M365Email, CheckPoint = 1 };
                    using (var content = new StringContent(checkin.ToJson(), Encoding.UTF8, ContentTypes.ApplicationJson))
                    using (var message = await this._http.PostAsync(this._settings.Workflows.CheckInUrl, content).ConfigureAwait(false))
                    {
                        message.EnsureSuccessStatusCode();
                    }
                }

                res = new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                response = new RegistrationResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                };

                res = new JsonResult(response) { StatusCode = (int)HttpStatusCode.InternalServerError };
            }

            return res;
        }
    }
}
