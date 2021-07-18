using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="BlobResultSegment"/> class.
    /// </summary>
    public static class BlobResultSegmentExtensions
    {
        /// <summary>
        /// GEts the list of blobs.
        /// </summary>
        /// <param name="value"><see cref="Task{BlobResultSegment}"/> instance.</param>
        /// <typeparam name="T">Type of blob.</typeparam>
        /// <returns>Returns the list of blobs.</returns>
        public static async Task<List<T>> GetResults<T>(this Task<BlobResultSegment> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);

            var results = instance.Results.Select(p => (T)p).ToList();

            return results;
        }
    }
}
