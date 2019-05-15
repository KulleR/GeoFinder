using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data;
using GeoFinder.Data.Repositories;
using GeoFinder.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeoFinder
{
    class IpRangeInfo
    {
        public uint ip_from;
        public uint ip_to;
        public uint location_index;
    }

    class LocationInfo
    {
        public string country;        // название страны (случайная строка с префиксом "cou_")
        public string region;        // название области (случайная строка с префиксом "reg_")
        public string postal;        // почтовый индекс (случайная строка с префиксом "pos_")
        public string city;          // название города (случайная строка с префиксом "cit_")
        public string organization;  // название организации (случайная строка с префиксом "org_")
        public float latitude;          // широта
        public float longitude;         // долгота
    }

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<DbContext>();
            services.AddScoped<IRangeRepository, RangeRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
