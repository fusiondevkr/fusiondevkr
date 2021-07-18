using System;
using System.Text.Json.Serialization;

using Fdk.FaceRecogniser.FunctionApp.Services;

using Microsoft.WindowsAzure.Storage.Table;

namespace Fdk.FaceRecogniser.FunctionApp.Models
{
    /// <summary>
    /// This represents the table entity for face identification.
    /// </summary>
    public class FaceEntity : TableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceEntity"/> class.
        /// </summary>
        public FaceEntity()
        {
            this.PersonId = string.Empty;
            this.FaceIds = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceEntity"/> class.
        /// </summary>
        /// <param name="personGroup">Name of the person group.</param>
        /// <param name="personGroupId">Person group ID.</param>
        public FaceEntity(string personGroup, string personGroupId)
            : this()
        {
            this.PartitionKey = personGroup ?? throw new ArgumentNullException(nameof(personGroup));
            this.RowKey = personGroupId ?? throw new ArgumentNullException(nameof(personGroupId));
        }

        /// <summary>
        /// Gets or sets the name of the person group.
        /// </summary>
        public virtual string PersonGroup
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        /// <summary>
        /// Gets or sets the person group ID.
        /// </summary>
        public virtual string PersonGroupId
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        /// <summary>
        /// Gets or sets the person ID.
        /// </summary>
        public virtual string PersonId { get; set; }

        /// <summary>
        /// Gets or sets the list of face IDs delimited by commas.
        /// </summary>
        public virtual string FaceIds { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether more than one or no face has been detected.
        /// </summary>
        public virtual bool TooManyOrNoFacesDetected { get; set; }

        /// <summary>
        /// Gets or sets the confidence value.
        /// </summary>
        public virtual double Confidence { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFaceService"/> instance.
        /// </summary>
        [JsonIgnore]
        public virtual IFaceService FaceService { get; set; }
    }
}
