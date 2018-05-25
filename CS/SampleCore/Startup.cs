using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Bootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using System.IO;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using SampleCore.Models;
using SampleCore.Utils;

namespace SampleCore {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            ConnectionStringsProvider = new ConnectionStringsProvider(Configuration, env.ContentRootPath);
        }

        public IConfiguration Configuration { get; }
        public ConnectionStringsProvider ConnectionStringsProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            
            services.AddMvc();
            services.AddDevExpressControls(options => {
                options.Bootstrap(bootstrapOptions => {
                    bootstrapOptions.IconSet = BootstrapIconSet.Embedded;
                    bootstrapOptions.Mode = BootstrapMode.Bootstrap4;
                });
                options.Resources = ResourcesType.DevExtreme;
            });
            // Sample data context registration
            services.AddLargeDatabaseUnitOfWork(ConnectionStringsProvider.GetConnectionString("XPLargeDatabaseConnectionString"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseDevExpressControls();
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                            }
            else
                app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();
                        app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sample}/{action=GridView}/{id?}");
            });
        }
    }
}