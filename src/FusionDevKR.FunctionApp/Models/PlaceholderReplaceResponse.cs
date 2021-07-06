namespace FusionDevKR.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to replace placeholder values.
    /// </summary>
    public class PlaceholderReplaceResponse
    {
        /// <summary>
        /// Gets or sets the result of the message that all the placeholders are replaced.
        /// </summary>
        public virtual string Result { get; set; }
    }
}
