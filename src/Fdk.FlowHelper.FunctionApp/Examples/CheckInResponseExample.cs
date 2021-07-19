using System.Net;

using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="CheckInResponse"/>.
    /// </summary>
    public class CheckInResponseExample : OpenApiExample<CheckInResponse>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<CheckInResponse> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "checkin",
                    new CheckInResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "hello FusionDevKR, it's FlowHelper.",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
