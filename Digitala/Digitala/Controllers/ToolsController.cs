using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class ToolsController : ApiController
    {
        [HttpGet]
        [Route("api/Tools/{sid}/{syear}")]
        public List<Tools> Get(int sid, int syear)
        {
            Tools t = new Tools();
            List<Tools> tlist = t.Read(sid, syear);
            return tlist;
        }

        [HttpDelete]
        [Route("api/Tools")]
        public List<Tools> Delete([FromBody]Tools t)
        {
            return t.Delete();
        }

        [HttpPost]
        [Route("api/Tools")]
        public List<Tools> Post([FromBody]Tools t)
        {
            return t.Insert();
        }
    }
}