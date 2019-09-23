using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Infrastructure;

namespace UserManagement.Service.Extentions
{
    public static class ExceptionHandlerMiddleware
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}
