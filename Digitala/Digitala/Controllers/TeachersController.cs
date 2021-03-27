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

        [HttpPost]
        [Route("api/Teachers/tEmail")]
        public string RetrivePass(string email)
        {
            Teachers t = new Teachers();
            string uMail = t.sendEmail(email);
            return uMail;
        }

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

    }
}