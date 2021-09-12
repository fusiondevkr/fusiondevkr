namespace Fdk.M365Register.ApiApp.Models
{
    /// <summary>
    /// This represents the model entity for response from the check-in workflow.
    /// </summary>
    public class CheckInResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code indicating whether the request was successful or not.
        /// </summary>
        public virtual int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public virtual string Message { get; set; }
    }
}
