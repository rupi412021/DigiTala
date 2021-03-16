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
        //[HttpGet]
        //[Route("api/Chararcteristics")]
        //public List<Chararcteristics> Get()
        //{
        //    Chararcteristics t = new Chararcteristics();
        //    List<Chararcteristics> tlist = t.Read();
        //    return tlist;
        //}

        [HttpGet]
        [Route("api/Chararcteristics/{areas}")]
        public List<Chararcteristics> Get(List<int> areas)
        {
            Chararcteristics c = new Chararcteristics();
            List<Chararcteristics> clist = c.Read(areas);
            return clist;
        }

        //[HttpPost]
        //[Route("api/Chararcteristics")]
        //public List<Chararcteristics> Post([FromBody]Chararcteristics f)
        //{
        //    if (f.AreaId == -1)
        //        return f.NewArea(f.Area);
        //    else
        //        return f.NewSubArea(f.AreaId, f.SubArea);
        //}

    }
}