using System;
using System.Collections.Generic;
using System.Text;

namespace GeoFinder.Data.Helpers
{
    /// <summary>
    /// Класс со вспомогающим функционалом для работы с временем
    /// </summary>
    public static class DateTimeHelpers
    {
        /// <summary>
        /// Переводит unix timestamp в DateTime
        /// </summary>
        public static DateTime UnixTimeStampToDateTime(ulong unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
