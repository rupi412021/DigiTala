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

        [HttpGet]
        [Route("api/Targets/{studentId}/{year}")]
        public List<Targets> Get(string studentId, int year)
        {
            Targets t = new Targets();
            List<Targets> tlist = t.Read(studentId, year);
            return tlist;
        }

        [HttpDelete]
        [Route("api/Targets")]
        public List<Targets> Delete([FromBody]Targets t)
        {
            return t.Delete();
        }

        [HttpPost]
        [Route("api/Targets")]
        public List<Targets> Post([FromBody]Targets t)
        {
            return t.Insert();
        }

        [HttpPost]
        [Route("api/Targets/{target}/{year}/{studentId}")]
        public List<Targets> Post(string target, int area, int sarea, int year, string studentId)
        {
            Targets t = new Targets();
            return t.Insert(target, area, sarea, year, studentId);
        }

        [HttpDelete]
        [Route("api/Targets/{Tindex}")]
        public List<Targets> Delete(int Tindex)
        {
            Targets t = new Targets();
            return t.Delete(Tindex);
        }

        [HttpPut]
        [Route("api/Targets")]
        public List<Targets> Put([FromBody]Targets t)
        {
            return t.Update();
        }

        [HttpPut]
        [Route("api/Targets/{Tindex}/{phrase}")]
        public List<Targets> Put(int Tindex, string phrase)
        {
            Targets t = new Targets();
            return t.Update(Tindex, phrase);
        }
    }
}