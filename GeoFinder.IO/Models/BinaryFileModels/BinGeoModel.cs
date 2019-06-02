using System.Collections.Generic;

namespace GeoFinder.IO.Models.BinaryFileModels
{
    /// <summary>
    /// Модель представляющую данные двоичной базы данных
    /// </summary>
    public class BinGeoModel
    {
        /// <summary>
        /// Версия база данных
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Название/префикс для базы данных
        /// </summary>
        public byte[] Name { get; set; }
        /// <summary>
        /// Время создания базы данных
        /// </summary>
        public ulong CreationDate { get; set; }
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

        public BinIpRange[] IpRangeCollection { get; set; }
        public BinLocation[] LocationCollection { get; set; }
        public uint[] Indexes { get; set; }

        public BinGeoModel()
        {
        }

        public BinGeoModel(int version, byte[] name, ulong creationDate, int recordsCount, uint rangesOffset, uint citiesOffset, uint locationsOffset)
        {
            Version = version;
            Name = name;
            CreationDate = creationDate;
            RecordsCount = recordsCount;
            RangesOffset = rangesOffset;
            CitiesOffset = citiesOffset;
            LocationsOffset = locationsOffset;
        }
    }
}
