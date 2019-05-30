using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;

namespace GeoFinder.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected GeoDatabaseContext DbContext { get; set; }

        public Repository() { }

        public Repository(GeoDatabaseContext context)
        {
            DbContext = context;
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.Factory.StartNew(() => DbContext.Query<TEntity>());
        }
    }
}
