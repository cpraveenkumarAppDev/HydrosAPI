using HydrosApi.Services.docushareClient;

namespace AdwrApi.Services.docushareClient
{
    public class SOCDOC : IDSAPIdoc
    {
        public string Handle { get; set; }
        public string DocType { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ObjSummary { get; set; }
        public string FileIdentifier { get; set; }
    }
}