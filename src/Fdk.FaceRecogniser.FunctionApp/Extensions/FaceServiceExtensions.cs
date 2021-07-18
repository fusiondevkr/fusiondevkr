using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Models;

using Microsoft.WindowsAzure.Storage.Blob;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extensions entity for the <see cref="FaceService"/> class.
    /// </summary>
    public static class FaceServiceExtensions
    {
        /// <summary>
        /// Adds person.
        /// </summary>
        /// <param name="value"><see cref="Task{FaceEntity}"/> instance.</param>
        /// <param name="blobs">List of <see cref"CloudBlockBlob"/> instances.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> WithPerson(this Task<FaceEntity> value, List<CloudBlockBlob> blobs)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (blobs == null)
            {
                throw new ArgumentNullException(nameof(blobs));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.FaceService.WithPerson(instance, blobs).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Trains the faces added.
        /// </summary>
        /// <param name="value"><see cref="Task{FaceEntity}"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> TrainFacesAsync(this Task<FaceEntity> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.FaceService.TrainFacesAsync(instance).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Identifies the face provided.
        /// </summary>
        /// <param name="value"><see cref="Task{FaceEntity}"/> instance.</param>
        /// <param name="face"><see cref="CloudBlockBlob"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> IdentifyFaceAsync(this Task<FaceEntity> value, CloudBlockBlob face)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (face == null)
            {
                throw new ArgumentNullException(nameof(face));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.FaceService.IdentifyFaceAsync(instance, face).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Updates the face identification result.
        /// </summary>
        /// <param name="value"><see cref="Task{FaceEntity}"/> instance.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> UpdateFaceIdentificationAsync(this Task<FaceEntity> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);
            var result = await instance.FaceService.UpdateFaceIdentificationAsync(instance).ConfigureAwait(false);

            return result;
        }
    }
}
