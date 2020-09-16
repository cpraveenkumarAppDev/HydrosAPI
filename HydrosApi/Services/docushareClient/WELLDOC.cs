//using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace HydrosApi.Services.docushareClient
{
    public class WELLDOC : IDSAPIdoc
    {
        public string Handle { get; set; }
        public string DocType { get; set; }
               
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "docUrl")]
        public string FileUrl { get; set; }

        [JsonProperty(PropertyName = "Location")]
        
        public string ObjSummary { get; set; }

        [JsonProperty(PropertyName = "RegistryId")]
        public string FileIdentifier { get; set; }

        public string Status { get; set; }
    }
}