
namespace HydrosApi.Controllers
{
    using System;
    using System.Web.Http;    
    using System.Threading.Tasks;
    using Models;
    using HydrosApi.ViewModel;    
    using System.Text.RegularExpressions;

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
            //this will format any pcc as long as the pattern is two numbers, six numbers, four numbers in it
            //so it can be all numbers or have characters as long as the character are in the correct locations
            //I'm sorry to change it
            
            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            var pcc = regex.Replace(id, "$1-$2.$3"); 
           // var pcc = regex.Replace("~", ".");

            return Json(AAWSProgramInfoViewModel.GetData(pcc));
        }       
       
        [Route("aws/GetNewAWSRight")]
        [HttpGet]
        public IHttpActionResult GetNewApplicationCredentials()
        {
            return Json(new AWSNewAppViewModel());
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newconv")]
        public async Task<IHttpActionResult> AddNewConveyance([FromBody] SP_AW_INS paramValues) //New conveyance
        {                 
            var conveyance = await Task.FromResult(SP_AW_INS.CreateNewFile(paramValues, "conveyance"));

            if(conveyance==null)
            {
                return Ok(new { message = "A new conveyance was not created" });
               
            }
            else if(conveyance.Result.ProcessStatus != null)
            {
                return Ok(new { message = conveyance.Result.ProcessStatus });               
            }
             
            return Ok(conveyance);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newapp")]
        public async Task<IHttpActionResult> AddNewApplication([FromBody] SP_AW_INS paramValues) //New file
        {            
            var application=await Task.FromResult(SP_AW_INS.CreateNewFile(paramValues, "newApplication"));

            if (application == null)
            {
                return Ok(new { message = "A new application was not created" });
            }
            else if (application.Result.ProcessStatus != null)
            {
                return Ok(new { message = application.Result.ProcessStatus });
            }

            return Ok(application);
             
        }
    }
}