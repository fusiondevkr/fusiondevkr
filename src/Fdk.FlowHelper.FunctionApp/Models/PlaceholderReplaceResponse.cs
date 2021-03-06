namespace Fdk.FlowHelper.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for response that includes the result message from the placeholders replace.
    /// </summary>
    public class PlaceholderReplaceResponse
    {
        /// <summary>
        /// Gets or sets the result of the message that all the placeholders are replaced.
        /// </summary>
        public virtual string Result { get; set; }
    }
}
