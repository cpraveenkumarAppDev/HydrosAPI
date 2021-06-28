namespace HydrosApi.ViewModel.ADWR
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Permitting.AAWS;
    using Models.ADWR;
    using System;
    using HydrosApi.Data;
    public class LocationViewModel
    {
        public List<CdAwCity> CdAwCities { get; set; }
        public AwFile AwFile { get; set; }
        public List<Location> Locations { get; set; }

        public LocationViewModel()
        {
        }

        public LocationViewModel(int id)
        {
            var awCity = CdAwCity.GetAll();
            var awFile = AwFile.Get(x => x.WaterRightFacilityId == id);
            var locationList = Location.GetList(x => x.WaterRightFacilityId == id);
            CdAwCities = awCity;
            AwFile = awFile;
            Locations = locationList;

        }

        //public AwsConsistencyViewModel(int id, AwsConsistencyViewModel data)
        //{

        //    if (data.AmaConsistent != null)
        //    {
        //        using (var context = new OracleContext())
        //        {
        //            var consist = VAwsActiveManagementArea.Get(x => x.WaterRightFacilityId == id, context);
        //            var props = typeof(VAwsActiveManagementArea).GetProperties().ToList();

        //            foreach (var prop in props)
        //            {
        //                var newValue = prop.GetValue(data.AmaConsistent);
        //                var oldValue = prop.GetValue(consist);

        //                if (newValue != null && !Object.Equals(newValue, (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : oldValue)))
        //                {
        //                    prop.SetValue(consist, newValue);

        //                }

        //            }
        //            context.SaveChanges();
        //        }
        //        AmaConsistent = data.AmaConsistent;
        //    }

        //    if (data.AmaGeneralInfo != null)
        //    {
        //        VAwsGeneralInfo genInfo;
        //        using (var context = new OracleContext())
        //        {
        //            genInfo = context.V_AWS_GENERAL_INFO.Where(x => x.WaterRightFacilityId == id).FirstOrDefault();
        //            if (data.AmaGeneralInfo.ContainsKey("MemberCAGRD"))
        //            {
        //                genInfo.MemberCAGRD = data.AmaGeneralInfo["MemberCAGRD"].ToString();
        //            }
        //            if (data.AmaGeneralInfo.ContainsKey("DateCAGRD"))
        //            {
        //                genInfo.DateCAGRD = DateTime.Parse(data.AmaGeneralInfo["DateCAGRD"].ToString());
        //            }
        //            context.SaveChanges();
        //        }
        //        //    var gen = VAwsGeneralInfo.Get(g => g.WaterRightFacilityId == id);

        //        //VAwsGeneralInfo.Update(gen);
        //        AmaGeneralInfo = data.AmaGeneralInfo;

        //    }

        //}




    }
}