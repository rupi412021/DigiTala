using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class CustodianController : ApiController
    {
        [HttpPost]
        [Route("api/Custodian")]
        public void Post([FromBody] Custodian c)
        {
            c.Insert();
        }

        [HttpGet]
        [Route("api/Custodian/{studentId}")]
        public Custodian Get(string studentId)
        {
            Custodian cust = new Custodian();
            return cust.Read(studentId);
        }

        [HttpPut]
        [Route("api/Custodian")]
        public void Put([FromBody]Custodian c)
        {
            c.Update();
        }
    }
}