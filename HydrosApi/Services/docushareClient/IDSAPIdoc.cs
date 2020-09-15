using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.Services.docushareClient
{
    public interface IDSAPIdoc
    {
        string Handle { get; set; }
        string DocType { get; set; }
        string FileName { get; set; }
        string FileUrl { get; set; }
        string ObjSummary { get; set; }
        string FileIdentifier { get; set; }
    }
}