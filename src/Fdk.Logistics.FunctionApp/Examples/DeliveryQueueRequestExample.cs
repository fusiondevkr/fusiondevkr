using System.Collections.Generic;

using Fdk.Logistics.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace Fdk.Logistics.FunctionApp.Examples
{
    /// <summary>
    /// This represents the example entity for <see cref="DeliveryQueueRequest"/>.
    /// </summary>
    public class DeliveryQueueRequestExample : OpenApiExample<DeliveryQueueRequest>
    {
        /// <inheritdoc/>
        public override IOpenApiExample<DeliveryQueueRequest> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(
                OpenApiExampleResolver.Resolve(
                    "queue",
                    new DeliveryQueueRequest()
                    {
                        Name = "김온유",
                        Email = "onyu@fourthcoffee.com",
                        Address = "서울시 종로구 종로 1길 50",
                        Items = new List<ShippingItem>() { new ShippingItem() { Name = "카드", Quantity = 100 }, new ShippingItem() { Name = "풍선", Quantity = 100 } },
                    },
                    namingStrategy
                )
            );

            return this;
        }
    }
}
