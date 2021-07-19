using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Configurations;
using Fdk.FaceRecogniser.FunctionApp.Extensions;
using Fdk.FaceRecogniser.FunctionApp.Models;

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace Fdk.FaceRecogniser.FunctionApp.Services
{
    /// <summary>
    /// This provides interfaces to the <see cref="FaceService"/> class.
    /// </summary>
    public interface IFaceService
    {
        /// <summary>
        /// Detects faces on the image.
        /// </summary>
        /// <param name="face"><see cref="CloudBlockBlob"/> instance.</param>
        /// <returns>Returns the list of <see cref="DetectedFace"/> instances.</returns>
        Task<List<DetectedFace>> DetectFacesAsync(CloudBlockBlob face);

        /// <summary>
        /// Adds person group.
        /// </summary>
        /// <param name="personGroup">Name of the person group.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        Task<FaceEntity> WithPersonGroup(string personGroup);

        /// <summary>
        /// Adds person.
        /// </summary>
        /// <param name="entity"><see cref="FaceEntity"/> instance.</param>
        /// <param name="blobs">List of <see cref"CloudBlockBlob"/> instances.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        Task<FaceEntity> WithPerson(FaceEntity entity, List<CloudBlockBlob> blobs);

        /// <summary>
        /// Trains the faces added.
        /// </summary>
        /// <param name="entity"><see cref="FaceEntity"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        Task<FaceEntity> TrainFacesAsync(FaceEntity entity);

        /// <summary>
        /// Identifies the face provided.
        /// </summary>
        /// <param name="entity"><see cref="FaceEntity"/> instance.</param>
        /// <param name="face"><see cref="CloudBlockBlob"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        Task<FaceEntity> IdentifyFaceAsync(FaceEntity entity, CloudBlockBlob face);

        /// <summary>
        /// Updates the face identification result.
        /// </summary>
        /// <param name="entity"><see cref="FaceEntity"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        Task<FaceEntity> UpdateFaceIdentificationAsync(FaceEntity entity);

        /// <summary>
        /// Gets the list of the latest face from all person groups.
        /// </summary>
        /// <returns>Returns the list of the latest face from all person groups.</returns>
        Task<List<FaceEntity>> GetAllFacesAsync();

        /// <summary>
        /// Gets the list of all faces from the given person group.
        /// </summary>
        /// <param name="personGroup">Name of the person group.</param>
        /// <returns>Returns the list of the latest face from all person groups.</returns>
        Task<List<FaceEntity>> GetAllFacesAsync(string personGroup);
    }

    /// <summary>
    /// This represents the service entity for Azure Face Service.
    /// </summary>
    public class FaceService : IFaceService
    {
        private readonly AppSettings _settings;
        private readonly CloudTableClient _table;
        private readonly IFaceClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="FaceService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="table"><see cref="CloudTableClient"/> instance.</param>
        /// <param name="client"><see cref="IFaceClient"/> instance.</param>
        public FaceService(AppSettings settings, CloudTableClient table, IFaceClient client)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._table = table ?? throw new ArgumentNullException(nameof(table));
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<List<DetectedFace>> DetectFacesAsync(CloudBlockBlob face)
        {
            var url = $"{face.Uri.AbsoluteUri}{this._settings.Blob.SasToken}";
            var faces = await this._client
                                  .Face
                                  .DetectWithUrlAsync(url, recognitionModel: RecognitionModel.Recognition01)
                                  .ConfigureAwait(false);

            return faces.ToList();
        }

        /// <inheritdoc/>
        public async Task<FaceEntity> WithPersonGroup(string personGroup)
        {
            var entity = await this._table
                                   .WithTable(this._settings.Table.Name)
                                   .GetEntityAsync(personGroup)
                                   .ConfigureAwait(false);

            await this._client
                      .PersonGroup
                      .CreateAsync(entity.PersonGroupId, entity.PersonGroup, recognitionModel: RecognitionModel.Recognition01)
                      .ConfigureAwait(false);

            entity.FaceService = this;

            return entity;
        }

        /// <inheritdoc/>
        public async Task<FaceEntity> WithPerson(FaceEntity entity, List<CloudBlockBlob> blobs)
        {
            var person = await this._client
                                   .PersonGroupPerson
                                   .CreateAsync(entity.PersonGroupId, entity.PersonGroup)
                                   .ConfigureAwait(false);

            entity.PersonId = person.PersonId.ToString();

            var faceIds = new List<string>();
            foreach (var blob in blobs)
            {
                try
                {
                    var url = $"{blob.Uri.AbsoluteUri}{this._settings.Blob.SasToken}";
                    var face = await this._client
                                         .PersonGroupPerson
                                         .AddFaceFromUrlAsync(entity.PersonGroupId, person.PersonId, url, blob.Name)
                                         .ConfigureAwait(false);

                    var filename = blob.Name
                                       .Replace($"{entity.PersonGroup}/", string.Empty)
                                       .Replace(".png", string.Empty);
                    faceIds.Add(filename);
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }

            entity.FaceIds = string.Join(",", faceIds);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<FaceEntity> TrainFacesAsync(FaceEntity entity)
        {
            await this._client
                      .PersonGroup
                      .TrainAsync(entity.PersonGroupId)
                      .ConfigureAwait(false);

            while (true)
            {
                await Task.Delay(1000);
                var trainingStatus = await this._client
                                               .PersonGroup
                                               .GetTrainingStatusAsync(entity.PersonGroupId)
                                               .ConfigureAwait(false);

                if (trainingStatus.Status == TrainingStatusType.Succeeded)
                {
                    break;
                }
            }

            return entity;
        }

        /// <inheritdoc/>
        public async Task<FaceEntity> IdentifyFaceAsync(FaceEntity entity, CloudBlockBlob face)
        {
            var faces = await this.DetectFacesAsync(face)
                                  .ConfigureAwait(false);
            if (faces.Count != 1)
            {
                entity.TooManyOrNoFacesDetected = true;

                return entity;
            }

            var detected = faces.Where(p => p.FaceId.HasValue)
                                .Select(p => p.FaceId.Value)
                                .ToList();
            var identified = await this._client
                                       .Face
                                       .IdentifyAsync(detected, entity.PersonGroupId)
                                       .ConfigureAwait(false);

            var confidence = identified.First().Candidates.First().Confidence;
            entity.Confidence = confidence;
            entity.Timestamp = DateTimeOffset.UtcNow;

            return entity;
        }

        /// <inheritdoc/>
        public async Task<FaceEntity> UpdateFaceIdentificationAsync(FaceEntity entity)
        {
            await this._table
                      .WithTable(this._settings.Table.Name)
                      .UpdateEntityAsync(entity)
                      .ConfigureAwait(false);

            return entity;
        }

        /// <inheritdoc/>
        public async Task<List<FaceEntity>> GetAllFacesAsync()
        {
            var entities = await this._table
                                     .WithTable(this._settings.Table.Name)
                                     .GetEntitiesAsync()
                                     .ConfigureAwait(false);

            return entities;
        }

        /// <inheritdoc/>
        public async Task<List<FaceEntity>> GetAllFacesAsync(string personGroup)
        {
            var entities = await this._table
                                     .WithTable(this._settings.Table.Name)
                                     .GetEntitiesAsync(personGroup)
                                     .ConfigureAwait(false);

            return entities;
        }
    }
}
