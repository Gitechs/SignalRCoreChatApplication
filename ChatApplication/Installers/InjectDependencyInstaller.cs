using System.Reflection;
using ChatApplication.Domain.Attributes;
using ChatApplication.Domain.Contracts;
using ChatApplication.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace ChatApplication.Installers {
    public class InjectDependencyInstaller : IInstaller {
        void IInstaller.InstallServices (IServiceCollection services, AppSetting appSetting) {
            services.Scan (scan => scan
                .FromAssemblies (Assembly.GetExecutingAssembly ())
                .AddClasses (classes => classes.WithAttribute (typeof (ScopeDependecyAttribute)))
                .UsingRegistrationStrategy (RegistrationStrategy.Skip)
                .AsImplementedInterfaces ()
                .AsSelf ()
                .WithScopedLifetime ());

            services.Scan (scan => scan
                .FromAssemblies (Assembly.GetExecutingAssembly ())
                .AddClasses (classes => classes.WithAttribute (typeof (TransientDependecyAttribute)))
                .UsingRegistrationStrategy (RegistrationStrategy.Skip)
                .AsImplementedInterfaces ()
                .AsSelf ()
                .WithTransientLifetime ());

            services.Scan (scan => scan
                .FromAssemblies (Assembly.GetExecutingAssembly ())
                .AddClasses (classes => classes.WithAttribute (typeof (SingletonDependecyAttribute)))
                .UsingRegistrationStrategy (RegistrationStrategy.Skip)
                .AsImplementedInterfaces ()
                .AsSelf ()
                .WithSingletonLifetime ());
        }
    }
}