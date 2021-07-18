using Newtonsoft.Json;

namespace Fdk.FaceRecogniser.FunctionApp.Models
{
    /// <summary>
    /// This represents the request entity to take embedded image data.
    /// </summary>
    public class EmbeddedRequest
    {
        /// <summary>
        /// Gets or sets the person group.
        /// </summary>
        [JsonProperty("personGroup")]
        public virtual string PersonGroup { get; set; }

        /// <summary>
        /// Gets or sets the embedded image data.
        /// </summary>
        [JsonProperty("image")]
        public virtual string Image { get; set; }
    }
}
