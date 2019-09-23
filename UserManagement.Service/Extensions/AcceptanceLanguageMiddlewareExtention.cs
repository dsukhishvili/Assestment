using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Infrastructure;

namespace UserManagement.Service.Extensions
{
    public static class AcceptanceLanguageMiddlewareExtention
    {
        public static IApplicationBuilder UseAcceptanceLanguageMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AcceptanceLanguageMiddleware>();
        }
    }
}
