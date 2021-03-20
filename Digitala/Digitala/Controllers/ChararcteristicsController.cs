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
        [Route("api/Chararcteristics")]
        public List<Chararcteristics> Get()
        {
            Chararcteristics c = new Chararcteristics();
            List<Chararcteristics> clist = c.Read();
            return clist;
        }

        [HttpPost]
        [Route("api/Chararcteristics/{studentId}/{year}/{chars}")]
        public void Post(string studentId, string year, string[] chars)
        {
            Chararcteristics c = new Chararcteristics();
            c.PostCharsToStudent(studentId, year, chars);            
        }

    }
}