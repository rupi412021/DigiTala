using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class SchoolsController : ApiController
    {
        [HttpGet]
        [Route("api/Schools")]
        public List<Schools> Get()
        {
            Schools s = new Schools();
            List<Schools> tlist = s.Read();
            return tlist;
        }
    }
}