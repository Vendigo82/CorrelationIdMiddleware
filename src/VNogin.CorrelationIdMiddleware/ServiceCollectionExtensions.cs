using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNogin.CorrelationIdMiddleware;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCorrelationId(this IServiceCollection services, Action<CorrelationIdMiddlewareSettings> adjustSettings)
        {
            //var settings = new CorrelationIdMiddlewareSettings();
            //adjustSettings?.Invoke(settings);

            services.Configure<CorrelationIdMiddlewareSettings>(adjustSettings);
            return services;
        }

        public static IServiceCollection AddCorrelationId(this IServiceCollection services, 
            string headerName, 
            string propertyName, 
            Func<HttpContext, string> factory)
        {            
            services.Configure<CorrelationIdMiddlewareSettings>((s) => s.Items.Add(new CorrelationIdMiddlewareSettings.SettingsItem { 
               Header = headerName,
               Property = propertyName,
               Factory = factory
            }));

            services.AddTransient<CorrelationIdMiddlewareSettings>((sp) => sp.GetRequiredService<IOptions<CorrelationIdMiddlewareSettings>>().Value);

            //services.AddHeaderPropagation(settings => settings.Headers.Add(headerName));

            return services;
        }

        public static IServiceCollection AddCorrelationId(this IServiceCollection services) => AddCorrelationId(services, "X-Correlation-ID", "CorrelationId", TraceIdFactory);

        public static string TraceIdFactory(HttpContext context) => context.TraceIdentifier;
    }
}
