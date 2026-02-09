using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
//using TestWCFServiceCore.Service;
using TestWCFServiceLib.Service;

namespace TestWCFServiceCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceModelServices()
                .AddServiceModelMetadata()
                .AddAuthorization()
                .AddServiceModelConfigurationManagerFile(Path.Combine(AppContext.BaseDirectory, "WeatherService.config"));

            services.AddTransient<WeatherService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseServiceModel(builder =>
            {
                builder.AddService<WeatherService>();

                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
            });
        }
    }
}
