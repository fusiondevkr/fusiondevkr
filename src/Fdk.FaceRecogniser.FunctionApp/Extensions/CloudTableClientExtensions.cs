using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Fdk.FaceRecogniser.FunctionApp.Models;

using Microsoft.WindowsAzure.Storage.Table;

namespace Fdk.FaceRecogniser.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extension entity for the <see cref="CloudTableClient"/> class.
    /// </summary>
    public static class CloudTableClientExtensions
    {
        /// <summary>
        /// Gets the table reference.
        /// </summary>
        /// <param name="instance"><see cref="CloudTableClient"/> instance.</param>
        /// <param name="tableName">Table name.</param>
        /// <returns>Returns the <see cref="CloudTable"/> instance.</returns>
        public static async Task<CloudTable> WithTable(this CloudTableClient instance, string tableName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            var table = instance.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync().ConfigureAwait(false);

            return table;
        }

        /// <summary>
        /// Gets the list of <see cref="FaceEntity"/> instances.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudTable}"/> instance.</param>
        /// <returns>Returns the list of <see cref="FaceEntity"/> instances.</returns>
        public static async Task<List<FaceEntity>> GetEntitiesAsync(this Task<CloudTable> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);

            var query = new TableQuery<FaceEntity>();
            var entities = await instance.ExecuteQuerySegmentedAsync<FaceEntity>(query, new TableContinuationToken()).ConfigureAwait(false);
            var result = entities.GroupBy(p => p.PersonGroup)
                                 .SelectMany(g => g.Where(p => p.Timestamp == g.Max(q => q.Timestamp)))
                                 .OrderBy(p => p.PersonGroup)
                                 .ToList();

            return result;
        }

        /// <summary>
        /// Gets the list of <see cref="FaceEntity"/> instances.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudTable}"/> instance.</param>
        /// <param name="personGroupName">Name of the person group.</param>
        /// <returns>Returns the list of <see cref="FaceEntity"/> instances.</returns>
        public static async Task<List<FaceEntity>> GetEntitiesAsync(this Task<CloudTable> value, string personGroupName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var instance = await value.ConfigureAwait(false);

            var query = new TableQuery<FaceEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, personGroupName));
            var entities = await instance.ExecuteQuerySegmentedAsync<FaceEntity>(query, new TableContinuationToken()).ConfigureAwait(false);
            var result = entities.Results
                                 .OrderByDescending(p => p.Timestamp)
                                 .ToList();

            return result;
        }

        /// <summary>
        /// Gets the <see cref="FaceEntity"/> instance.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudTable}"/> instance.</param>
        /// <param name="personGroupName">Name of the person group.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> GetEntityAsync(this Task<CloudTable> value, string personGroupName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(personGroupName))
            {
                throw new ArgumentNullException(nameof(personGroupName));
            }

            var instance = await value.ConfigureAwait(false);
            var entity = new FaceEntity(personGroupName, Guid.NewGuid().ToString());
            var operation = TableOperation.InsertOrReplace(entity);

            var result = await instance.ExecuteAsync(operation).ConfigureAwait(false);

            return entity;
        }

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="value"><see cref="Task{CloudTable}"/> instance.</param>
        /// <param name="entity"><see cref="FaceEntity"/> to be updated.</param>
        /// <returns>Returns the <see cref="FaceEntity"/> instance.</returns>
        public static async Task<FaceEntity> UpdateEntityAsync(this Task<CloudTable> value, FaceEntity entity)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var instance = await value.ConfigureAwait(false);

            var operation = TableOperation.InsertOrReplace(entity);

            var result = await instance.ExecuteAsync(operation).ConfigureAwait(false);

            return entity;
        }
    }
}
