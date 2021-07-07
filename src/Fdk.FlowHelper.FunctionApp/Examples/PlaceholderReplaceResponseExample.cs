using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="PlaceholderReplaceResponse"/>.
    /// </summary>
    public class PlaceholderReplaceResponseExample : OpenApiExample<PlaceholderReplaceResponse>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<PlaceholderReplaceResponse> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "error",
                    new PlaceholderReplaceResponse()
                    {
                        Result = "hello FusionDevKR, it's FlowHelper.",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
