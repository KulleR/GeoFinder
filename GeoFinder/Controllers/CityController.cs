using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoFinder.Data.Models;
using GeoFinder.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoFinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public CityController(ILocationRepository locationRepository)
        {
            this._locationRepository = locationRepository;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<List<Location>>> Get()
        {
            return await _locationRepository.GetAllAsync();
        }

        // GET api/values/5
        [HttpGet("{city}")]
        public async Task<ActionResult<Location>> Get(string city)
        {
            return await _locationRepository.Get(city);
        }
    }
}