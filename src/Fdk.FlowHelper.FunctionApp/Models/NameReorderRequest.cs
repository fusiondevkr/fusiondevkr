namespace Fdk.FlowHelper.FunctionApp.Models
{
    /// <summary>
    /// This represents the model entity for request to reorder name values.
    /// </summary>
    public class NameReorderRequest
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public virtual string LastName { get; set; }
    }
}
