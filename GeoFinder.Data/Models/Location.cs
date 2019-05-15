using System;
using System.Collections.Generic;
using System.Text;

namespace GeoFinder.Data.Models
{
    /// <summary>
    /// Хранит информацию о местоположении с координатами (долгота и широта)
    /// </summary>
    public class Location : IEntity
    {
        /// <summary>
        /// Название страны (случайная строка с префиксом "cou_")
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Название области (случайная строка с префиксом "reg_")
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// Почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public string Postal { get; set; }
        /// <summary>
        /// Название города (случайная строка с префиксом "cit_")
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Название организации (случайная строка с префиксом "org_")
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// Широта
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// Долгота
        /// </summary>
        public float Longitude { get; set; }
    }
}
