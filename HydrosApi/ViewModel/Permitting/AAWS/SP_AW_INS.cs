namespace HydrosApi.ViewModel 
{
    using System;
    using Data;
    using Newtonsoft.Json;
     
    using Oracle.ManagedDataAccess.Client;
   
    using System.Collections.Generic;
    using System.Data;
    using System.ComponentModel;
    using System.Web;
    using Models;
    using System.Threading.Tasks;

    public class SP_AW_INS:Repository<SP_AW_INS> //for SP_AW_INS_FILE, SP_AW_INS_CONV procedures
    {        
        public string p_program_code { get; set; }
         
        public string p_ama_code { get; set; }
        
        public string p_exist_filenum { get; set; } //not used for SP_AW_INS_FILE
       
        public string p_file_reviewer { get; set; }       
        
        public string p_createby { get; set; }
         
        public string p_new_filenum { get; set; }
        public int? p_new_wrf_id { get; set; }

        [JsonProperty(PropertyName = "programNumber")]
        public string programNumber { set { p_program_code = value; } }

        [JsonProperty(PropertyName = "filemanagerEmail")]
        public string filemanagerEmail { set { p_file_reviewer = value; } }

        [JsonProperty(PropertyName = "ama")]
        public string ama { set
            {
                try
                {
                    var amaCode = V_CD_AW_AMA_INA.Get(a => a.CODE == value.ToUpper() || a.DESCR == value.ToUpper().Replace(" AMA", "") + " AMA");

                    if(amaCode==null)
                    {
                       p_ama_code="0";
                    }
                    else
                    {
                        p_ama_code=amaCode.CODE;
                    }
                }
                catch
                {
                    p_ama_code = "0";
                }
            }                
        }

        public static async Task<V_AWS_GENERAL_INFO> CreateNewFile(SP_AW_INS paramValues, string requestType)
        {
            var parameter = new List<OracleParameter>();
            var result = new SP_AW_INS();
            var user = HttpContext.Current.User.Identity.Name;
            string command = "";
            var generalInfo=new V_AWS_GENERAL_INFO();

            try { 

                parameter.Add(new OracleParameter("p_program_code", paramValues.p_program_code));
                parameter.Add(new OracleParameter("p_ama_code", paramValues.p_ama_code));
                if(requestType=="conveyance")
                { 
                    parameter.Add(new OracleParameter("p_exist_filenum", paramValues.p_exist_filenum));
                }

                parameter.Add(new OracleParameter("p_file_reviewer", paramValues.p_file_reviewer.ToLower().Replace("@azwater.gov", "")));
                parameter.Add(new OracleParameter("p_createby", user.Replace("AZWATER0\\", "")));

                var newFileNum = new OracleParameter("p_new_filenum", OracleDbType.Varchar2, 20);
                newFileNum.IsNullable = true;
                newFileNum.Direction = ParameterDirection.ReturnValue;
                parameter.Add(newFileNum);

                var newWrfId = new OracleParameter("p_new_wrf_id", OracleDbType.Decimal);
                newWrfId.Direction = ParameterDirection.ReturnValue;
                newWrfId.IsNullable = true;
                parameter.Add(newWrfId);

                if(requestType=="conveyance")
                {
                    command = "BEGIN aws.aw_spkg_hydros_insert.ins_conv(:p_program_code, :p_ama_code, :p_exist_filenum, :p_file_reviewer, :p_createby, :p_new_filenum, :p_new_wrf_id); end;";
                }
                else if(requestType=="newApplication")
                {
                    command = "BEGIN aws.aw_spkg_hydros_insert.ins_file(:p_program_code, :p_ama_code, :p_file_reviewer, :p_createby, :p_new_filenum, :p_new_wrf_id); end;";
                }
                else
                {
                        generalInfo.ProcessStatus = "A valid request type defining the the correct stored procedure was not submitted";
                        return generalInfo;
                }

                var newFile = await Task.FromResult(SP_AW_INS.ExecuteStoredProcedure(command, parameter.ToArray()));

                foreach (var p in parameter)
                {
                    var property = result.GetType().GetProperty(p.ParameterName);
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    var value = converter.ConvertFrom(p.Value.ToString());
                    property.SetValue(result, value);
                }

                return await Task.FromResult(V_AWS_GENERAL_INFO.Get(i => i.WaterRightFacilityId == result.p_new_wrf_id));
            }
             
            catch (Exception exception)
            {
                var wrapExceptionMessage = new { message = exception.Message };

                generalInfo.ProcessStatus = wrapExceptionMessage.ToString();
                return generalInfo;
                 
            }
         
        }
    }   
}