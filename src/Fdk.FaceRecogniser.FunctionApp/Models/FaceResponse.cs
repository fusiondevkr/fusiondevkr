using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Newtonsoft.Json;

namespace Fdk.FaceRecogniser.FunctionApp.Models
{
    /// <summary>
    /// This represents the collection entity of the <see cref="FaceResponse"/> class.
    /// </summary>
    public class FaceCollectionResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceCollectionResponse"/> class.
        /// </summary>
        /// <param name="faces">List of <see cref="FaceEntity"/> instances.</param>
        public FaceCollectionResponse(List<FaceEntity> faces)
        {
            this.Faces = faces.Select(p => new FaceResponse(p)).ToList();
        }

        /// <summary>
        /// Gets or sets the list of the <see cref="FaceResponse"/> objects.
        /// </summary>
        [JsonProperty("faces")]
        public virtual List<FaceResponse> Faces { get; set; } = new List<FaceResponse>();
    }

    /// <summary>
    /// This represents the response entity for the face.
    /// </summary>
    public class FaceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaceResponse"/> class.
        /// </summary>
        /// <param name="face">The <see cref="FaceEntity"/> object.</param>
        public FaceResponse(FaceEntity face)
        {
            this.PersonGroup = face.PersonGroup;
            this.Confidence = Convert.ToDecimal(Math.Round(face.Confidence, 2));
            this.Timestamp = face.Timestamp;
        }

        /// <summary>
        /// Gets or sets the person group.
        /// </summary>
        [JsonProperty("personGroup")]
        public virtual string PersonGroup { get; set; }

        /// <summary>
        /// Gets or sets the confidence level.
        /// </summary>
        [JsonProperty("confidence")]
        public virtual decimal Confidence { get; set; }

        /// <summary>
        /// Gets or sets the timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public virtual DateTimeOffset Timestamp { get; set; }
    }
}
