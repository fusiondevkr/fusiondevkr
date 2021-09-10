using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Fdk.M365Register.WasmApp.Models
{
    public class AuthenticationDetails
    {
        [JsonPropertyName("clientPrincipal")]
        public ClientPrincipal ClientPrincipal { get; set; }
    }

    public class ClientPrincipal
    {
        [JsonPropertyName("identityProvider")]
        public string IdentityProvider { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userDetails")]
        public string UserDetails { get; set; }

        [JsonPropertyName("userRoles")]
        public IEnumerable<string> UserRoles { get; set; }
    }
}
