using System;

using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.FlowHelper.FunctionApp.Examples
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
                    "checkin",
                    new CheckInRequest()
                    {
                        WorkflowUrl = "https://prod-1.southeastasia.logic.azure.com:443/workflows/abcdefghijklmn/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=opqrstuvwxyz",
                        CheckInDetails = new CheckInDetails()
                        {
                            Email = "flowhelper@fusiondev.kr",
                            Timestamp = DateTimeOffset.UtcNow,
                        },
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
