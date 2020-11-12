using ChatApplication.Domain.Contracts;
using ChatApplication.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Installers
{
    public class Jwtinstaller : IInstaller
    {
        private ILogger logger;

        public void InstallServices(IServiceCollection services, AppSetting appSetting)
        {
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = (ILoggerFactory)serviceProvider.GetService(typeof(ILoggerFactory));
            logger = loggerFactory.CreateLogger(typeof(ILogger));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var secretkey = Encoding.UTF8.GetBytes(appSetting.JwtSetting.SecretKey);
                    // var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                    var validationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero, // default: 5 min
                        RequireSignedTokens = true,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                        RequireExpirationTime = true,
                        ValidateLifetime = true,

                        ValidateAudience = true, //default : false
                        ValidAudience = appSetting.JwtSetting.Audience,

                        ValidateIssuer = true, //default : false
                        ValidIssuer = appSetting.JwtSetting.Issuer,
                        // TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                    };

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = validationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            //logger.LogError("Authentication failed.", context.Exception);

                            if (context.Exception != null)
                                logger.LogError(context.Exception, "Authentication failed");
                            //throw new Exception("Authentication failed", context.Exception); //AppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                            return Task.FromResult(context);
                        },
                    };
                });
        }
    }
}
