using System;

using Newtonsoft.Json;

namespace Fdk.FlowHelper.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to run check-in workflow.
    /// </summary>
    public class CheckInRequest
    {
        /// <summary>
        /// Gets or sets the workflow trigger URL.
        /// </summary>
        [JsonProperty("workflowUrl")]
        public virtual string WorkflowUrl { get; set; }

        /// <summary>
        /// Gets or sets the check-in details.
        /// </summary>
        [JsonProperty("checkInDetails")]
        public virtual CheckInDetails CheckInDetails { get; set; }
    }

    public class CheckInDetails
    {
        /// <summary>
        /// Gets or sets the email address identifying who checked-in.
        /// </summary>
        [JsonProperty("email")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the user checked-in.
        /// </summary>
        [JsonProperty("timestamp")]
        public virtual DateTimeOffset Timestamp { get; set; }
    }
}
