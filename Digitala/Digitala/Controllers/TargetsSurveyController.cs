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
        [Route("api/TargetsSurvey")]
        public List<TargetsSurvey> Get()
        {
            TargetsSurvey t = new TargetsSurvey();
            List<TargetsSurvey> tlist = t.Read();
            return tlist;
        }

        [HttpDelete]
        [Route("api/TargetsSurvey")]
        public List<TargetsSurvey> Delete([FromBody]TargetsSurvey t)
        {
            return t.Delete();
        }

        [HttpPost]
        [Route("api/TargetsSurvey")]
        public List<TargetsSurvey> Post([FromBody]TargetsSurvey t)
        {
            return t.Insert();
        }

        [HttpPut]
        [Route("api/TargetsSurvey")]
        public List<TargetsSurvey> Put([FromBody]TargetsSurvey t)
        {
            return t.Update();
        }
    }
}