﻿using Digitala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digitala.Controllers
{
    public class TargetsController : ApiController
    {
        [HttpGet]
        [Route("api/Targets")]
        public List<Targets> Get()
        {
            Targets t = new Targets();
            List<Targets> tlist = t.Read();
            return tlist;
        }

        [HttpGet]
        [Route("api/Targets/{studentId}/{year}")]
        public List<Targets> Get(string studentId, int year)
        {
            Targets t = new Targets();
            List<Targets> tlist = t.Read(studentId, year);
            return tlist;
        }

        [HttpDelete]
        [Route("api/Targets")]
        public List<Targets> Delete([FromBody]Targets t)
        {
            return t.Delete();
        }

        [HttpPost]
        [Route("api/Targets")]
        public List<Targets> Post([FromBody]Targets t)
        {
            return t.Insert();
        }

        [HttpPut]
        [Route("api/Targets")]
        public List<Targets> Put([FromBody]Targets t)
        {
            return t.Update();
        }
    }
}