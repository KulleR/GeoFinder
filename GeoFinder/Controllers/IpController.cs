using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeoFinder.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IRangeRepository _rangeRepository;
        private readonly ILocationRepository _locationRepository;

        public IpController(IRangeRepository rangeRepository, ILocationRepository locationRepository)
        {
            this._rangeRepository = rangeRepository;
            this._locationRepository = locationRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IpRange>>> Get()
        {
            return Ok(await _rangeRepository.GetAllAsync());
        }
        
        [HttpGet("{ip}")]
        public async Task<ActionResult<Location>> Location(string ip)
        {
            IpRange ipRange = await _rangeRepository.GetAsync(ip);
            return Ok(await _locationRepository.GetAsync((int)ipRange.LocationIndex));
        }
    }
}
