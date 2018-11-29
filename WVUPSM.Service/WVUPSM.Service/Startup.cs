using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using WVUPSM.DAL.Repos.Interfaces;
using WVUPSM.Models.Entities;
using WVUPSM.Service.Filters;

namespace WVUPSM.Service
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment Enviroment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Enviroment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //Sets up MVC and JSON Formatting
            services.AddMvcCore(config =>
                    config.Filters.Add(new WVUPSMExceptionFilter(Enviroment.IsDevelopment())))
                .AddJsonFormatters(j =>
                {
                    j.ContractResolver = new DefaultContractResolver();
                    j.Formatting = Formatting.Indented;
                })
                //This below code fixed JSON not displaying NAV Props -> https://stackoverflow.com/questions/41373878/json-response-does-not-contain-all-the-navigation-properties-entityframework-cor
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            if (Enviroment.IsDevelopment())
            {
                services.AddDbContext<SMContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WVUPSM")));
            }
            else
            {
                services.AddDbContext<SMContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WVUPSMProduction")));
            }


            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<SMContext>()
                .AddDefaultTokenProviders();


            //Setting up dependecy injection for REPOS
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IPostRepo, PostRepo>();
            services.AddScoped<IFollowRepo, FollowRepo>();
            services.AddScoped<IGroupRepo, GroupRepo>();
            services.AddScoped<ICommentRepo, CommentRepo>();
            services.AddScoped<IMessageRepo, MessageRepo>();
            services.AddScoped<IFileRepo, FileRepo>();
            services.AddScoped<ITagRepo, TagRepo>();
            services.AddScoped<INotificationRepo, NotificationRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SMContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                DbInitializer.InitializeData(db);
            }
            if (env.IsProduction())
            {
                DbInitializer.ProductionInitializeData(db);
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
