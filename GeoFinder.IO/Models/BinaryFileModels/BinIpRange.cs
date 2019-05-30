namespace GeoFinder.IO.Models.BinaryFileModels
{
    /// <summary>
    /// Хранит информацию об интервалах IP адресов
    /// </summary>
    public class BinIpRange
    {
        /// <summary>
        /// Начало диапазона IP адресов
        /// </summary>
        public uint IpFrom { get; set; }
        /// <summary>
        /// Конец диапазона IP адресов
        /// </summary>
        public uint IpTo { get; set; }
        /// <summary>
        /// Индекс записи о местоположении
        /// </summary>
        public uint LocationIndex { get; set; }
    }
}
