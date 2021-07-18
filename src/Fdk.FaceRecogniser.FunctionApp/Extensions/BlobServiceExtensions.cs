using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Services;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="BlobService"/> class.
    /// </summary>
    public static class BlobServiceExtensions
    {
        /// <summary>
        /// Gets the list of face blobs.
        /// </summary>
        /// <param name="value"><see cref="Task{IBlobService}"/> instance.</param>
        /// <param name="personGroup">Name of the person group.</param>
        /// <param name="numberOfPhotos">Number of photos to retrieve.</param>
        /// <returns>Returns the list of <see cref="CloudBlockBlob"/> instances as faces.</returns>
        public static async Task<List<CloudBlockBlob>> GetFacesAsync(this Task<IBlobService> value, string personGroup, int numberOfPhotos = 6)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(personGroup))
            {
                throw new ArgumentNullException(nameof(personGroup));
            }

            var instance = await value.ConfigureAwait(false);

            var result = await instance.GetFacesAsync(personGroup, numberOfPhotos).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Uploads the face to the blob.
        /// </summary>
        /// <param name="value"><see cref="Task{IBlobService}"/> instance.</param>
        /// <param name="bytes">Byte array of the face image.</param>
        /// <param name="filename">File name of the face image.</param>
        /// <param name="contentType">Content type of the face image.</param>
        /// <returns>Returns the <see cref="CloudBlockBlob"/> instance.</returns>
        public static async Task<CloudBlockBlob> UploadAsync(this Task<IBlobService> value, byte[] bytes, string filename, string contentType)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

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

            var instance = await value.ConfigureAwait(false);

            var result = await instance.UploadAsync(bytes, filename, contentType).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Deletes the face from the blob.
        /// </summary>
        /// <param name="value"><see cref="Task{IBlobService}"/> instance.</param>
        /// <param name="filename">Filename of the face image.</param>
        /// <returns>Returns <c>True</c>, if deleted; otherwise returns <c>False</c>.</returns>
        public static async Task<bool> DeleteAsync(this Task<IBlobService> value, string filename)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            var instance = await value.ConfigureAwait(false);

            var result = await instance.DeleteAsync(filename).ConfigureAwait(false);

            return result;
        }
    }
}
