using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.RabbitMQ;
using System;
using WebApi.Cache;
using WebApi.SignalR;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var connection = Configuration.GetSection("SqlConnectionString").Value;
            services.AddEntityFrameworkSqlServer().AddDbContext<TrackerDBContext>
                (options => options.UseSqlServer(connection));

            services.AddMemoryCache();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSignalR();
            //services.AddCors(options => options.AddPolicy("CorsPolicy",
            //builder =>
            //{
            //    builder.AllowAnyMethod().AllowAnyHeader()
            //           .WithOrigins("http://localhost:59802")
            //           .AllowCredentials();
            //}));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMemoryCache cache,TrackerDBContext db, IConfiguration config, IHubContext<LiveEventHub> hubContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSignalR(routes =>
            {
                routes.MapHub<LiveEventHub>("/live");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            //app.UseCors("CorsPolicy");
          

            cache.Set("LeagueOfLegends", CacheCreator.CreateSportCacheById(1,db,Configuration));
            cache.Set("CSGO", CacheCreator.CreateSportCacheById(2, db, Configuration));
            cache.Set("Dota2", CacheCreator.CreateSportCacheById(3, db, Configuration));

            RabbitMQMessageReceiver r = new RabbitMQMessageReceiver();
            LiveEventHub hub = new LiveEventHub(hubContext);
            r.LiveEventReached += hub.SendEvent;
        }
    }
}
