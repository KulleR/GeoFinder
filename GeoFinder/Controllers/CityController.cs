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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public CityController(ILocationRepository locationRepository)
        {
            this._locationRepository = locationRepository;
        }
       
        [HttpGet]
        public async Task<ActionResult<List<Location>>> Get()
        {
            return Ok(await _locationRepository.GetAllAsync());
        }
        
        [HttpGet("{city}")]
        public async Task<ActionResult<List<Location>>> Locations(string city)
        {
            return Ok(await _locationRepository.GetAsync(city));
        }
    }
}