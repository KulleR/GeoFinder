using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeoFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
        private readonly IRangeRepository _rangeRepository;

        public IpController(IRangeRepository rangeRepository)
        {
            this._rangeRepository = rangeRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<IpRange>>> Get()
        {
            return await _rangeRepository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{ip}")]
        public async Task<ActionResult<IpRange>> Get(string ip)
        {
            return await _rangeRepository.Get(ip);
        }
    }
}
