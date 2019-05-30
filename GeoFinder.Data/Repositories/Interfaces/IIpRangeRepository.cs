using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;

namespace GeoFinder.Data.Repositories.Interfaces
{
    public interface IRangeRepository : IRepository<IpRange>
    {
        /// <summary>
        /// Загрузка информации об интервале IP адреса
        /// </summary>
        /// <param name="ip">IP адреса в строковом представлении</param>
        /// <returns></returns>
        Task<IpRange> GetAsync(string ip);
    }
}
