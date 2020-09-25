
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
        // GET: AAWS
        //IRR-29-A16011018CBB-01
        [Route("aws/getgeneralInfo")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {
            return Ok(V_AWS_GENERAL_INFO.GetAll());
        }
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [Route("aws/getgeneralInfoById/{id}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfoById(string id)
        {
            var pcc = id.Replace("~", ".");
            return Json(AAWSProgramInfoViewModel.GetData(pcc));
        }

       
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [Route("aws/GetNewApplicationCredentials")]
        [HttpGet]
        public IHttpActionResult GetNewApplicationCredentials()
        {

            return Json(AWSNewAppViewModel.GetNewAppData());
        }
    }
}