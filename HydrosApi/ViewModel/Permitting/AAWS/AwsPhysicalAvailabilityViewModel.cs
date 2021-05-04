namespace HydrosApi.ViewModel.Permitting.AAWS
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.Permitting.AAWS;
    using Data;


    public class AwsPhysicalAvailabilityViewModel
    {


        /// <summary>
        /// Basis of Physical Availability (on the hydros physical availability tag)
        /// </summary>
        public List<VAwsWrfWrfDemand> Basis { get; set; }
        VAwsActiveManagementArea AmaDemand { get; set; }

        public AwsPhysicalAvailabilityViewModel()
        {
        }
        /// <summary>
        /// For the PhysicalAvailability tab
        /// </summary>
        /// <param name="id">Provide an SQL Statement (ideally you should only use this for select statements) </param>
        /// <returns>Returns the WaterDemand values for Basis of Physical Availability Source and Use</returns>
        /// <remarks>
        /// <para>
        /// Physical Availability, Basis of Physical Availability and Well Serving? 
        /// </para>      
        /// </remarks>
        public AwsPhysicalAvailabilityViewModel(int id)
        {           
            using (var context = new OracleContext())
            {
                //AmaDemand = context.V_AWS_AMA.Where(x => x.WaterRightFacilityId == id).FirstOrDefault();
                
                var basis = context.VAwsWrfWrfDemand.Where(d => d.WaterRightFacilityId == id).Distinct().OrderByDescending(x => x.WaterDemand);
                Basis = basis.ToList();
            }
        }
    }
}