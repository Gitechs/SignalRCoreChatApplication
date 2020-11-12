using ChatApplication.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatApplication.Domain.Contracts
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services,AppSetting appSetting);
    }
}
