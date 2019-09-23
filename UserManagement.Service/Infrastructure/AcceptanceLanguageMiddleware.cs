using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Service.Infrastructure
{
    public class AcceptanceLanguageMiddleware
    {
        private readonly RequestDelegate _request;
        public AcceptanceLanguageMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task Invoke(HttpContext context)
        {
            var cultureName = "en-US";
            IList<CultureInfo> supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-Us"),
                new CultureInfo("ka-GE")
            };
            var queryCulture = context.Request.Headers["Accept-Language"].ToString();

            if (!string.IsNullOrWhiteSpace(queryCulture))
            {
                var requestedCulture = queryCulture.Split(',')[0];
                if (supportedCultures.Contains(new CultureInfo(requestedCulture)))
                    cultureName = requestedCulture;
            }

            var culture = new CultureInfo(cultureName);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            await _request(context);
        }
    }
}
