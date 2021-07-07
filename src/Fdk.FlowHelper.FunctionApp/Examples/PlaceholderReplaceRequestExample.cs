using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="PlaceholderReplaceRequest"/>.
    /// </summary>
    public class PlaceholderReplaceRequestExample : OpenApiExample<PlaceholderReplaceRequest>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<PlaceholderReplaceRequest> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "hello",
                    new PlaceholderReplaceRequest()
                    {
                        Message = "hello {{yourName}}, it's {{myName}}.",
                        Placeholders = { { "yourName", "FusionDevKR" }, { "myName", "FlowHelper" } },
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
