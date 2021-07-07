using System.Collections.Generic;

namespace Fdk.FlowHelper.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to replace placeholder values.
    /// </summary>
    public class PlaceholderReplaceRequest
    {
        /// <summary>
        /// Gets or sets the message that contains the placeholders.
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Gets or sets the collection of placeholders and replacement values.
        /// </summary>
        public virtual Dictionary<string, string> Placeholders { get; set; } = new Dictionary<string, string>();
    }
}
