using System.Net;

using Fdk.M365Register.ApiApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.M365Register.ApiApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="RegistrationResponse"/>.
    /// </summary>
    public class RegistrationResponseExample : OpenApiExample<RegistrationResponse>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<RegistrationResponse> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "registration",
                    new RegistrationResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "hello FusionDevKR, it's Registration API.",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
