using ChatApplication.Utilities;
using ChatApplication.Utilities.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ChatApplication
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new NullReferenceException("configuration can't be null");
            AppSetting = configuration.GetSection("AppSetting").Get<AppSetting>();
        }

        public IConfiguration Configuration { get; }
        public AppSetting AppSetting { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSetting>(Configuration.GetSection("AppSetting"));
            services.AddControllersWithViews();

            //Automatically register all dependencies in DI by reflection

            //you neet to add new class for each dependecies you have. this class have to Implemented 'IInstaller' interface.
            // on starup when application loaded, all the classes that Implemented 'IInstaller' interface will tracked by reflection and
            //autumatically will add to DI.
            services.InstallServicesFromCallinfAssembly(AppSetting);
        }
    }
}
