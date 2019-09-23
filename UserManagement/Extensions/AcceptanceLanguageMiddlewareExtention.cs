using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Infrastructure;

namespace UserManagement.Extensions
{
    public static class AcceptanceLanguageMiddlewareExtention
    {
        public static IApplicationBuilder UseAcceptanceLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AcceptanceLanguageMiddleware>();
        }
    }
}
