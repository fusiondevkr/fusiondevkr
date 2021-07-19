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
        private static Random random = new Random();

        /// <summary>
        /// Gets the list of blobs in random order.
        /// </summary>
        /// <typeparam name="T">Type of blob.</typeparam>
        /// <param name="value"><see cref="Task{BlobResultSegment}"/> instance.</param>
        /// <param name="count">Number of blobs to retrieve.</param>
        /// <returns>Returns the list of blobs.</returns>
        public static async Task<List<T>> GetResults<T>(this Task<BlobResultSegment> value, int count)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var instance = await value.ConfigureAwait(false);

            var maxValue = instance.Results.Count();
            if (maxValue <= count)
            {
                return instance.Results.Select(p => (T)p).ToList();
            }

            var indices = new List<int>();
            while (indices.Count < count)
            {
                var index = random.Next(maxValue);
                if (indices.Contains(index))
                {
                    continue;
                }

                indices.Add(index);
            }

            var interim = instance.Results.Select(p => (T)p).ToList();

            var results = new List<T>();
            foreach(var index in indices)
            {
                results.Add(interim.Skip(index).Take(1).First());
            }

            return results;
        }
    }
}
