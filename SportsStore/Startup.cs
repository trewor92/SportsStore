using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;



namespace SportsStore
{
    public class Startup
    {
        IConfigurationRoot Configuration;
        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));
            services.AddTransient<IProductRepository, EFProductRepository>();
            
            services.AddMvc();
            // services.AddTransient<IProductRepository, FakeProductRepository>();
            services.AddMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
            //<DotNetCliToolReference Include="Microsoft.AspNetCore.Razor.Tools" Version="1.1.0-preview4-final"  />

            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{pageNum:int}",
                    defaults: new { Controller = "Product", action = "List",pageNum=1 });
                routes.MapRoute(
                    name: null,
                    template: "Page{pageNum:int}",
                    defaults: new { Controller = "Product", action = "List",pageNum=1 });
                routes.MapRoute(
                    name: null,
                    template: "Page{category}",
                    defaults: new { Controller = "Product", action = "List" });
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { Controller = "Product", action = "List" });

                routes.MapRoute(
                    name: null,
                    template: "{controller=Product}/{action=List}/{id?}");
                
            });
            SeedData.EnsurePopulated(app);
        }
    }
}
