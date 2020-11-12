using ChatApplication.Domain.Contracts;
using ChatApplication.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSetting appSetting)
        {
            services.AddControllersWithViews().AddNewtonsoftJson();
        }
    }
}
