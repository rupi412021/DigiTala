using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class PrivilegesController : ApiController
    {
        [HttpGet]
        [Route("api/Privileges")]
        public List<Privileges> Get()
        {
            Privileges p = new Privileges();
            List<Privileges> plist = p.Read();
            return plist;
        }

        [HttpGet]
        [Route("api/Privileges/{studentId}/{year}")]
        public List<Privileges> Get(string studentId, string year)
        {
            Privileges p = new Privileges();
            List<Privileges> plist = p.Read(studentId, year);
            return plist;
        }

        [HttpPost]
        [Route("api/Privileges")]
        public void Post([FromBody]Privileges p)
        {
            p.Insert();
        }

        [HttpDelete]
        [Route("api/Privileges")]
        public void Delete([FromBody]Privileges p)
        {
            p.Delete();
        }

    }
}