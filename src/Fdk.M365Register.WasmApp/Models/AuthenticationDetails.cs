using System.Text.Json.Serialization;

namespace Fdk.M365Register.WasmApp.Models
{
    /// <summary>
    /// This represents the entity for authentication details.
    /// </summary>
    public class AuthenticationDetails
    {
        /// <summary>
        /// Gets or sets the <see cref="ClientPrincipal"/> instance.
        /// </summary>
        [JsonPropertyName("clientPrincipal")]
        public ClientPrincipal ClientPrincipal { get; set; }
    }
}
