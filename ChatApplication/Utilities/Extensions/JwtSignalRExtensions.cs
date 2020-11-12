using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;

namespace ChatApplication.Utilities.Extensions
{
    public static class JwtSignalRExtensions
    {
        private static readonly string AUTH_QUERY_STRING_KEY = "access_token";

        public static void UseJwtSignalRAuthentication(this IApplicationBuilder app, ILogger logger)
        {
            app.Use(async (context, next) =>
            {
                if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                {
                    try
                    {
                        if (context.Request.QueryString.HasValue)
                        {

                            var token = context.Request.Query[AUTH_QUERY_STRING_KEY];
                     
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"read token from querystring throw error");
                        // if multiple headers it may throw an error.  Ignore both.
                    }
                }
                await next.Invoke();
            });

        }
    }
}
