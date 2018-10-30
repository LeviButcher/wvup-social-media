using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WVUPSM.DAL.EF;
using WVUPSM.Models.Entities;
using WVUPSM.MVC.Configuration;
using WVUPSM.MVC.Service;
using WVUPSM.MVC.WebServiceAccess;
using WVUPSM.MVC.WebServiceAccess.Base;
using WVUPSM.Services;

namespace WVUPSM.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            services.AddSingleton<IWebServiceLocator, WebServiceLocator>();
            services.AddSingleton<IWebApiCalls, WebApiCalls>();

            if (Environment.IsDevelopment())
            {
                services.AddDbContext<SMContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WVUPSM")));
            }
            else
            {
                services.AddDbContext<SMContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WVUPSMProduction")));
            }

            services.AddIdentity<User, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<SMContext>()
                .AddDefaultTokenProviders();

            var id = Configuration["SendGridKey"];
            var user = Configuration["SendGridUser"];

            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc()
            .AddJsonOptions(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            //Ensure uploads folder exists
            Directory.CreateDirectory(Path.Combine(env.WebRootPath, "uploads"));

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
