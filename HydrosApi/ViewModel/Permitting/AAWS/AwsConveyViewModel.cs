namespace HydrosApi.ViewModel.Permitting.AAWS
{

    using System.Collections.Generic;
    using System.Linq;
    using Models.Permitting.AAWS;
    using Models.ADWR;
    using Data;
    using System;
 


    public class AwsConveyViewModel
    {
        public VAwsOriginalFile OriginalFile { get; set; }
        public List<VAwsConveyFile> ConveyFile { get; set; }
        public Dictionary<string,object> StatusReport { get; set; }


        public AwsConveyViewModel()
        {
        }

        public AwsConveyViewModel(int? id)
        {
            var originalFile = VAwsOriginalFile.Get(o => o.SearchWaterRightFacilityId == id);
            var conveyFile = VAwsConveyFile.GetList(c => c.SearchWaterRightFacilityId == id).OrderBy(c => c.ConveyingFileNo).ToList();
            OriginalFile = originalFile;
            ConveyFile = conveyFile;
        }

        public AwsConveyViewModel(int? id, AwsConveyViewModel conveyance, string user)
        {
            int actionCount = 0;
            var currentDate = DateTime.Now;

            if (id != null && conveyance != null)
            {
                var wrf_id = id ?? -1;    
                
                if(conveyance.OriginalFile != null && conveyance.OriginalFile.OriginalFileNo != null)                    
                {
                    UpdateOriginalFile(wrf_id, conveyance.OriginalFile.OriginalFileNo, user, currentDate);
                }

                if (conveyance.ConveyFile != null && conveyance.ConveyFile.Count() > 0)
                {
                    var deletes = conveyance.ConveyFile.Where(d => d.DeleteItem == 1).Select(d => d.ConveyingWaterRightFacilityId).ToArray();
                    var adds = conveyance.ConveyFile.Where(a => !(a.DeleteItem == 1)).ToList();

                    if (deletes != null && deletes.Count() > 0)
                    {
                        actionCount += DeleteConveyance(wrf_id, deletes, user);                        
                    }

                    if (adds != null && adds.Count() > 0)
                    {
                        actionCount+=AddConveyance(wrf_id, adds, user, currentDate);
                    }
                }
            }

            var originalFile = VAwsOriginalFile.Get(o => o.SearchWaterRightFacilityId == id);
            var conveyFile = VAwsConveyFile.GetList(c => c.SearchWaterRightFacilityId == id).OrderBy(c => c.ConveyingFileNo).ToList();
            OriginalFile = originalFile;
            ConveyFile = conveyFile;
        }
        

        private void UpdateOriginalFile(int id, string originalFile, string user, DateTime currentDate)
        {
            
            var record = QueryResult.GetWrfRecord(originalFile);
            if(record==null || originalFile==null)
            {
                StatusReport.Add("OriginalFileNumber", "Invalid Original File Number");
            }

            else
            {
                var activityRecord=AwAppActivityTrk.Get(a => a.ActivityCode == "ISSD" && a.WaterRightFacilityId == record.Id);

                if (activityRecord != null)
                {
                    var orig = WaterRightFacilityToWaterRightFacility.GetList(o => o.WaterRightFacilityIdFrom == id && o.RelationshipTypeCode == "AWOF");
                    var exact = orig.Where(o => o.WaterRightFacilityIdTo == record.Id);

                    if (orig != null && exact == null)
                    {
                        var origItem = orig.FirstOrDefault();
                        origItem.WaterRightFacilityIdTo = record.Id;
                        origItem.UpdateBy = user;
                        origItem.UpdateDt = currentDate;
                        WaterRightFacilityToWaterRightFacility.Update(origItem);
                    }

                    else if (orig == null)
                    {
                        var item = new WaterRightFacilityToWaterRightFacility()
                        {
                            WaterRightFacilityIdFrom = id,
                            WaterRightFacilityIdTo = record.Id,
                            RelationshipTypeCode = "AWOF",
                            IsActive = "Y",
                            CreateBy = user,
                            CreateDt = currentDate
                        };
                    }
                }
                else
                {
                    StatusReport.Add("OriginalFileActivity", "There is no issued record for the Original File Number provided.");
                }
            }
        }

        private int DeleteConveyance(int id, int[] deleteId, string user)
        {
            int actionCount = 0;

            try
            {
                using (var context = new OracleContext())
                {
                    var del = context.WRF_WRF.Where(dd => dd.WaterRightFacilityIdFrom == id && dd.RelationshipTypeCode == "AWPF" && deleteId.Contains(dd.WaterRightFacilityIdTo));

                    if (del != null && del.Count() > 0)
                    {
                            context.WRF_WRF.RemoveRange(del);
                            context.SaveChanges();
                            actionCount = del.Count();
                    }
                }     
            }
            catch(Exception exception)
            {
              
                StatusReport.Add("DeleteError", exception);
            }

             return actionCount;
        }

        private int AddConveyance(int id, List<VAwsConveyFile> adds, string user, DateTime currentDate)
        {
            int actionCount = 0;
            foreach(var a in adds)
            {
                var record = QueryResult.GetWrfRecord(a.ConveyingFileNo);

                if(record != null)
                {
                    var addItem = new WaterRightFacilityToWaterRightFacility()
                    {
                        WaterRightFacilityIdFrom = id,
                        WaterRightFacilityIdTo = record.Id,
                        RelationshipTypeCode = "AWPF",
                        IsActive="Y",
                        CreateBy = user,
                        CreateDt = currentDate
                    };

                    WaterRightFacilityToWaterRightFacility.Add(addItem);

                    actionCount++;
                }
            }

            return actionCount;
        }        

    }
}