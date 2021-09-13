using System;

using Newtonsoft.Json;

namespace Fdk.CheckPointHelper.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to run check-in workflow.
    /// </summary>
    public class CheckInRequest
    {
        /// <summary>
        /// Gets or sets the check-in user's email.
        /// </summary>
        [JsonProperty("email")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the check-in details.
        /// </summary>
        [JsonProperty("checkpoint")]
        public virtual int CheckPoint { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user checked-in.
        /// </summary>
        [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public virtual DateTimeOffset? Timestamp { get; set; }
    }
}
