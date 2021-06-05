using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class YearsController : ApiController
    {
        [HttpGet]
        [Route("api/Years")]
        public List<Years> Get()
        {
            Years y = new Years();
            List<Years> ylist = y.Read();
            return ylist;
        }

    }
}