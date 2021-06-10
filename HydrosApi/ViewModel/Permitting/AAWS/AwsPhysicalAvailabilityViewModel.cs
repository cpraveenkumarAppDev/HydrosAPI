namespace HydrosApi.ViewModel.Permitting.AAWS
{
 
    using System.Collections.Generic;
    using System.Linq; 
    using Models.Permitting.AAWS; 
    using Models.ADWR;
    using System;

    public class AwsPhysicalAvailabilityViewModel
    {
        /// <summary>
        /// Basis of Physical Availability (on the hydros physical availability tag)
        /// </summary>
        public List<VAwsWrfWrfDemand> Basis { get; set; }

        public AwAreaOfImpact100 WtrImpact { get; set; }
        public VAwsActiveManagementArea AmaDemand { get; set; }
        public VAwsHydro Hydrology { get; set; }         
        private Dictionary<string,object> ActionStatus { get; set; }
        private string userName { get; set; }
        private DateTime currentDate { get; set; }

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
            //AmaDemand = context.V_AWS_AMA.Where(x => x.WaterRightFacilityId == id).FirstOrDefault();
            var basis = VAwsWrfWrfDemand.GetList(d => d.WaterRightFacilityId == id).Distinct().OrderByDescending(x => x.WaterDemand).ToList();
            var hydro = VAwsHydro.Get(h => h.WaterRightFacilityId == id);               
            var amaDemand=VAwsActiveManagementArea.Get(x => x.WaterRightFacilityId == id);
            var wtrImpact = AwAreaOfImpact100.Get(x => x.WaterRightFacilityId == id);

            Basis = basis;
            Hydrology = hydro;            
            AmaDemand = amaDemand;
            WtrImpact = wtrImpact;
        }                

        public  AwsPhysicalAvailabilityViewModel(int id, AwsPhysicalAvailabilityViewModel awsPhysicalAvailabilityViewModel, string user)
        {           
            var data = awsPhysicalAvailabilityViewModel;

            var hydro = new VAwsHydro();
            var amaDemand = new VAwsActiveManagementArea();
            var impact = new AwAreaOfImpact100();

            userName = user;
            currentDate = DateTime.Now;

            var actionStatus = new Dictionary<string, object>();
            actionStatus.Add("deleteDemand",0);
            actionStatus.Add("deleteWrfWrf",0);
            actionStatus.Add("updateDemand",0);
            actionStatus.Add("insertDemand",0);
            actionStatus.Add("insertWrfWrf",0);

            ActionStatus = actionStatus;

            if (data != null)
            {
                if (data.Basis != null)
                {
                    var currentBasis = VAwsWrfWrfDemand.GetList(d => d.WaterRightFacilityId == id && d.AvailabilityType == "Source").ToList();

                    if (currentBasis == null)
                    {
                        BasisAction(id, data.Basis);
                    }
                    else
                    {
                        var refIdList = currentBasis.Select(b => b.ReferenceWaterRightFacilityId).ToArray();
                        var newBasis = data.Basis.Where(b => !refIdList.Contains(b.ReferenceWaterRightFacilityId)).ToList();
                        var deleteBasis = currentBasis.Where(c => !data.Basis.Select(b => b.ReferenceWaterRightFacilityId).ToArray().Contains(c.ReferenceWaterRightFacilityId)).ToList();

                        BasisAction(id, newBasis);

                        var updateBasis = data.Basis.Where(b => refIdList.Contains(b.ReferenceWaterRightFacilityId)).ToList();

                        BasisAction(id, updateBasis, currentBasis);
                        BasisAction(deleteBasis,id);
                    }
                }               
                if(data.Hydrology != null)
                {
                    data.Hydrology.UserName = userName;
                    hydro=VAwsHydro.Update(data.Hydrology);
                }
                else
                {
                    hydro = VAwsHydro.Get(h => h.WaterRightFacilityId == id);
                }

                if (data.AmaDemand != null)
                {
                    if (data.AmaDemand.WaterRightFacilityId == null)
                    {
                        data.AmaDemand.WaterRightFacilityId = id;
                        //var pcc=QueryResult.RgrRptGet(id);
                    }
                    amaDemand = VAwsActiveManagementArea.Update(data.AmaDemand);
                }
                else
                {
                    amaDemand = VAwsActiveManagementArea.Get(x => x.WaterRightFacilityId == id);
                }

                impact=AwAreaOfImpact100.Get(a => a.WaterRightFacilityId == id);

                if (data.WtrImpact != null)
                {
                    var wtrImpact = data.WtrImpact;
                    wtrImpact.WaterRightFacilityId = id;

                    if (!(wtrImpact.GroundwaterAreaOfImpact == null && wtrImpact.SurfaceWaterAreaOfImpact == null && wtrImpact.EffluentAreaOfImpact == null && wtrImpact.CAPAreaOfImpact == null && wtrImpact.ColoradoRiverAreaOfImpact == null))
                    {                       

                        if(impact != null)
                        {
                            wtrImpact.UpdateBy = userName;
                            wtrImpact.UpdateDt = currentDate;                           
                            impact = AwAreaOfImpact100.Update(wtrImpact);
                        }

                       else
                        {

                            wtrImpact.CreateBy = userName;
                            wtrImpact.CreateDt = currentDate;
                            wtrImpact.WaterRightFacilityId = id;                           
                            AwAreaOfImpact100.Add(wtrImpact);
                            impact = wtrImpact;
                            impact = wtrImpact;
                        }

                    }
                }
            }

            var basis = VAwsWrfWrfDemand.GetList(d => d.WaterRightFacilityId == id).Distinct().OrderByDescending(x => x.WaterDemand).ToList();
            Basis = basis;
            Hydrology = hydro;
            AmaDemand = amaDemand;
            WtrImpact = impact;           
        }        

        private void BasisAction(List<VAwsWrfWrfDemand> deleteBasis, int id)
        {
            int demandCount = 0;
            int wrfwrfCount = 0;
            if (!(deleteBasis != null && deleteBasis.Count() > 0))
            {    
                return;
            }

            foreach (var d in deleteBasis)
            {
                var deleteId= deleteBasis.Select(b => b.ReferenceWaterRightFacilityId).ToArray();
                var dDemand = AwWrfWrfDemand.Get(dd => dd.WaterRightFacilityIdTo == id && dd.RelationshipTypeCode == "AWWD" && deleteId.Contains(dd.WaterRightFacilityIdFrom));
                var dWrfWrf = WaterRightFacilityToWaterRightFacility.Get(dd => dd.WaterRightFacilityIdTo == id && dd.RelationshipTypeCode == "AWWD" && deleteId.Contains(dd.WaterRightFacilityIdFrom));

                if (dDemand != null)
                {
                    AwWrfWrfDemand.Delete(dDemand);
                    demandCount++;
                }

                if (dWrfWrf != null)
                {
                    WaterRightFacilityToWaterRightFacility.Delete(dWrfWrf);
                    wrfwrfCount++;                
                   
                }
            }
            ActionStatus["deleteWrfWrf"]=wrfwrfCount;
            ActionStatus["deleteDemand"]=demandCount;
        }

        private void BasisAction(int id, List<VAwsWrfWrfDemand> updateBasis, List<VAwsWrfWrfDemand> currentBasis)
        {
            int updateCount = 0;

           // dynamic actionStatus = new ExpandoObject();
            if (!(updateBasis != null && updateBasis.Count() > 0))
            {                
                return;
            }

            foreach (var b in updateBasis)
            {
                // VAwsWrfWrfDemand.Update(b);

                var sameBasis = currentBasis.Where(c => c.ReferenceWaterRightFacilityId == b.ReferenceWaterRightFacilityId && c.WaterDemand == b.WaterDemand).ToList();

                if (!(sameBasis != null && sameBasis.Count() > 0))
                {
                    var updateRecord = new AwWrfWrfDemand()
                    {
                        UpdateBy = userName,
                        UpdateDt = currentDate,
                        WaterRightFacilityIdFrom = b.ReferenceWaterRightFacilityId ?? -1,
                        WaterRightFacilityIdTo = id,
                        RelationshipTypeCode = "AWWD",
                        WaterDemand = b.WaterDemand
                    };

                    AwWrfWrfDemand.Update(updateRecord);

                    updateCount++;
                }
            }
            ActionStatus["updateDemand"] = updateCount;
        }

        private void BasisAction(int id, List<VAwsWrfWrfDemand> basis)
        {
            var wrfwrf = new List<WaterRightFacilityToWaterRightFacility>();
            var demand = new List<AwWrfWrfDemand>();            
            
            if(!(basis != null || basis.Count() > 0))
            {            
                return;
            }

            foreach (var b in basis)
            {
                if (b.ReferenceWaterRightFacilityId != null)
                {
                    var existingWrfWrf = WaterRightFacilityToWaterRightFacility.Get(w => w.RelationshipTypeCode == "AWWD" && w.WaterRightFacilityIdFrom == b.ReferenceWaterRightFacilityId && w.WaterRightFacilityIdTo == id);

                    if (existingWrfWrf == null)
                    {
                        wrfwrf.Add(new WaterRightFacilityToWaterRightFacility()
                        {
                            CreateBy = userName,
                            CreateDt = currentDate,
                            WaterRightFacilityIdFrom = b.ReferenceWaterRightFacilityId ?? -1,
                            RelationshipTypeCode = "AWWD",
                            WaterRightFacilityIdTo = id
                        });                         
                    }

                    demand.Add(new AwWrfWrfDemand()
                    {
                        CreateBy = userName,
                        CreateDt = currentDate,

                        WaterRightFacilityIdFrom = b.ReferenceWaterRightFacilityId ?? -1,
                        WaterRightFacilityIdTo = id,
                        RelationshipTypeCode = "AWWD",
                        WaterDemand = b.WaterDemand
                    });
                }


                if (wrfwrf != null && wrfwrf.Count() > 0)
                {
                    WaterRightFacilityToWaterRightFacility.AddAll(wrfwrf);
                   ActionStatus["insertWrfWrf"]= wrfwrf.Count();
                }

                if (demand != null && demand.Count() > 0)
                {
                    AwWrfWrfDemand.AddAll(demand);
                    ActionStatus["insertDemand"] = demand.Count();                   
                }

                
            }

        }

    }



    
}