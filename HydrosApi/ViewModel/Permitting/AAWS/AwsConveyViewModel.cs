namespace HydrosApi.ViewModel.Permitting.AAWS
{

    using System.Collections.Generic;
    using System.Linq;
    using Models.Permitting.AAWS;
    using Models.ADWR;
    using Data;
    using System;
    using System.Data.Entity;


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
            StatusReport = new Dictionary<string, object>();

            if (id != null && conveyance != null)
            {
                var wrf_id = id ?? -1;


                using (var context = new OracleContext())
                {
                    if (conveyance.OriginalFile != null && conveyance.OriginalFile.OriginalFileNo != null)
                    {
                        actionCount += UpdateOriginalFile(wrf_id, conveyance.OriginalFile.OriginalFileNo, user, currentDate, context);
                    }

                    if (conveyance.ConveyFile != null && conveyance.ConveyFile.Count() > 0)
                    {
                        var deletes = conveyance.ConveyFile.Where(d => d.DeleteItem == 1).Select(d => d.ConveyingWaterRightFacilityId).ToArray();
                        var adds = conveyance.ConveyFile.Where(a => !(a.DeleteItem == 1)).ToList();

                        if (deletes != null && deletes.Count() > 0)
                        {
                            actionCount += DeleteConveyance(wrf_id, deletes, user, context);

                        }

                        if (adds != null && adds.Count() > 0)
                        {
                            actionCount += AddConveyance(wrf_id, adds, user, currentDate, context);
                        }
                    }


                    var errors = StatusReport != null ? StatusReport.Where(s => s.Value.ToString().StartsWith("Error")) : null;
                    var errorCount = errors != null && errors.Count() > 0 ? errors.Count() : 0;

                    if (errorCount == 0 && actionCount > 0)
                    {
                        StatusReport = null;
                        context.SaveChanges();
                    }

                }
            }

            var originalFile = VAwsOriginalFile.Get(o => o.SearchWaterRightFacilityId == id);
            var conveyFile = VAwsConveyFile.GetList(c => c.SearchWaterRightFacilityId == id).OrderBy(c => c.ConveyingFileNo).ToList();
            OriginalFile = originalFile;
            ConveyFile = conveyFile;
        }
        

        private int UpdateOriginalFile(int id, string originalFile, string user, DateTime currentDate, OracleContext context)
        {
            var actionCount = 0;
            var record = QueryResult.GetWrfRecord(originalFile);
            if(record==null || originalFile==null)
            {
                StatusReport.Add("OriginalFileNumberInvalid", string.Format("Error: Original File Number {0} is invalid",originalFile));
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

                        context.WRF_WRF.Attach(origItem);
                        context.Entry(origItem).State = EntityState.Modified;                        
                        actionCount++;

                        StatusReport.Add("OriginalFileNumberUpdate", string.Format("Updated: {0}", OriginalFile));
                    }

                    else if (!(orig != null && orig.Count() > 0))
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

                        context.WRF_WRF.Add(item);                         
                        actionCount++;
                        StatusReport.Add("OriginalFileNumberAdd", string.Format("Added: {0}", OriginalFile));
                    }
                }
                else
                {
                    StatusReport.Add("OriginalFileNumberError", string.Format("Error: There is no issued record for the Original File Number {0} provided.", OriginalFile));
                }  
                
                
                 
            }
            return actionCount;
        }

        private int DeleteConveyance(int id, int[] deleteId, string user, OracleContext context)
        {
            int actionCount = 0;            

            try
            { 
                var del = context.WRF_WRF.Where(dd => dd.WaterRightFacilityIdFrom == id && dd.RelationshipTypeCode == "AWPF" && deleteId.Contains(dd.WaterRightFacilityIdTo));
                actionCount = del.Count();


                if (del != null && del.Count() > 0)
                {
                    context.WRF_WRF.RemoveRange(del);
                    StatusReport.Add("ConveyanceDelete", String.Format("Successfully deleted {0} records", actionCount));
                }                
                   
            }
            catch(Exception exception)
            {
                StatusReport.Add("ConveyanceException", QueryResult.BundleExceptions(exception));
            }

             return actionCount;
        }

        private int AddConveyance(int id, List<VAwsConveyFile> adds, string user, DateTime currentDate, OracleContext context)
        {
            int actionCount = 0;
            foreach(var a in adds)
            {
                var record = a.ConveyingFileNo != null ? QueryResult.GetWrfRecord(a.ConveyingFileNo) : null;

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

                    context.WRF_WRF.Add(addItem);
                    actionCount++;
                }

                else
                {
                    StatusReport.Add(String.Format("ConveyanceNotAdded{0}", actionCount), String.Format("Unable to add {0}", a.ConveyingFileNo ?? "No value supplied"));
                }

                if (actionCount > 0)
                {
                    StatusReport.Add(String.Format("ConveyanceAdded{0}", actionCount), String.Format("Successfully added {0} records", actionCount));
                }
            }

            return actionCount;
        }        

    }
}