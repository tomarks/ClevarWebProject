using ClevarWeb.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.IO;
using Westwind.AspNetCore.LiveReload;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using ClevarWeb.Utility;
using System.Linq;
using ClevarWeb.Data.Models;
using AutoMapper;

namespace ClevarWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            SystemProperties.Instance.IncreaseRevision(Configuration);
            General.Instance.wwwRootPath = env.WebRootPath;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication(opt =>
            {
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }
            ).AddCookie(opt => {
                opt.LoginPath = new PathString("/Admin/Login");

                if(SystemProperties.Instance.LoginTimeoutMinutes > 0)
                    opt.ExpireTimeSpan = TimeSpan.FromMinutes(SystemProperties.Instance.LoginTimeoutMinutes);
            });

            if (Debugger.IsAttached)
            {
                services.AddLiveReload(cfg =>
                {
                    cfg.LiveReloadEnabled = true;
                    cfg.ClientFileExtensions = ".cshtml,.css,.js,.htm,.html,.ts,.razor";
                });
            }

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin");
                options.Conventions.AllowAnonymousToPage("/Admin/Login");
            }).AddRazorRuntimeCompilation();

            if(Debugger.IsAttached)
            {
                services.AddDbContext<ClevarDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ClevarDbContextDev")));
            }
            else
            {
                services.AddDbContext<ClevarDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ClevarDbContext")));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ClevarWeb.Data.ClevarDbContext context)
        {
            try
            {
                var db = context;
                db.Database.Migrate();
                db.SystemHealthCheckAndSeeding();


                if (SystemProperties.Instance.SystemSettings == null)
                {
                    SystemProperties.Instance.SystemSettings = db.SystemSettings.First();
                }
            }
            catch (Exception ex)
            {
                General.Instance.WriteToLog(ex.Message + Environment.NewLine + ex.ToString());
            }

            if (Debugger.IsAttached)
                app.UseLiveReload();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseStatusCodePagesWithReExecute("/Error", "?statusCode={0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }
    }
}
