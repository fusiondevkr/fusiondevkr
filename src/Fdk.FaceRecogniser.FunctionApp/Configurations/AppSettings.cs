using Microsoft.Azure.WebJobs.Extensions.OpenApi.Configurations.AppSettings.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;

namespace Fdk.FaceRecogniser.FunctionApp.Configurations
{
    /// <summary>
    /// This represents the app settings entity.
    /// </summary>
    public class AppSettings : OpenApiAppSettingsBase
    {
        public AppSettings()
            : base()
        {
            this.Blob = this.Config.Get<BlobSettings>("Blob");
            this.Table = this.Config.Get<TableSettings>("Table");
            this.Face = this.Config.Get<FaceSettings>("Face");
        }

        /// <summary>
        /// Gets or sets the <see cref="BlobSettings"/> instance.
        /// </summary>
        public virtual BlobSettings Blob { get; }

        /// <summary>
        /// Gets or sets the <see cref="TableSettings"/> instance.
        /// </summary>
        public virtual TableSettings Table { get; }

        /// <summary>
        /// Gets or sets the <see cref="FaceSettings"/> instance.
        /// </summary>
        public virtual FaceSettings Face { get; }
    }
}
