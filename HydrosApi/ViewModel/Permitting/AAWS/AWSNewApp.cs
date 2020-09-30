namespace HydrosApi.Models
{ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    public class AWSNewAppViewModel
    {
        public List<V_CD_AW_APP_FEE_RATES> ApplicationTypes { get; set; }
        public AW_VERIFIED_OAWS ApplicationComplete { get; set; }
 

        public V_CD_AW_AMA_INA Ama { get; set; }
        public AWSNewAppViewModel()
        {

            this.ApplicationTypes = V_CD_AW_APP_FEE_RATES.GetAll();
            this.Ama = V_CD_AW_AMA_INA.GetAll().FirstOrDefault();

        }
       
    }

    //internal class NewApplication
    //{
    //    public string APP_COMPLETE { get; set; }
    //    public string APP_ACCEPTED { get; set; }


    //}
}