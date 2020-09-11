using AdwrApi.Models.Docushare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AdwrApi.Services
{
    public class DocushareService
    {
        private HttpClient client;

        public DocushareService()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri("dwrsrvc.azwater.gov/dsapi/api/");
        }
        public async Task<string> getSocDoc(string pcc)
        {
            //call DSAPI
            var result = await this.client.GetAsync("soc/getsocdocuments?fileNumber=" + pcc);

            if (result.IsSuccessStatusCode)
            {
                //send json object back
                var content = await result.Content.ReadAsStringAsync();
                var socDocs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SOCDOC>>(content);
                var firstDoc = socDocs.FirstOrDefault();
                return firstDoc.FileUrl;
            }

            return $"There was a non success code sent back from the DSAPI: {result.StatusCode}";
        }
    }
}