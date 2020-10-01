namespace HydrosApi.ViewModel 
{
    
    using Data;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using Oracle.ManagedDataAccess.Client;

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
        public string ama { set { p_ama_code = value; } }

         
        //7220503

    }

}