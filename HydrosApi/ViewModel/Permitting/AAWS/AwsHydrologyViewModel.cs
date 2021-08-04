namespace HydrosApi.ViewModel.Permitting.AAWS
{

    using System.Collections.Generic;
    using System.Linq;
    using Models.Permitting.AAWS;    

    public class AwsHydrologyViewModel
    {

        public VAwsHydro Hydrology { get; set; }
        public List<VAwsWellServing> WellServing { get; set; }
        

        public AwsHydrologyViewModel()
        {
        }

        public AwsHydrologyViewModel(int id)
        {
            var hydro = VAwsHydro.Get(h => h.WaterRightFacilityId == id);
            var wellServing = VAwsWellServing.GetList(x => x.WaterRightFacilityId == id).OrderBy(x=>x.WellRegistryId).ToList();
             

            Hydrology = hydro;
            WellServing = wellServing;

        }

        
    }


}