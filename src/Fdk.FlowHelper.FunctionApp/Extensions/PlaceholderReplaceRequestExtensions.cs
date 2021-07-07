using Fdk.FlowHelper.FunctionApp.Models;

namespace Fdk.FlowHelper.FunctionApp.Extensions
{
    /// <summary>
    /// This represents the extensions entity for <see cref="PlaceholderReplaceRequest"/>.
    /// </summary>
    public static class PlaceholderReplaceRequestExtensions
    {
        /// <summary>
        /// Replaces the placeholder with the given value.
        /// </summary>
        /// <param name="request"><see cref="PlaceholderReplaceRequest"/> instance.</param>
        /// <returns>Returns the replaced value.</returns>
        public static string Replace(this PlaceholderReplaceRequest request)
        {
            var result = request.Message;

            foreach (var kvp in request.Placeholders)
            {
                var placeholder = $"{{{{{kvp.Key}}}}}";
                var replacement = kvp.Value;

                result = result.Replace(placeholder, replacement);
            }

            return result;
        }
    }
}
