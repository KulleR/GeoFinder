using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;

namespace GeoFinder.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbContext DbContext { get; set; }

        public Repository() { }

        public Repository(DbContext context)
        {
            DbContext = context;
        }
    }
}
