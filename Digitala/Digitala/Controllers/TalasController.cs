using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class TalasController : ApiController
    {
        [HttpGet]
        [Route("api/Talas/{studentId}/{year}")]
        public Talas Get(string studentId, int year)
        {
            Talas t = new Talas();
            return t.Read(studentId, year);
        }

        [HttpPost]
        [Route("api/Talas")]
        public Talas Post([FromBody]Talas t)
        {
            return t.Insert();
        }
    }
}