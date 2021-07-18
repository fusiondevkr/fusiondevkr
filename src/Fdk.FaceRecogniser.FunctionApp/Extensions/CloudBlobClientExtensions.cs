using System;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="CloudBlobClient"/> class.
    /// </summary>
    public static class CloudBlobClientExtensions
    {
        /// <summary>
        /// Gets the container reference.
        /// </summary>
        /// <param name="instance"><see cref="CloudBlobClient"/> instance.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <returns>Returns the <see cref="CloudBlobContainer"/> instance.</returns>
        public static async Task<CloudBlobContainer> WithContainer(this CloudBlobClient instance, string containerName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException(nameof(containerName));
            }

            var container = instance.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            return container;
        }

        /// <summary>
        /// Gets the list of blobs.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudBlobContainer}"/> instance.</param>
        /// <param name="prefix">Prefix value. It's usually the directory name.</param>
        /// <param name="count">Number of blobs to retrieve.</param>
        /// <returns>Returns the <see cref="BlobResultSegment"/> instance.</returns>
        public static async Task<BlobResultSegment> GetBlobsAsync(this Task<CloudBlobContainer> value, string prefix, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.ListBlobsSegmentedAsync(prefix, true, BlobListingDetails.Metadata, count, null, null, null)
                                       .ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Gets the blob.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudBlobContainer}"/> instance.</param>
        /// <param name="filename">Blob filename.</param>
        /// <returns>Returns the <see cref="CloudBlockBlob"/> instance.</returns>
        public static async Task<CloudBlockBlob> GetBlobAsync(this Task<CloudBlobContainer> value, string filename)
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
            var result = instance.GetBlockBlobReference(filename);

            return result;
        }
    }
}
