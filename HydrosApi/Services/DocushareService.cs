using HydrosApi.Models.Docushare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace HydrosApi.Services
{
    public class DocushareService
    {
        private HttpClient client;

        public DocushareService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("http://dwrsrvc.azwater.gov/dsapi/api/");
        }
        public string getSocDoc(string pcc)
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
    }
}