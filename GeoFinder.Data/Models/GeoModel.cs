using System;
using System.Collections.Generic;
using System.Text;

namespace GeoFinder.Data.Models
{
    public class GeoModel
    {
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
        public List<int> Indexes { get; set; }

        public GeoModel()
        {
            IpRangeCollection = new List<IpRange>();
            LocationCollection = new List<Location>();
            Indexes = new List<int>();
        }
    }
}
