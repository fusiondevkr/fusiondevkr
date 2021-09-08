using System.Net;

using Fdk.Logistics.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.Logistics.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="DeliveryQueueResponse"/>.
    /// </summary>
    public class DeliveryQueueResponseExample : OpenApiExample<DeliveryQueueResponse>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<DeliveryQueueResponse> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "result",
                    new DeliveryQueueResponse()
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = "hello Fourth Coffee.",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
