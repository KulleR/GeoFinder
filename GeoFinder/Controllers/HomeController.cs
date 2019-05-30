using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoFinder.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeoFinder.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoDatabaseContext _databaseContext;
        public HomeController(GeoDatabaseContext dbContext)
        {
            _databaseContext = dbContext;
        }

        public ActionResult Index()
        {
            ViewBag.DatabseLoadedTimeMs = _databaseContext.DatabaseLoadedTimeMs;
            return View();
        }
    }
}