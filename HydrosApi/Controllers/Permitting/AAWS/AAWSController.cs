
namespace HydrosApi
{ 
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Models;
 

    public class AAWSController : ApiController
    {
        private ADWRContext db = new ADWRContext();
        // GET: AAWS
        //IRR-29-A16011018CBB-01
        //[System.Web.Http.Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("aws/getgeneralInfo")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {
            var data = db.V_AWS_GENERAL_INFO.ToListAsync();
            return Ok(data);
        }

        [Route("aws/getgeneralInfoById/{id}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfoById(string id)
        {
            var pcc = id.Replace("~",".");
            List<V_AWS_GENERAL_INFO> data = null;
            using (ADWRContext db = new ADWRContext())
            {
                data=db.V_AWS_GENERAL_INFO.Where(p => p.ProgramCertificateConveyance == pcc).ToList();
                var overView = new AWS_OVER_VIEW();
                data.FirstOrDefault().OverView = overView;
            }
                return Ok(data);
        }
    }
}