
namespace HydrosApi
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    using Models;

    public class AAWSController : ApiController
    {

        private ADWRContext db = new ADWRContext();
        // GET: AAWS
        //IRR-29-A16011018CBB-01
        [System.Web.Http.Route("aws/getgeneralInfo")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {
            return Ok(V_AWS_GENERAL_INFO.GetAll());
        }
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [System.Web.Http.Route("aws/getgeneralInfoById/{id}")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetGeneralInfoById(string id)
        {
            var pcc = id.Replace("~", ".");
            return Json(AAWSProgramInfoViewModel.GetData(pcc));
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [System.Web.Http.Route("aws/GetNewApplicationCredentials")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetNewApplicationCredentials()
        {

            return Json(AWSNewAppViewModel.GetNewAppData());
        }
    }
}