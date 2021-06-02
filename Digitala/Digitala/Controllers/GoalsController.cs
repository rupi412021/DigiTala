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

        [HttpPut]
        [Route("api/Goals")]
        public void Put([FromBody]Goals g)
        {
            g.Update();
        }

        [HttpDelete]
        [Route("api/Goals")]
        public List<Goals> Delete([FromBody]Goals g)
        {
            return g.Delete();
        }

        [HttpPost]
        [Route("api/Goals")]
        public List<Goals> Post([FromBody]Goals g)
        {
            return g.Insert();
        }
    }
}