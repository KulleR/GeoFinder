using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;

namespace GeoFinder.Data.Repositories
{
    public class RangeRepository : Repository<IpRange>, IRangeRepository
    {
        public RangeRepository(DbContext databaseContext) : base(databaseContext) { }

        public Task<List<IpRange>> GetAllAsync()
        {
            return Task.Factory.StartNew(() => DbContext.IpRangeCollection);
        }

        public Task<IpRange> Get(string ip)
        {
            return Task.Factory.StartNew(() => DbContext.IpRangeCollection.FirstOrDefault(r => r.IpFrom.ToString() == ip));
        }
    }
}
