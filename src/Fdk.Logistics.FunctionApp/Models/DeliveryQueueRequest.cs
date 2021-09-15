using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Fdk.Logistics.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to run check-in workflow.
    /// </summary>
    public class DeliveryQueueRequest
    {
        /// <summary>
        /// Gets or sets the receiver's name.
        /// </summary>
        [JsonProperty("name")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the receiver's email.
        /// </summary>
        [JsonProperty("email")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the receiver's shipping address.
        /// </summary>
        [JsonProperty("address")]
        public virtual string Address { get; set; }

        /// <summary>
        /// Gets or sets the list of shipping items.
        /// </summary>
        [JsonProperty("items")]
        public virtual List<ShippingItem> Items { get; set; }
    }

    /// <summary>
    /// This represents the model entity for shipping item.
    /// </summary>
    public class ShippingItem
    {
        /// <summary>
        /// Gets or sets the shipping item ID.
        /// </summary>
        [JsonProperty("itemId")]
        public virtual int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the shipping item quantity.
        /// </summary>
        [JsonProperty("quantity")]
        public virtual int Quantity { get; set; }
    }
}
