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

        [HttpPost]
        [Route("api/Chararcteristics")]
        public RecommendedTargets Post([FromBody]RecommendedTargets rt)
        {
            return rt.GetRecommendedTargets();
        }

        //[HttpPost]
        //[Route("api/Chararcteristics/{id}")]
        //public void Post(int id)
        //{
        //    Chararcteristics c = new Chararcteristics();
        //    c.TEMP(id);
        //}

    }
}