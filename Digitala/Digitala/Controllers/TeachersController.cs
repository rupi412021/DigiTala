using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class TeachersController : ApiController
    {
        public void Post([FromBody] Teachers t)
        {
            t.Insert();
        }

        public List<Teachers> Get()
        {
            Teachers t = new Teachers();
            List<Teachers> tlist = t.Read();
            return tlist;
        }

        [HttpPost]
        [Route("api/Students")]
        public List<Students> Post([FromBody] Students s)
        {
            return s.Insert();
        }
    }
}