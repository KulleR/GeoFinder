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
        public LocationRepository(GeoDatabaseContext databaseContext) : base(databaseContext) { }

        public Task<List<Location>> GetAsync(string city)
        {
            return Task.Factory.StartNew(() => DbContext.GeoModel.LocationCollection.Where(r => r.City == city).ToList());
        }

        public Task<Location> GetAsync(int index)
        {
            return Task.Factory.StartNew(() =>
            {
                if (DbContext.GeoModel.LocationCollection.Count > index)
                {
                    return DbContext.GeoModel.LocationCollection[index];
                }

                return null;
            });
        }
    }
}
