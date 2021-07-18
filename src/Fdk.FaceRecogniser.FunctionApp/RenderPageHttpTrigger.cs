using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Fdk.FaceRecogniser.FunctionApp
{
    /// <summary>
    /// This represents the HTTP trigger entity to render web page.
    /// </summary>
    public class RenderPageHttpTrigger
    {
        private readonly ILogger<RenderPageHttpTrigger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPageHttpTrigger"/> class.
        /// </summary>
        /// <param name="logger"><see cref="ILogger{RenderPageHttpTrigger}"/> instance.</param>
        public RenderPageHttpTrigger(ILogger<RenderPageHttpTrigger> logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes to render web page.
        /// </summary>
        /// <param name="req"><see cref="HttpRequest"/> instance.</param>
        /// <returns>Returns the <see cref="IActionResult"/> instance.</returns>
        [FunctionName(nameof(RenderPageHttpTrigger.CaptureFace))]
        [OpenApiIgnore()]
        public async Task<IActionResult> CaptureFace(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "pages/capture")] HttpRequest req,
            ExecutionContext context)
        {
            this._logger.LogInformation("C# HTTP trigger function processed a request.");

            var filepath = $"{context.FunctionAppDirectory}/photo-capture.html";
            var file = await File.ReadAllTextAsync(filepath, Encoding.UTF8).ConfigureAwait(false);
            var result = new ContentResult()
            {
                Content = file,
                StatusCode = 200,
                ContentType = "text/html"
            };

            return result;
        }
    }
}
