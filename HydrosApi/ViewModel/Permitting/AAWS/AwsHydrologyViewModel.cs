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

        public List<AwWrfWrfDemand> Demand { get; set; }
        public VAwsHydro Hydro { get; set; }       
        public decimal? TotalDemandSource { get; set; }
        public decimal? TotalDemandUse { get; set; }
        public decimal? RemainingAvailability { get; set; }

        public AwsHydrologyViewModel()
        {
        }

        public AwsHydrologyViewModel(string pcc)
        {
            this.Hydro = VAwsHydro.Get(h => h.PCC == pcc);
            PopulateHydroModel();
            
        }

        public AwsHydrologyViewModel(int wrf)
        {

            this.Hydro = VAwsHydro.Get(h => h.WaterRightFacilityId == wrf);
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

            var wrf = Hydro.WaterRightFacilityId;

           
            var demandSource = AwWrfWrfDemand.GetList(d => d.WaterRightFacilityIdTo == wrf);
            var demandUse = AwWrfWrfDemand.GetList(d => d.WaterRightFacilityIdFrom==wrf);           

            var demand = demandSource != null && demandUse != null ? demandSource.Union(demandUse).ToList() : demandUse != null ? demandUse : demandSource != null ? demandSource : null;
            
            if(demand != null)
            { 

                foreach (var d in demand)
                {
                    var matchId = wrf == d.WaterRightFacilityIdFrom ? d.WaterRightFacilityIdTo : d.WaterRightFacilityIdFrom;
                    d.AvailabilityType = d.WaterRightFacilityIdTo == wrf ? "Source" : d.WaterDemand==null ? "Source" : "Use";
                    d.WaterDemand=(d.WaterDemand ?? 0) * (d.AvailabilityType=="Use" ?  -1 : 1);
                
                    TotalDemandSource+= (d.AvailabilityType == "Source" ? d.WaterDemand : 0);
                
                    TotalDemandUse += (d.AvailabilityType == "Use" ? d.WaterDemand : 0);
                    d.AssociatedPCC = d.WaterRightFacilityIdTo == d.WaterRightFacilityIdFrom ? this.Hydro.PCC : WaterRightFacility.Get(w => w.Id == matchId).PCC;
                }

                RemainingAvailability = TotalDemandSource + TotalDemandUse;
                Demand = demand;
            }
        }

        
    }
}