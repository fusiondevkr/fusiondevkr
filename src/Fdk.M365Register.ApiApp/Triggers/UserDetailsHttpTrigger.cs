using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Fdk.Common.Extensions;
using Fdk.M365Register.ApiApp.Configurations;
using Fdk.M365Register.ApiApp.Examples;
using Fdk.M365Register.ApiApp.Extensions;
using Fdk.M365Register.ApiApp.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json;


namespace Fdk.M365Register.ApiApp.Triggers
{
    /// <summary>
    /// This represents the HTTP trigger entity to retreive user details.
    /// </summary>
    public class UserDetailsHttpTrigger
    {
        private readonly AppSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailsHttpTrigger"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        public UserDetailsHttpTrigger(AppSettings settings)
        {
            this._settings = settings.ThrowIfNullOrDefault();
        }

        /// <summary>
        /// Invokes the run method.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <param name="log"><see cref="ILogger"/> instance.</param>
        /// <returns>Returns the check-in message.</returns>
        [FunctionName(nameof(UserDetailsHttpTrigger.GetUserDetailsAsync))]
        [OpenApiOperation(operationId: "getUser", tags: new[] { "registration" }, Summary = "Get user details", Description = "This endpoint gets the user details.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiRequestBody(contentType: ContentTypes.ApplicationJson, bodyType: typeof(RegistrationRequest), Required = true, Example = typeof(RegistrationRequestExample), Description = "This is the request payload for the registration.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: ContentTypes.ApplicationJson, bodyType: typeof(RegistrationResponse), Example = typeof(RegistrationResponseExample), Summary = "Response payload including the registration result.", Description = "Response payload that includes the registration result.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.BadRequest, Summary = "Invalid request payload", Description = "This indicates the request payload is invalid.")]
        public async Task<IActionResult> GetUserDetailsAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpVerbs.GET, Route = "users/get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var request = await req.To<IHeaderDictionary>(SourceFrom.Header).ConfigureAwait(false);
            if (request.IsNullOrDefault())
            {
                return new BadRequestResult();
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String((string) request["x-ms-client-principal"]));
            var principal = JsonConvert.DeserializeObject<ClientPrincipal>(json);

            var client = await this.GetGraphClientAsync().ConfigureAwait(false);

            var users = await client.Users.Request().GetAsync().ConfigureAwait(false);
            var user = users.SingleOrDefault(p => p.Mail == principal.UserDetails);
            if (user == null)
            {
                return new NotFoundResult();
            }

            var loggedInUser = new LoggedInUser(user);

            return new OkObjectResult(loggedInUser);
        }

        private async Task<GraphServiceClient> GetGraphClientAsync()
        {
            var baseUri = $"{this._settings.MsGraph.ApiUri.TrimEnd('/')}/{this._settings.MsGraph.BaseUri}";
            var provider = new DelegateAuthenticationProvider(async p =>
                           {
                               var accessToken = await this.GetAccessTokenAsync().ConfigureAwait(false);
                               p.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                           });
            var client = new GraphServiceClient(baseUri, provider);

            return await Task.FromResult(client).ConfigureAwait(false);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var scopes = new [] { $"{this._settings.MsGraph.ApiUri.TrimEnd('/')}/.default" };
            var options = this._settings.MsGraph.ToConfidentialClientApplicationOptions();
            var authority = $"{options.Instance.TrimEnd('/')}/{options.TenantId}";

            var app = ConfidentialClientApplicationBuilder
                          .Create(options.ClientId)
                          .WithClientSecret(options.ClientSecret)
                          .WithAuthority(authority)
                          .Build();

            var result = await app.AcquireTokenForClient(scopes)
                                  .ExecuteAsync()
                                  .ConfigureAwait(false);
            var accessToken = result.AccessToken;

            return accessToken;
        }
    }
}
