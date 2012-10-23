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
    public class AssetsController : ApiController
    {
        // GET api/values
        public IEnumerable<PIAsset> Get(double longitude, double latitude, double radius)
        {
            var j = PIService.GetAssetsByLocation(longitude, latitude, radius);
            return j;
        }
    }
}
