using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GeoFinder.Data.Models;
using Microsoft.AspNetCore.Hosting;

namespace GeoFinder.Data
{
    public class DbContext
    {
        private string _dbFileName = "geobase.dat";

        /// <summary>
        /// Версия база данных
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Название/префикс для базы данных
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Время создания базы данных
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Общее количество записей
        /// </summary>
        public int RecordsCount { get; set; }
        /// <summary>
        /// Смещение относительно начала файла до начала списка записей с геоинформацией
        /// </summary>
        public uint RangesOffset { get; set; }
        /// <summary>
        /// Смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        public uint CitiesOffset { get; set; }
        /// <summary>
        /// Смещение относительно начала файла до начала индекса с сортировкой по названию городов
        /// </summary>
        public uint LocationsOffset { get; set; }

        public List<IpRange> IpRangeCollection { get; set; }
        public List<Location> LocationCollection { get; set; }

        public DbContext(IHostingEnvironment hostingEnvironment)
        {
            IpRangeCollection = new List<IpRange>();
            LocationCollection = new List<Location>();

            Load(hostingEnvironment);
        }

        public void Load(IHostingEnvironment hostingEnvironment)
        {
            string filePath = Path.Combine(hostingEnvironment.ContentRootPath, _dbFileName);

            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                LoadHeader(reader);
                LoadRanges(reader);
                LoadLocations(reader);
                LoadIndexes(reader);
            }
        }

        private void LoadHeader(BinaryReader reader)
        {
            int version = reader.ReadInt32();
            byte[] nameBytes = new byte[32];
            reader.Read(nameBytes, 0, 32);
            ulong timestamp = reader.ReadUInt64();
            int records = reader.ReadInt32();
            uint offsetRanges = reader.ReadUInt32();
            uint offsetCities = reader.ReadUInt32();
            uint offsetLocations = reader.ReadUInt32();

            this.Version = version;
            this.Name = Encoding.UTF8.GetString(nameBytes, 0, nameBytes.Length);
            this.CreationDate = UnixTimeStampToDateTime(timestamp);
            this.RecordsCount = records;
            this.RangesOffset = offsetRanges;
            this.CitiesOffset = offsetCities;
            this.LocationsOffset = offsetLocations;
        }

        private void LoadRanges(BinaryReader reader)
        {
            for (int i = 0; i < this.RecordsCount; i++)
            {
                this.IpRangeCollection.Add(new IpRange()
                {
                    IpFrom = reader.ReadUInt32(),
                    IpTo = reader.ReadUInt32(),
                    LocationIndex = reader.ReadUInt32()
                });
            }
        }

        private void LoadLocations(BinaryReader reader)
        {
            for (int i = 0; i < this.RecordsCount; i++)
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

                this.LocationCollection.Add(new Location()
                {
                    Country = Encoding.UTF8.GetString(country, 0, country.Length),
                    Region = Encoding.UTF8.GetString(region, 0, region.Length),
                    Postal = Encoding.UTF8.GetString(postal, 0, postal.Length),
                    City = Encoding.UTF8.GetString(city, 0, city.Length),
                    Organization = Encoding.UTF8.GetString(organization, 0, organization.Length),
                    Latitude = reader.ReadSingle(),
                    Longitude = reader.ReadSingle()
                });
            }
        }

        private void LoadIndexes(BinaryReader reader)
        {

        }

        private DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
