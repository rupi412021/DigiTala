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
        public List<Custodian> Post([FromBody] Custodian c)
        {
            return c.Insert();
        }
    }
}