using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class TargetsSurveyController : ApiController
    {
        [HttpGet]
        [Route("api/TargetsSurvey/{teacherId}")]
        public List<TargetsSurvey> Get(string teacherId)
        {
            TargetsSurvey t = new TargetsSurvey();
            List<TargetsSurvey> tlist = t.Read(teacherId);
            return tlist;
        }

        //[HttpDelete]
        //[Route("api/TargetsSurvey")]
        //public void Delete([FromBody]TargetsSurvey t)
        //{
        //    t.Delete();
        //}

        [HttpPost]
        [Route("api/TargetsSurvey")]
        public void Post([FromBody]TargetsSurvey t)
        {
            t.Insert();
        }

        //[HttpPut]
        //[Route("api/TargetsSurvey")]
        //public void Put([FromBody]TargetsSurvey t)
        //{
        //    t.Update();
        //}
    }
}