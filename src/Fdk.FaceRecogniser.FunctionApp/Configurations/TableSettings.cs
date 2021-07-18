namespace Fdk.FaceRecogniser.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the settings entity for Azure Table Storage.
    /// </summary>
    public class TableSettings
    {
        // private const string TableNameKey = "Table__Name";

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
