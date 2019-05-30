using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using GeoFinder.Data.AutoMapper;
using GeoFinder.Data.Helpers;
using GeoFinder.Data.Models;
using GeoFinder.IO;
using GeoFinder.IO.Models.BinaryFileModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace GeoFinder.Data
{
    /// <summary>
    /// Экземпляр DbContext представляет сеанс с базой данных и может использоваться для
    /// запроса экземпляров ваших сущностей.
    /// </summary>
    public class GeoDatabaseContext
    {
        private string _dbFileName = "geobase.dat";
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;
        private readonly ICommonMapper _mapper;
        /// <summary>
        /// Количество байтов в записи с информацией об интервалах IP адресов
        /// </summary>
        private const int IpRangeBytesCount = 12;
        /// <summary>
        /// Количество байтов в записи с информацией о местоположении
        /// </summary>
        private const int LocationBytesCount = 96;
        /// <summary>
        /// Количество байтов в записи с индексом записи местоположения
        /// </summary>
        private const int IndexBytesCount = 4;

        public GeoModel GeoModel { get; set; }
        /// <summary>
        /// Время загрузки базы данных в мс
        /// </summary>
        public long DatabaseLoadedTimeMs { get; set; }


        public GeoDatabaseContext(IHostingEnvironment hostingEnvironment, ICommonMapper mapper,
            IHostingEnvironment environment, ILogger<GeoDatabaseContext> logger)
        {
            _logger = logger;
            _environment = environment;
            _mapper = mapper;
        }

        /// <summary>
        /// Возвращает коллекцию экземпляров заданного класса сущности
        /// или значение NULL, если такого постоянного экземпляра нет.
        /// </summary>
        public IEnumerable<T> Query<T>() where T:IEntity
        {
            if (typeof(T) == typeof(Location))
            {
                return (IEnumerable<T>)GeoModel.LocationCollection;
            }

            if (typeof(T) == typeof(IpRange))
            {
                return (IEnumerable<T>)GeoModel.IpRangeCollection;
            }

            return null;
        }

        /// <summary>
        /// Загрузка базы данных в память
        /// </summary>
        /// <returns></returns>
        public void Load()
        {
            string filePath = Path.Combine(_environment.ContentRootPath, _dbFileName);
            BinGeoModel binModel;
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();

                var bufferedReader = new BufferedBinaryReader(stream, 8192);
                stopwatch.Start();

                bufferedReader.FillBuffer();

                int version = bufferedReader.ReadInt32();
                byte[] nameBytes = bufferedReader.Read(0, 32);
                ulong timestamp = bufferedReader.ReadUInt64();
                int records = bufferedReader.ReadInt32();
                uint offsetRanges = bufferedReader.ReadUInt32();
                uint offsetCities = bufferedReader.ReadUInt32();
                uint offsetLocations = bufferedReader.ReadUInt32();

                binModel = new BinGeoModel(version, nameBytes, timestamp, records, offsetRanges, offsetCities, offsetLocations);

                int currentIndex = 0;
                binModel.IpRangeCollection = new BinIpRange[binModel.RecordsCount];
                while (bufferedReader.FillBuffer() && currentIndex < binModel.RecordsCount)
                {
                    for (; currentIndex < binModel.RecordsCount && bufferedReader.NumBytesAvailable >= IpRangeBytesCount; currentIndex++)
                    {
                        binModel.IpRangeCollection[currentIndex] = new BinIpRange()
                        {
                            IpFrom = bufferedReader.ReadUInt32(),
                            IpTo = bufferedReader.ReadUInt32(),
                            LocationIndex = bufferedReader.ReadUInt32()
                        };
                    }
                }

                currentIndex = 0;
                binModel.LocationCollection = new BinLocation[binModel.RecordsCount];
                while (bufferedReader.FillBuffer() && currentIndex < binModel.RecordsCount)
                {
                    for (; currentIndex < binModel.RecordsCount && bufferedReader.NumBytesAvailable >= LocationBytesCount; currentIndex++)
                    {
                        byte[] country = bufferedReader.Read(0, 8);
                        byte[] region = bufferedReader.Read(0, 12);
                        byte[] postal = bufferedReader.Read(0, 12);
                        byte[] city = bufferedReader.Read(0, 24);
                        byte[] organization = bufferedReader.Read(0, 32);
                        float latitude = bufferedReader.ReadSingle();
                        float longitude = bufferedReader.ReadSingle();

                        binModel.LocationCollection[currentIndex] = new BinLocation(country, region, postal, city, organization, latitude, longitude);
                    }
                }

                currentIndex = 0;
                binModel.Indexes = new uint[binModel.RecordsCount];
                while (bufferedReader.FillBuffer() && currentIndex < binModel.RecordsCount)
                {
                    for (; currentIndex < binModel.RecordsCount && bufferedReader.NumBytesAvailable >= IndexBytesCount; currentIndex++)
                    {
                        binModel.Indexes[currentIndex] = bufferedReader.ReadUInt32();
                    }
                }

                stopwatch.Stop();
                DatabaseLoadedTimeMs = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation($"Database loading time: {stopwatch.ElapsedMilliseconds} ms");
            }

            // Отображение объектов сущностей двоичной базы в объекты бизнес сущностей
            GeoModel = _mapper.Map<GeoModel>(binModel);
        }
    }
}
