
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
    using WebApi.OutputCache.V2;
    using System.ComponentModel;
    using Microsoft.Ajax.Utilities;


    public class AAWSController : ApiController
    {
        // GET: AAWS
        //IRR-29-A16011018CBB-01
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
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

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newconv")]
        public async Task<IHttpActionResult> AddNewConveyance([FromBody] SP_AW_INS paramValues) //New conveyance
        {
            var parameter = new List<OracleParameter>();
            var result = new SP_AW_INS();

            parameter.Add(new OracleParameter("p_program_code", paramValues.p_program_code));
            parameter.Add(new OracleParameter("p_ama_code", paramValues.p_ama_code));
            parameter.Add(new OracleParameter("p_exist_filenum)", paramValues.p_exist_filenum));

            parameter.Add(new OracleParameter("p_file_reviewer", paramValues.p_file_reviewer.ToLower().Replace("@azwater.gov", "")));
            parameter.Add(new OracleParameter("p_createby", User.Identity.Name.Replace("AZWATER0\\", "")));

            var newFileNum = new OracleParameter("p_new_filenum", OracleDbType.Varchar2, 20);
            newFileNum.IsNullable = true;
            newFileNum.Direction = ParameterDirection.ReturnValue;
            parameter.Add(newFileNum);

            var newWrfId = new OracleParameter("p_new_wrf_id", OracleDbType.Decimal);
            newWrfId.Direction = ParameterDirection.ReturnValue;
            newWrfId.IsNullable = true;
            parameter.Add(newWrfId);

            var newFile = await Task.FromResult(SP_AW_INS.ExecuteStoredProcedure(
                   "BEGIN aws.sp_aw_ins_file(:p_program_code, :p_ama_code, :p_exist_filenum, :p_file_reviewer, :p_createby, :p_new_filenum, :p_new_wrf_id); end;"
               , parameter.ToArray()));

            foreach (var p in parameter)
            {
                var property = result.GetType().GetProperty(p.ParameterName);
                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                var value = converter.ConvertFrom(p.Value.ToString());
                property.SetValue(result, value);

            }
            return Ok(result);
        }
       
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newapp")]
        public async Task<IHttpActionResult> AddNewApplication([FromBody] SP_AW_INS paramValues) //New file
        {

            var parameter = new List<OracleParameter>();
            var result = new SP_AW_INS(); 
             
            parameter.Add(new OracleParameter("p_program_code", paramValues.p_program_code));
            parameter.Add(new OracleParameter("p_ama_code", paramValues.p_ama_code));  
            
            parameter.Add(new OracleParameter("p_file_reviewer", paramValues.p_file_reviewer.ToLower().Replace("@azwater.gov", "")));
            parameter.Add(new OracleParameter("p_createby", User.Identity.Name.Replace("AZWATER0\\", "")));

            var newFileNum= new OracleParameter("p_new_filenum", OracleDbType.Varchar2,20);
            newFileNum.IsNullable = true;
            newFileNum.Direction = ParameterDirection.ReturnValue;
            parameter.Add(newFileNum);

            var newWrfId= new OracleParameter("p_new_wrf_id", OracleDbType.Decimal);
            newWrfId.Direction = ParameterDirection.ReturnValue;
            newWrfId.IsNullable = true;
            parameter.Add(newWrfId);

            var newFile = await Task.FromResult(SP_AW_INS.ExecuteStoredProcedure(
                   "BEGIN aws.sp_aw_ins_file(:p_program_code, :p_ama_code, :p_file_reviewer, :p_createby, :p_new_filenum, :p_new_wrf_id); end;"
               , parameter.ToArray()));            

            foreach (var p in parameter)
            {
                var property = result.GetType().GetProperty(p.ParameterName);
                var converter = TypeDescriptor.GetConverter(property.PropertyType);                 
                var value= converter.ConvertFrom(p.Value.ToString());
                property.SetValue(result, value);
                    
            }
            return Ok(result);
        }
    }
}