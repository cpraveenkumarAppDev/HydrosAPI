using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using HydrosApi.Models;
using System.Text.RegularExpressions;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using HydrosApi.Data;
using System.Runtime.Remoting.Messaging;
using HydrosApi.Models.Permitting.AAWS;

namespace HydrosApi.Controllers.Permitting.AAWS
{
    public class AAWSController : ApiController
    {

        private ADWRContext db = new ADWRContext();
        // GET: AAWS
        //IRR-29-A16011018CBB-01
        //[System.Web.Http.Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [System.Web.Http.Route("aws/getgeneralInfo")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {
            var data = db.V_AWS_GENERAL_INFO.ToListAsync();
            return Ok(data);
        }
    }
}