using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthyService.WebPanel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Home/AccessDenied";
                options.LoginPath = "/Account/Home/Login";
            });


            services.AddRazorPages();
            //.AddFacebook(options =>
            //{
            //    options.ClientId = "593588681425639";
            //    options.ClientSecret = "c0717dd3385f66bbd7bf6cf3322cbff8";
            //    options.AccessDeniedPath = "/System/Account/AccessDenied";
            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //}); //TODO : Zainstalowac pakiet nugat, uzyskac client id i secret ze strony fb
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists=Dashboard}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
            InitializeHealthyService();

        }

        private async void InitializeHealthyService()
        {
            //TODO: Odpal baze danych, wgraj dane startowe ... 

            //HealthyService.Core.Database.DatabaseBus.DeleteDatabase();

            HealthyService.Core.Database.DatabaseBus.MigrateDatabase();

            //await new HealthyService.Core.Database.StartData().AddDataFromCodeAsync();

        }
    }
}
