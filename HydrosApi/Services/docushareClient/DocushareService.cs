using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
 
namespace HydrosApi.Services.docushareClient
{
    public class DocushareService
    {
        private HttpClient client;

        public DocushareService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://dwrsrvc.azwater.gov/dsapi/api/");
        }
        public List<SOCDOC>GetSocDocs(string pcc)
        {
            //call DSAPI
            var result = this.client.GetAsync("soc/getsocdocuments?fileNumber=" + pcc).Result;
            List<SOCDOC> socDocs = new List<SOCDOC>();
          
            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                if(content.Contains("No records found"))
                {
                    socDocs.FirstOrDefault().Status= $"No records found for "+pcc;
                    return socDocs;
                }
                socDocs = JsonConvert.DeserializeObject<List<SOCDOC>>(content);
               
                return socDocs;
                //var firstDoc = socDocs.FirstOrDefault();
                //return firstDoc.FileUrl;
                //return result;
            }

            socDocs.FirstOrDefault().Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
            return socDocs;
        }

        public List<SWDOC> getSurfaceWaterDocs(string pcc)
        {
            //call DSAPI

            List<SWDOC> swDoc = new List<SWDOC>();
            var result = this.client.GetAsync("surfacewater/Getswdocuments?pc=" + pcc).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;

                if (content.Contains("No records found"))
                {
                    swDoc.FirstOrDefault().Status= $"No records found for " + pcc;
                    return swDoc;
                }
                
                swDoc=JsonConvert.DeserializeObject<List<SWDOC>>(content);
                return swDoc;
            }

            //swDoc.FirstOrDefault().Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
            var swStatus = new SWDOC();
            swStatus.Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
            swDoc.Add(swStatus);
            return swDoc;

        }

        public WELLDOC getWellDocs(string pcc)
        {
            WELLDOC wellDoc = new WELLDOC();
            //call DSAPI
            var result = this.client.GetAsync("wellregdoc/get?regid=" + pcc).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                wellDoc = JsonConvert.DeserializeObject<WELLDOC>(content);      
                return wellDoc;
            }

            wellDoc.Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
             
            return wellDoc;

        }
    }
}