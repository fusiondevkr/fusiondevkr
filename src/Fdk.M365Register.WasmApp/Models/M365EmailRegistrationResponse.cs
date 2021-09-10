using System.Text.Json.Serialization;

namespace Fdk.M365Register.WasmApp.Models
{
    /// <summary>
    /// This represents the response entity for M365 email registration.
    /// </summary>
    public class M365EmailRegistrationResponse
    {
        /// <summary>
        /// Gets or sets the registration result message.
        /// </summary>
        [JsonPropertyName("message")]
        public virtual string Message { get; set; }
    }
}
