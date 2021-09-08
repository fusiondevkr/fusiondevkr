using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Fdk.Common.Extensions
{
    /// <summary>
    /// This represents the extension entity for <see cref="HttpRequest"/>.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Converts the <see cref="HttpRequest"/> to the instance of the given type.
        /// </summary>
        /// <typeparam name="T">Type to convert and return.</typeparam>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the converted instance.</returns>
        public static async Task<T> To<T>(this HttpRequest req)
        {
            var result = default(T);
            using (var reader = new StreamReader(req.Body))
            {
                var serialised = await reader.ReadToEndAsync().ConfigureAwait(false);
                result = JsonConvert.DeserializeObject<T>(serialised);
            }

            return result;
        }
    }
}
