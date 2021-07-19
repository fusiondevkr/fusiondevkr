using Fdk.FlowHelper.FunctionApp.Models;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

using Newtonsoft.Json;

namespace Fdk.FlowHelper.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extensions entity for any payload.
    /// </summary>
    public static class PayloadExtensions
    {
        /// <summary>
        /// Replaces the placeholder with the given value.
        /// </summary>
        /// <param name="payload"><see cref="PlaceholderReplaceRequest"/> instance.</param>
        /// <returns>Returns the replaced value.</returns>
        public static string ToJson<T>(this T payload)
        {
            if (payload.IsNullOrDefault())
            {
                return null;
            }

            var result = JsonConvert.SerializeObject(payload);

            return result;
        }
    }
}
