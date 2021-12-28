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
            List<SOCDOC> socDocs = new List<SOCDOC>();
            var socMsg = new SOCDOC();

            if (pcc==null)
            {
                socMsg.Status= "PCC submitted was empty";
                socDocs.Add(socMsg);
                return socDocs;

            }
            //call DSAPI
            var path = string.Format("soc/getsocdocuments?fileNumber={0}", pcc);

            var result = this.client.GetAsync(path).Result;
            
          
            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                if(content.Contains("No records found"))
                {
                    socMsg.Status = $"No records found for "+pcc;
                    socDocs.Add(socMsg);
                    return socDocs;
                }
                socDocs = JsonConvert.DeserializeObject<List<SOCDOC>>(content);
               
                return socDocs;
                //var firstDoc = socDocs.FirstOrDefault();
                //return firstDoc.FileUrl;
                //return result;
            }

            socMsg.Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
            socDocs.Add(socMsg);
            return socDocs;
            
        }

        public List<SWDOC> getSurfaceWaterDocs(string pcc)
        {
            //call DSAPI

            List<SWDOC> swDoc = new List<SWDOC>();
            var swMsg = new SWDOC();
            var path = string.Format("surfacewater/Getswdocuments?pc={0}", pcc);
            var result = this.client.GetAsync(path).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;

                if (content.Contains("No records found"))
                {
                    swMsg.Status= $"No records found for " + pcc;
                    swDoc.Add(swMsg);
                    return swDoc;
                }
                
                swDoc=JsonConvert.DeserializeObject<List<SWDOC>>(content);
                return swDoc;
            }

            //swDoc.FirstOrDefault().Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
             
            swMsg.Status = $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
            swDoc.Add(swMsg);
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