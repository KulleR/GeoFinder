using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;

namespace GeoFinder.Data.Repositories
{
    public class RangeRepository : Repository<IpRange>, IRangeRepository
    {
        public RangeRepository(GeoDatabaseContext databaseContext) : base(databaseContext) { }

        public Task<IpRange> GetAsync(string ip)
        {
            uint parsedIp = BitConverter.ToUInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);
            return Task.Factory.StartNew(() => DbContext.GeoModel.IpRangeCollection.FirstOrDefault(r => r.IpFrom <= parsedIp && r.IpTo >= parsedIp));
        }
    }
}
