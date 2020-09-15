using AdwrApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

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
        public string GetSocDocs(string pcc)
        {
            //call DSAPI
            var result = this.client.GetAsync("soc/getsocdocuments?fileNumber=" + pcc).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                var socDocs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SOCDOC>>(content);
                var firstDoc = socDocs.FirstOrDefault();
                return firstDoc.FileUrl;
            }

            return $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
        }

        public string getSurfaceWaterDocs(string pcc)
        {
            //call DSAPI
            var result = this.client.GetAsync("surfacewater/Getswdocuments?pc=" + pcc).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                var socDocs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SWDOC>>(content);
                var firstDoc = socDocs.FirstOrDefault();
                return firstDoc.FileUrl;
            }

            return $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
        }

        public string getWellDocs(string pcc)
        {
            //call DSAPI
            var result = this.client.GetAsync("wellregdoc/get?regid=" + pcc).Result;

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = result.Content.ReadAsStringAsync().Result;
                var socDocs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<WELLDOC>>(content);
                var firstDoc = socDocs.FirstOrDefault();
                return firstDoc.FileUrl;
            }

            return $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
        }
    }
}