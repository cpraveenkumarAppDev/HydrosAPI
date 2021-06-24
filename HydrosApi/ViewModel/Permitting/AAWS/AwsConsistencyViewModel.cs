namespace HydrosApi.ViewModel.Permitting.AAWS
{

    using System.Collections.Generic;
    using System.Linq;
    using Models.Permitting.AAWS;
    using Models.ADWR;
    using System;

    public class AwsConsistencyViewModel
    {
        public VAwsActiveManagementArea AmaConsistent { get; set; }
        public Dictionary<string, object> AmaGeneralInfo { get; set; }

        public AwsConsistencyViewModel()
        {
        }

        public AwsConsistencyViewModel(int id)
        {
            //AmaDemand = context.V_AWS_AMA.Where(x => x.WaterRightFacilityId == id).FirstOrDefault();
             
            var amaConsistent = VAwsActiveManagementArea.Get(x => x.WaterRightFacilityId == id);
            var amaGeneralInfo = VAwsGeneralInfo.Get(x => x.WaterRightFacilityId == id);

            var d = new Dictionary<string, object>();

            d.Add("MemberCAGRD", amaGeneralInfo.MemberCAGRD);
            d.Add("DateCAGRD", amaGeneralInfo.DateCAGRD);



            AmaConsistent = amaConsistent;
            AmaGeneralInfo = d;
            
        }

        public AwsConsistencyViewModel(int id, AwsConsistencyViewModel data)
        {
            var amaConsistent=new VAwsActiveManagementArea();

            if (data.AmaConsistent != null)
            { 
                amaConsistent = VAwsActiveManagementArea.Update(data.AmaConsistent);
                AmaConsistent = amaConsistent;
            }

            if(data.AmaGeneralInfo != null)
            {
                var gen = VAwsGeneralInfo.Get(g => g.WaterRightFacilityId == id);
                gen.MemberCAGRD = data.AmaGeneralInfo["MemberCAGRD"].ToString();
                gen.DateCAGRD = DateTime.Parse(data.AmaGeneralInfo["DateCAGRD"].ToString());

                VAwsGeneralInfo.Update(gen);
                AmaGeneralInfo = data.AmaGeneralInfo;


            }

        }



    }
}