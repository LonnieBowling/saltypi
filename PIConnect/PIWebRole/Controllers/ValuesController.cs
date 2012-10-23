using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PIWebRole.Services;
using PIConnect2.Services;

namespace PIWebRole.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<PIValue> Get(string id, int range)
        {
            var j =  PIService.GetPlotValues(id, DateTime.Now.AddHours(range), DateTime.Now, 100);
            return j;
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //public void Post(string value)
        //{
        //}

        //// PUT api/values/5
        //public void Put(int id, string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}