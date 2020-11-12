using ChatApplication.Domain.Contracts;
using ChatApplication.Infrastructures.Stores;
using ChatApplication.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Installers
{
    public class DbContextInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services,AppSetting appSetting)
        {
            services.AddDbContextPool<ChatDbContext>(opt =>
            {
                opt.UseSqlServer(appSetting.ConncetionString.ChatDbConnnectionString);
            });
        }
    }
}
