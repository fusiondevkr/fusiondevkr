using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Configurations;
using Fdk.FaceRecogniser.FunctionApp.Extensions;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Services
{
    /// <summary>
    /// This provides interfaces to the <see cref="BlobService"/> class.
    /// </summary>
    public interface IBlobService
    {
        /// <summary>
        /// Gets the list of face blobs.
        /// </summary>
        /// <param name="personGroup">Name of the person group.</param>
        /// <param name="numberOfPhotos">Number of photos to retrieve.</param>
        /// <returns>Returns the list of <see cref="CloudBlockBlob"/> instances as faces.</returns>
        Task<List<CloudBlockBlob>> GetFacesAsync(string personGroup, int numberOfPhotos = 6);

        /// <summary>
        /// Uploads the face to the blob.
        /// </summary>
        /// <param name="bytes">Byte array of the face image.</param>
        /// <param name="filename">File name of the face image.</param>
        /// <param name="contentType">Content type of the face image.</param>
        /// <returns>Returns the <see cref="CloudBlockBlob"/> instance.</returns>
        Task<CloudBlockBlob> UploadAsync(byte[] bytes, string filename, string contentType);

        /// <summary>
        /// Deletes the face from the blob.
        /// </summary>
        /// <param name="filename">Filename of the face image.</param>
        /// <returns>Returns <c>True</c>, if deleted; otherwise returns <c>False</c>.</returns>
        Task<bool> DeleteAsync(string filename);
    }

    /// <summary>
    /// This represents the service entity for Azure Blob Storage.
    /// </summary>
    public class BlobService : IBlobService
    {
        private readonly AppSettings _settings;
        private readonly CloudBlobClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="AppSettings"/> instance.</param>
        /// <param name="client"><see cref="CloudBlobClient"/> instance.</param>
        public BlobService(AppSettings settings, CloudBlobClient client)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc/>
        public async Task<List<CloudBlockBlob>> GetFacesAsync(string personGroup, int numberOfPhotos = 6)
        {
            if (string.IsNullOrWhiteSpace(personGroup))
            {
                throw new ArgumentNullException(nameof(personGroup));
            }

            var items = await this._client
                                  .WithContainer(this._settings.Blob.Container)
                                  .GetBlobsAsync($"{personGroup}/", numberOfPhotos)
                                  .GetResults<CloudBlockBlob>()
                                  .ConfigureAwait(false);

            return items;
        }

        /// <inheritdoc/>
        public async Task<CloudBlockBlob> UploadAsync(byte[] bytes, string filename, string contentType)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (string.IsNullOrWhiteSpace(contentType))
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            var blob = await this._client
                                 .WithContainer(this._settings.Blob.Container)
                                 .GetBlobAsync(filename)
                                 .SetContentType(contentType)
                                 .UploadByteArrayAsync(bytes, 0, bytes.Length)
                                 .ConfigureAwait(false);

            return blob;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            var deleted = await this._client
                                    .WithContainer(this._settings.Blob.Container)
                                    .GetBlobAsync(filename)
                                    .DeleteAsync()
                                    .ConfigureAwait(false);

            return deleted;
        }
    }
}
