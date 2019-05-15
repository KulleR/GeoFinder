using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoFinder.Data.Models;

namespace GeoFinder.Data.Repositories.Interfaces
{
    public interface IRepository<out TEntity> where TEntity : IEntity
    {
        
    }
}
