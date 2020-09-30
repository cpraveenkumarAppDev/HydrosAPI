namespace HydrosApi.ViewModel 
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
    using Data;
    using Newtonsoft.Json;

    public class SP_AW_INS:Repository<SP_AW_INS> //for SP_AW_INS_FILE, SP_AW_INS_CONV procedures
    {
        [JsonProperty(PropertyName = "programNumber")]
        public string p_program_code { get; set; }

        [JsonProperty(PropertyName = "ama")]
        public string p_ama_code { get; set; }

        public string p_exist_filenum { get; set; } //not used for SP_AW_INS_FILE

        [JsonProperty(PropertyName = "filemanagerEmail")]
        public string p_file_reviewer { get; set; }

        public string p_createby { get; set; }
        public string p_new_filenum { get; set; }
        public string p_new_wrf_id { get; set; }
        //7220503

    }

}