using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdwrApi.Services.docushareClient
{
    public class SWDOC : IDSAPIdoc
    {
        public string Handle { get; set; }
        public string DocType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ObjSummary { get; set; }
        public string FileIdentifier { get; set; }
    }
}