using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNogin.CorrelationIdMiddleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly CorrelationIdMiddlewareSettings _options;

        public CorrelationIdMiddleware(RequestDelegate next, CorrelationIdMiddlewareSettings options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InvokeAsync(HttpContext context, ILogger<CorrelationIdMiddleware> logger)
        {
            var properties = new Dictionary<string, object>();
            foreach (var item in _options.Items) {
                var headerValue = context.Request.Headers[item.Header].FirstOrDefault();
                if (headerValue == null && item.Factory != null) {
                    headerValue = item.Factory(context);
                    context.Request.Headers[item.Header] = headerValue;
                }

                if (headerValue != null)
                    properties.Add(item.Property ?? item.Header, headerValue);
            }

            // Call the next delegate/middleware in the pipeline
            using (logger.BeginScope(properties)) {
                await _next(context);
            }
        }
    }
}
