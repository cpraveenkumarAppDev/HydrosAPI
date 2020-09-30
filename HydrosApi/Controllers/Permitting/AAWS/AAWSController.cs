
namespace HydrosApi
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using System.Threading.Tasks;

    using Models;
    using HydrosApi.ViewModel;
    using Oracle.ManagedDataAccess.Client;

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

       
       
        [Route("aws/GetNewAWSRight")]
        [HttpGet]
        public IHttpActionResult GetNewApplicationCredentials()
        {

            return Json(new AWSNewAppViewModel());
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newconv")]
        public async Task<IHttpActionResult> AddNewConveyance([FromBody] SP_AW_INS paramValues) //New conveyance
        {
            var parameter = new List<OracleParameter>();

            parameter.Add(new OracleParameter("p_program_code", paramValues.p_program_code));
            parameter.Add(new OracleParameter("p_ama_code", paramValues.p_ama_code));
            parameter.Add(new OracleParameter("p_exist_filenum", paramValues.p_exist_filenum));
            parameter.Add(new OracleParameter("p_file_reviewer", paramValues.p_file_reviewer.ToLower().Replace("@azwater.gov", "")));
            parameter.Add(new OracleParameter("p_createby", User.Identity.Name.Replace("AZWATER0\\", "")));

            var procedureVals =
                await Task.FromResult(SP_AW_INS.ExecuteStoredProcedure("BEGIN aws.sp_aw_ins_conv(:p_program_code, :p_ama_code, :p_exist_filenum, :p_file_reviewer, :p_createby); end;", parameter.ToArray()));

            return Ok(procedureVals);
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newapp")]
        public async Task<IHttpActionResult> AddNewApplication([FromBody] SP_AW_INS paramValues) //New file
        {
            var parameter = new List<OracleParameter>();

            parameter.Add(new OracleParameter("p_program_code", paramValues.p_program_code));
            parameter.Add(new OracleParameter("p_ama_code", paramValues.p_ama_code));           
            parameter.Add(new OracleParameter("p_file_reviewer", paramValues.p_file_reviewer.ToLower().Replace("@azwater.gov","")));
            parameter.Add(new OracleParameter("p_createby", User.Identity.Name.Replace("AZWATER0\\", "")));

            var procedureVals = 
                await Task.FromResult(SP_AW_INS.ExecuteStoredProcedure("BEGIN aws.sp_aw_ins_file(:p_program_code, :p_ama_code, :p_file_reviewer, :p_createby); end;", parameter.ToArray()));

            return Ok(procedureVals);
        }
    }
}