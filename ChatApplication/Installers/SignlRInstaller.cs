using ChatApplication.Domain.Contracts;
using ChatApplication.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ChatApplication.Installers
{
    public class SignlRInstaller : IInstaller
    {
        private IWebHostEnvironment env;

        public void InstallServices(IServiceCollection services, AppSetting appSetting)
        {
            var serviceProvider = services.BuildServiceProvider();
            env = serviceProvider.GetService<IWebHostEnvironment>();

            if (env.IsDevelopment())
                services.AddSignalR(cfg =>
                {
                    if (env.IsDevelopment())
                    {
                        cfg.EnableDetailedErrors = true;
                    }

                    cfg.ClientTimeoutInterval = TimeSpan.FromMinutes(10);
                    //}.AddNewtonsoftJsonProtocol();
                }).AddJsonProtocol();
        }
    }
}
