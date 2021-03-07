using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class TargetsController : ApiController
    {
        [HttpGet]
        [Route("api/Targets")]
        public List<Targets> Get()
        {
            Targets t = new Targets();
            List<Targets> tlist = t.Read();
            return tlist;
        }

        [HttpDelete]
        [Route("api/Targets")]
        public List<Targets> Delete([FromBody]Targets t)
        {
            return t.Delete();
        }
    }
}