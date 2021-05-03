using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNogin.CorrelationIdMiddleware
{
    public class CorrelationIdMiddlewareSettings
    {
        public ICollection<SettingsItem> Items { get; set; } = new List<SettingsItem>();

        public class SettingsItem
        {
            public string Header { get; set; }

            public string Property { get; set; }

            public Func<HttpContext, string> Factory { get; set; }
        }
    }
}
