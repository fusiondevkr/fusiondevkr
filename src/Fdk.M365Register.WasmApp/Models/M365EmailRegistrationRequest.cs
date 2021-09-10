using System.Text.Json.Serialization;

namespace Fdk.M365Register.WasmApp.Models
{
    /// <summary>
    /// This represents the request entity for M365 email registration.
    /// </summary>
    public class M365EmailRegistrationRequest
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [JsonPropertyName("user")]
        public virtual string UserEmail { get; set; }

        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        [JsonPropertyName("m365")]
        public virtual string M365Email { get; set; }
    }
}
