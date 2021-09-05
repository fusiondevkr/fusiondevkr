using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="NameReorderResponse"/>.
    /// </summary>
    public class NameReorderResponseExample : OpenApiExample<NameReorderResponse>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<NameReorderResponse> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "english",
                    new NameReorderResponse()
                    {
                        Result = "Natasha Romanoff",
                    },
                    namingStrategy
                )
            );
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "korean",
                    new NameReorderResponse()
                    {
                        Result = "안세빈",
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
