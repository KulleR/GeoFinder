namespace GeoFinder.IO.Models.BinaryFileModels
{
    /// <summary>
    /// Хранит информацию о местоположении с координатами (долгота и широта)
    /// </summary>
    public class BinLocation
    {
        /// <summary>
        /// Название страны (случайная строка с префиксом "cou_")
        /// </summary>
        public byte[] Country { get; set; }
        /// <summary>
        /// Название области (случайная строка с префиксом "reg_")
        /// </summary>
        public byte[] Region { get; set; }
        /// <summary>
        /// Почтовый индекс (случайная строка с префиксом "pos_")
        /// </summary>
        public byte[] Postal { get; set; }
        /// <summary>
        /// Название города (случайная строка с префиксом "cit_")
        /// </summary>
        public byte[] City { get; set; }
        /// <summary>
        /// Название организации (случайная строка с префиксом "org_")
        /// </summary>
        public byte[] Organization { get; set; }
        /// <summary>
        /// Широта
        /// </summary>
        public float Latitude { get; set; }
        /// <summary>
        /// Долгота
        /// </summary>
        public float Longitude { get; set; }

        public BinLocation(byte[] country, byte[] region, byte[] postal, byte[] city, byte[] organization, float latitude, float longitude)
        {
            Country = country;
            Region = region;
            Postal = postal;
            City = city;
            Organization = organization;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
