using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Fdk.M365Register.ApiApp.Models
{
    /// <summary>
    /// This specifies the email type.
    /// </summary>
    /// <summary>
    /// This specifies the email type.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EmailType
    {
        /// <summary>
        /// Identifies the user's email.
        /// </summary>
        [EnumMember(Value = "user")]
        User,

        /// <summary>
        /// Identifies the M365 dev account email.
        /// </summary>
        [EnumMember(Value = "m365")]
        M365
    }
}
