using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="NameReorderRequest"/>.
    /// </summary>
    public class NameReorderRequestExample : OpenApiExample<NameReorderRequest>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<NameReorderRequest> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "english",
                    new NameReorderRequest()
                    {
                        FirstName = "Natasha",
                        LastName = "Romanoff"
                    },
                    namingStrategy
                )
            );
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "korean",
                    new NameReorderRequest()
                    {
                        FirstName = "세빈",
                        LastName = "안"
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
