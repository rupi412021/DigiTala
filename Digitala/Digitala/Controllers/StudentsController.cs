using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class StudentsController : ApiController
    {
        [HttpGet]
        [Route("api/Students")]
        public List<Students> Get()
        {
            Students s = new Students();
            List<Students> slist = s.Read();
            return slist;
        }  

        [HttpGet]
        [Route("api/Students/{teacherId}/{year}/{classN}")]
        public List<Students> Get(string teacherId, int year, string classN)
        {
            Students s = new Students();
            List<Students> slist = s.ReadStudentsForTeacherPerYear(teacherId, year, classN);
            return slist;
        }

        [HttpPost]
        [Route("api/Students")]
        public List<Students> Post([FromBody]Students s)
        {
            return s.Insert();
        }

        [HttpPut]
        [Route("api/Students")]
        public List<Students> Put([FromBody]Students s)
        {
            return s.Update();
        }

        [HttpPost]
        [Route("api/Students/{sschool}/{sclass}/{syear}/{sId}")]
        public void InsertStudentInClass(int sschool, string sclass, int syear, int sId)
        {
            Students s = new Students();
            s.UpdateClass(sschool, sclass, syear, sId);
        }

        [HttpDelete]
        [Route("api/Students/{id}")]
        public void Delete(int id)
        {
            Students s = new Students();
            s.Delete(id);
        }

    }
}