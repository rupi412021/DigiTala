using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class ChararcteristicsController : ApiController
    {

        [HttpGet]
        [Route("api/Chararcteristics")]
        public List<Chararcteristics> Get()
        {
            Chararcteristics c = new Chararcteristics();
            List<Chararcteristics> clist = c.Read();
            return clist;
        }

        [HttpGet]
        [Route("api/Chararcteristics/{studentID}/{year}")]
        public List<Chararcteristics> Get(string studentID, int year)
        {
            Chararcteristics c = new Chararcteristics();
            List<Chararcteristics> clist = c.Read(studentID, year);
            return clist;
        }

        [HttpPost]
        [Route("api/Chararcteristics")]
        public RecommendedTargets Post([FromBody]RecommendedTargets rt)
        {
            return rt.GetRecommendedTargets();
        }

        [HttpPut]
        [Route("api/Chararcteristics")]
        public void Put([FromBody]RecommendedTargets rt)
        {
            rt.Update();
        }


        [HttpDelete]
        [Route("api/Chararcteristics")]
        public void Delete([FromBody]RecommendedTargets rt)
        {
            rt.Delete();
        }

    }
}