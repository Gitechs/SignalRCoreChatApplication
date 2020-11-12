using ChatApplication.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ChatApplication.Utilities.Extensions
{
    public static class InstallerExtensions
    {
        private static void InstallServicesFromAssembly(IServiceCollection services, Assembly assembly, AppSetting appSetting)
        {
            List<IInstaller> installers = (from t in assembly.GetTypes()
                                           where t.GetInterfaces().Contains(typeof(IInstaller))
                                                    && t.GetConstructor(Type.EmptyTypes) != null
                                           select Activator.CreateInstance(t) as IInstaller).ToList();

            installers.ForEach(installer => installer.InstallServices(services, appSetting));
        }

        public static void InstallServicesFromCallinfAssembly(this IServiceCollection services, AppSetting appSetting)
        {
            InstallServicesFromAssembly(services, Assembly.GetExecutingAssembly(), appSetting);
        }

        public static void InstallServicesInAssemblies(this IServiceCollection services, AppSetting appSetting, params Assembly[] assemblies)
        {
            foreach (Assembly item in assemblies)
            {
                InstallServicesFromAssembly(services, item, appSetting);
            }
        }
    }
}
