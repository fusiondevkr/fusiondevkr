using Newtonsoft.Json;

namespace Fdk.M365Register.ApiApp.Models
{
    /// <summary>
    /// This represents the model entity for request to run registration workflow.
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [JsonProperty("user")]
        public virtual string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the M365 dev account email.
        /// </summary>
        [JsonProperty("m365")]
        public virtual string M365Email { get; set; }
    }
}
