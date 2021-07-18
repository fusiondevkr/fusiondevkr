using Newtonsoft.Json;

namespace Fdk.FaceRecogniser.FunctionApp.Models
{
    /// <summary>
    /// This represents the response entity for the face identification result.
    /// </summary>
    public class FaceIdentificationResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultResponse"/> class.
        /// </summary>
        /// <param name="message">Return message.</param>
        public FaceIdentificationResponse(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [JsonProperty("message")]
        public virtual string Message { get; set; }
    }
}
