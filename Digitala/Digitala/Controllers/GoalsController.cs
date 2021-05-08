using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class GoalsController : ApiController
    {
        [HttpGet]
        [Route("api/Goals/{studentId}/{year}")]
        public List<Goals> Get(string studentId, int year)
        {
            Goals g = new Goals();
            List<Goals> glist = g.Read(studentId, year);
            return glist;
        }
    }
}