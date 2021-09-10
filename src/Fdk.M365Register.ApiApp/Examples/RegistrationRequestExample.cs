using System;

using Fdk.M365Register.ApiApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.M365Register.ApiApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="RegistrationRequest"/>.
    /// </summary>
    public class RegistrationRequestExample : OpenApiExample<RegistrationRequest>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<RegistrationRequest> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "registration",
                    new RegistrationRequest()
                    {
                        UserEmail = "user@contoso.com",
                        M365Email = "m365@fabrikam.onmicrosoft.com",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
