
using log4net.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using UserSampleApi.Model;

namespace UserSampleApi.Middleware
{
    public static class UsersApiExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<UsersApiExceptionMiddleware>();
        }

    }
}
