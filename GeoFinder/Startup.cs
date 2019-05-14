using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            string path = $@"{env.ContentRootPath}\geobase.dat";

            // создаем объект BinaryReader
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                // пока не достигнут конец файла
                // считываем каждое значение из файла

                int version = reader.ReadInt32();//4

                byte[] nameBytes = new byte[32];
                int c = reader.Read(nameBytes, 0, 32);//32
                string name = Encoding.UTF8.GetString(nameBytes, 0, nameBytes.Length);

                ulong timestamp = reader.ReadUInt64();//8
                int records = reader.ReadInt32();//4
                uint offset_ranges = reader.ReadUInt32();//4
                uint offset_cities = reader.ReadUInt32();//4
                uint offset_locations = reader.ReadUInt32();//4

                string outputString = $"DB: {version}\n" +
                                      $"Prefix: {name}\n" +
                                      $"Creation datetime: {timestamp}\n" +
                                      $"Rows count: {records}\n" +
                                      $"Offset ranges: {offset_ranges}\n" +
                                      $"Offset cities: {offset_cities}\n" +
                                      $"Offset locations: {offset_locations}\n";

                List<IpRangeInfo> ipRanges = new List<IpRangeInfo>();

                for (int i = 0; i < records; i++)
                {
                    ipRanges.Add(new IpRangeInfo()
                    {
                        ip_from = reader.ReadUInt32(),
                        ip_to = reader.ReadUInt32(),
                        location_index = reader.ReadUInt32()
                    });
                }

                List<LocationInfo> locations = new List<LocationInfo>();

                for (int i = 0; i < records; i++)
                {
                    byte[] country = new byte[8];
                    byte[] region = new byte[12];
                    byte[] postal = new byte[12];
                    byte[] city = new byte[24];
                    byte[] organization = new byte[32];

                    reader.Read(country, 0, 8);
                    reader.Read(region, 0, 12);
                    reader.Read(postal, 0, 12);
                    reader.Read(city, 0, 24);
                    reader.Read(organization, 0, 32);

                    locations.Add(new LocationInfo()
                    {
                        country = Encoding.UTF8.GetString(country, 0, country.Length),
                        region = Encoding.UTF8.GetString(region, 0, region.Length),
                        postal = Encoding.UTF8.GetString(postal, 0, postal.Length),
                        city = Encoding.UTF8.GetString(city, 0, city.Length),
                        organization = Encoding.UTF8.GetString(organization, 0, organization.Length),
                        latitude = reader.ReadSingle(),
                        longitude = reader.ReadSingle()
                    });
                }
            }
        }
    }
}
