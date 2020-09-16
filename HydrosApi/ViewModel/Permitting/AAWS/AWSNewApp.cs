using AdwrApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdwrApi.ViewModel.Permitting.AAWS
{
    public class AWSNewAppViewModel
    {
        public List<V_CS_AW_APP_FEE_RATES> ApplicationTypes { get; set; }
        public AW_VERIFIED_OAWS ApplicationComplete { get; set; }
        public static AWSNewAppViewModel GetNewAppData()
        {
            AWSNewAppViewModel AWSNewAppViewModel = new AWSNewAppViewModel();
            AWSNewAppViewModel.ApplicationTypes = V_CS_AW_APP_FEE_RATES.GetAll();
            return AWSNewAppViewModel;
        }
    }

    //internal class NewApplication
    //{
    //    public string APP_COMPLETE { get; set; }
    //    public string APP_ACCEPTED { get; set; }


    //}
}