using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;

namespace GeoFinder.Data.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        /// <summary>
        /// Загрузка всех объектов данной сущности
        /// </summary>
        /// <returns>Неупорядоченный список всех объектов</returns>
        Task<List<Location>> GetAllAsync();

        Task<Location> Get(string ip);
    }
}
