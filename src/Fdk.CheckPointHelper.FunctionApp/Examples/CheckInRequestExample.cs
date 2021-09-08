using System;

using Fdk.CheckPointHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.CheckPointHelper.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="CheckInRequest"/>.
    /// </summary>
    public class CheckInRequestExample : OpenApiExample<CheckInRequest>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<CheckInRequest> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "checkin_without_timestamp",
                    new CheckInRequest()
                    {
                        Upn = "checkpointhelper@fusiondev.kr",
                        CheckPoint = 1,
                    },
                    namingStrategy
                )
            );
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "checkin_with_timestamp",
                    new CheckInRequest()
                    {
                        Upn = "checkpointhelper@fusiondev.kr",
                        CheckPoint = 1,
                        Timestamp = DateTimeOffset.UtcNow,
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
