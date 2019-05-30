using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;

namespace GeoFinder.Data.Repositories.Interfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        /// <summary>
        /// Загрузка информации о местоположении по указанному названию города
        /// </summary>
        Task<List<Location>> GetAsync(string city);

        /// <summary>
        /// Загрузка информации о местоположении по индексу в списке
        /// </summary>
        Task<Location> GetAsync(int index);
    }
}
