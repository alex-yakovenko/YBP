using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.BP;
using Sample.BP.UserRegistration;
using Sample.Data;
using Sample.Definitions.Companies;
using Sample.Services.Company;
using YBP.Framework;
using YBP.Framework.Regisry;

namespace Scheduler.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            YbpConfiguration.LoadActionsFromAssembly<CreateUserAction>();
            Mapper.Initialize(c => {
                c.AddProfiles(typeof(SampleDbContext).Assembly); // Sample.Data
                c.AddProfiles(typeof(CreateUserAction).Assembly); // Sample.BP
                c.AddProfiles(typeof(ICompanyValidator).Assembly); // Sample.Definitions
                c.AddProfiles(typeof(CompanyValidator).Assembly); // SampleServices
            });

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.InitBLServices();
            services.AddSingleton(new YbpUserContext { { "UserId", 75675 } });


            var ybpConnectionString = Configuration["YbpConnectionString"];
            var ybpSampleConnectionString = Configuration["YbpSampleAppConnectionString"];

            services.InitDataContext(ybpSampleConnectionString);
            services.InitYbpDataContext(ybpConnectionString);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
