using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DepsWebApp.Middlewares
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context.Request);

            await _next.Invoke(context);

            await LogResponse(context);
        }

        private async Task LogRequest(HttpRequest request)
        {
            var requestBody = await ObtainRequestBody(request);

            _logger.LogInformation("Request raw data");
            _logger.LogInformation($"Content-Type: {request.ContentType}");
            _logger.LogInformation($"Body: {requestBody}");
            _logger.LogInformation($"Path: {request.Path}");
            _logger.LogInformation($"Query: {request.QueryString}");
            _logger.LogInformation("Headers:");
            request.Headers.Values.ToList().ForEach(header =>
                _logger.LogInformation(string.Join('\n', header)));
            _logger.LogInformation(new string('_', 25));
        }

        private async Task LogResponse(HttpContext context)
        {
            var responseBody = await ObtainResponseBody(context);
            var response = context.Response;

            _logger.LogInformation("Response raw data");
            _logger.LogInformation($"Content-Type: {response.ContentType}");
            _logger.LogInformation($"Body: {responseBody}");
            _logger.LogInformation($"Status code: {response.StatusCode}");
            _logger.LogInformation("Headers:");
            response.Headers.Values.ToList().ForEach(header =>
                _logger.LogInformation(string.Join('\n', header)));
            _logger.LogInformation(new string('_', 25));
        }

        private static async Task<string> ObtainRequestBody(HttpRequest request)
        {
            if (request.Body == null) return string.Empty;
            request.EnableBuffering();

            var encoding = GetEncodingFromContentType(request.ContentType);
            string bodyStr;
            using var reader = new StreamReader(request.Body, encoding, true, 1024, true);

            bodyStr = await reader.ReadToEndAsync().ConfigureAwait(false);
            request.Body.Seek(0, SeekOrigin.Begin);
            return bodyStr;
        }

        private static async Task<string> ObtainResponseBody(HttpContext context)
        {
            var response = context.Response;
            response.Body.Seek(0, SeekOrigin.Begin);

            var encoding = GetEncodingFromContentType(response.ContentType);
            using var reader = new StreamReader(response.Body, encoding, false, 4096, true);
            var text = await reader.ReadToEndAsync().ConfigureAwait(false);

            response.Body.Seek(0, SeekOrigin.Begin);
            return text;
        }

        private static Encoding GetEncodingFromContentType(string contentTypeStr)
        {
            if (string.IsNullOrEmpty(contentTypeStr))
            {
                return Encoding.UTF8;
            }
            ContentType contentType;
            try
            {
                contentType = new ContentType(contentTypeStr);
            }
            catch (FormatException)
            {
                return Encoding.UTF8;
            }
            if (string.IsNullOrEmpty(contentType.CharSet))
            {
                return Encoding.UTF8;
            }
            return Encoding.GetEncoding(contentType.CharSet, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
    }
}
