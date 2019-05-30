using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;

namespace GeoFinder.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Загрузка всех объектов данной сущности
        /// </summary>
        /// <returns>Неупорядоченный список всех объектов</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
