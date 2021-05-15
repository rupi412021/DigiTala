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
            string uPass = t.sendEmail(email);
            return uPass;
        }

        [HttpPut]
        [Route("api/Teachers/{passWord}/{email}")]
        public void ChangePass(string passWord, string email)
        {
            Teachers t = new Teachers();
            t.setTpass(passWord, email);
        }

        [HttpPut]
        [Route("api/Teachers/{tschool}/{tclass}/{tyear}/{temail}")]
        public void ChangeClassInfo(int tschool, string tclass, int tyear, string temail)
        {
            Teachers t = new Teachers();
            t.updateClass(tschool, tclass, tyear, temail); ;
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