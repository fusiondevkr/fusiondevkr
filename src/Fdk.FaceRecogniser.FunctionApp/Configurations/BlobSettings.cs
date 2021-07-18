namespace Fdk.FaceRecogniser.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the settings entity for Azure Blob Storage.
    /// </summary>
    public class BlobSettings
    {
        // private const string SasTokenKey = "Blob__SasToken";
        // private const string ContainerNameKey = "Blob__Container";
        // private const string NumberOfPhotosKey = "Blob__NumberOfPhotos";

        /// <summary>
        /// Gets or sets the SAS token of the blob container.
        /// </summary>
        /// <returns></returns>
        public virtual string SasToken { get; set; }

        /// <summary>
        /// Gets or sets the blob container name.
        /// </summary>
        public virtual string Container { get; set; }

        /// <summary>
        /// Gets or sets the number of photos to check.
        /// </summary>
        public virtual int NumberOfPhotos { get; set; }
    }
}
