using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class EsportsResultsController : Controller
    {
        private TrackerDBContext db;
        public EsportsResultsController(TrackerDBContext db)
        {
            if (this.db == null)
            {
                this.db = db;
            }
        }
        [HttpGet("[action]")]
        public IEnumerable<Results> GetResults()
        {
            var results = db.Results.ToList();
            foreach(var result in results)
            {
                yield return result;
            }
            
        }
    }
}
