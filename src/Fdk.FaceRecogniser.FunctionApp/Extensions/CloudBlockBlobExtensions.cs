using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="CloudBlockBlob"/> class.
    /// </summary>
    public static class CloudBlockBlobExtensions
    {
        /// <summary>
        /// Sets the content type of the blob.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudBlockBlob}"/> instance.</param>
        /// <param name="contentType">Content type.</param>
        /// <returns>Returns the <see cref="CloudBlockBlob"/> instance.</returns>
        public static async Task<CloudBlockBlob> SetContentType(this Task<CloudBlockBlob> value, string contentType)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);
            instance.Properties.ContentType = contentType;

            return instance;
        }

        /// <summary>
        /// Uploads the byte array to blob.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudBlockBlob}"/> instance.</param>
        /// <param name="bytes">Byte array of the blob.</param>
        /// <param name="index">Start index of the array.</param>
        /// <param name="count">Total length of the array.</param>
        /// <returns>Returns the <see cref="CloudBlockBlob"/> instance.</returns>
        public static async Task<CloudBlockBlob> UploadByteArrayAsync(this Task<CloudBlockBlob> value, byte[] bytes, int index, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);
            await instance.UploadFromByteArrayAsync(bytes, index, count).ConfigureAwait(false);

            return instance;
        }

        /// <summary>
        /// Deletes the blob from the storage.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudBlockBlob}"/> instance.</param>
        /// <returns>Returns <c>True</c>, if deleted; otherwise returns <c>False</c>.</returns>
        public static async Task<bool> DeleteAsync(this Task<CloudBlockBlob> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.DeleteIfExistsAsync().ConfigureAwait(false);

            return result;
        }
    }
}
