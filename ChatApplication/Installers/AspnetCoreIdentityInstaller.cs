using ChatApplication.Domain.Contracts;
using ChatApplication.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using ChatApplication.Infrastructures.Stores;
using ChatApplication.Domain.Entities;

namespace ChatApplication.Installers
{
    public class AspnetCoreIdentityInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, AppSetting appSetting)
        {
            services.AddIdentity<User, Role>(setupAction =>
            {
                setupAction.Password.RequireDigit = true;
                setupAction.Password.RequiredUniqueChars = 1;
                setupAction.Password.RequireLowercase = false;
                setupAction.Password.RequireNonAlphanumeric = false;
                setupAction.Password.RequireUppercase = false;
                setupAction.Password.RequiredLength = 5;
                setupAction.SignIn.RequireConfirmedEmail = true;
                setupAction.SignIn.RequireConfirmedPhoneNumber = false;
                setupAction.User.RequireUniqueEmail = true;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ChatDbContext>();
        }
    }
}
