namespace HydrosApi.ViewModel.Permitting.AAWS
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HydrosApi.Models.Permitting.AAWS;
    using Models.ADWR;



    public class AwsHydrologyViewModel
    {

        public List<AW_WRF_WRF_DEMAND> Demand { get; set; }
        public V_AWS_HYDRO Hydro { get; set; }       
        public decimal? TotalDemandSource { get; set; }
        public decimal? TotalDemandUse { get; set; }
        public decimal? RemainingAvailability { get; set; }

        public AwsHydrologyViewModel()
        {
        }

        public AwsHydrologyViewModel(string pcc)
        {
            this.Hydro = V_AWS_HYDRO.Get(h => h.PCC == pcc);
            PopulateHydroModel();
            
        }

        public AwsHydrologyViewModel(int wrf)
        {

            this.Hydro = V_AWS_HYDRO.Get(h => h.WRFID == wrf);
            PopulateHydroModel();
            
        }


        public void PopulateHydroModel()
        {
            if(Hydro == null)
            {
                return;
            }

            TotalDemandSource = 0;
            TotalDemandUse = 0;

            var wrf = Hydro.WRFID;

           
            var demandSource = AW_WRF_WRF_DEMAND.GetList(d => d.WRF_ID_TO == wrf);
            var demandUse = AW_WRF_WRF_DEMAND.GetList(d => d.WRF_ID_FROM==wrf);           

            var demand = demandSource != null && demandUse != null ? demandSource.Union(demandUse).ToList() : demandUse != null ? demandUse : demandSource != null ? demandSource : null;
            
            if(demand != null)
            { 

                foreach (var d in demand)
                {
                    var matchId = wrf == d.WRF_ID_FROM ? d.WRF_ID_TO : d.WRF_ID_FROM;
                    d.AVAILABILITY_TYPE = d.WRF_ID_TO == wrf ? "Source" : d.WTR_DEMAND==null ? "Source" : "Use";
                    d.WTR_DEMAND=(d.WTR_DEMAND ?? 0) * (d.AVAILABILITY_TYPE=="Use" ?  -1 : 1);
                
                    TotalDemandSource+= (d.AVAILABILITY_TYPE == "Source" ? d.WTR_DEMAND : 0);
                
                    TotalDemandUse += (d.AVAILABILITY_TYPE == "Use" ? d.WTR_DEMAND : 0);
                    d.ASSOCIATED_PCC = d.WRF_ID_TO == d.WRF_ID_FROM ? this.Hydro.PCC : WTR_RIGHT_FACILITY.Get(w => w.ID == matchId).PCC;
                }

                RemainingAvailability = TotalDemandSource + TotalDemandUse;
                Demand = demand;
            }
        }

        
    }
}