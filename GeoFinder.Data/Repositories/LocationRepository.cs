using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;

namespace GeoFinder.Data.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(DbContext databaseContext) : base(databaseContext) { }

        public Task<List<Location>> GetAllAsync()
        {
            return Task.Factory.StartNew(() => DbContext.LocationCollection);
        }

        public Task<Location> Get(string city)
        {
            return Task.Factory.StartNew(() => DbContext.LocationCollection.FirstOrDefault(r => r.City == city));
        }
    }
}
