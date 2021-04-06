namespace HydrosApi.Models
{ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    public class AWSNewAppViewModel
    {
        public List<VCdAwAppFeeRates> ApplicationTypes { get; set; }
        public AwVerifiedOaws ApplicationComplete { get; set; }


        public VCdAwAmaIna Ama { get; set; }
        public AWSNewAppViewModel()
        {

            this.ApplicationTypes = VCdAwAppFeeRates.GetAll();
            this.Ama = VCdAwAmaIna.GetAll().FirstOrDefault();

        }
       
    }

    //internal class NewApplication
    //{
    //    public string APP_COMPLETE { get; set; }
    //    public string APP_ACCEPTED { get; set; }


    //}
}