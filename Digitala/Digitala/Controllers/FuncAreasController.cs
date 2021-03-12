using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class FuncAreasController : ApiController
    {
        [HttpGet]
        [Route("api/FuncAreas")]
        public List<FuncAreas> Get()
        {
            FuncAreas t = new FuncAreas();
            List<FuncAreas> tlist = t.Read();
            return tlist;
        }

        [HttpPut]
        [Route("api/FuncAreas")]
        public List<FuncAreas> Put([FromBody]FuncAreas f)
        {
            return f.Update();
        }

    }
}