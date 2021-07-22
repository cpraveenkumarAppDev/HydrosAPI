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
        public List<string> StatusReport { get; set; }
       

        public AwsConveyViewModel()
        {
        }

        public AwsConveyViewModel(int? id)
        {
            var originalFile = VAwsOriginalFile.Get(o => o.SearchWaterRightFacilityId == id);
            var conveyFile = VAwsConveyFile.GetList(c => c.SearchWaterRightFacilityId == id).OrderBy(c=>c.ConveyingFileNo).ToList();
            OriginalFile = originalFile;
            ConveyFile = conveyFile;
        }

        public AwsConveyViewModel(int? id, AwsConveyViewModel conveyance, string user)
        {
            int actionCount = 0;
            var currentDate = DateTime.Now;

            if (id != null && conveyance != null)
            {
                using (var context = new OracleContext())
                {

                if (conveyance.OriginalFile != null)
                {
                    if(conveyance.OriginalFile.OriginalFileNo !=null && conveyance.OriginalFile.OriginalFileDate !=null)
                    {
                        var originalNo = conveyance.OriginalFile.OriginalFileNo;
                        var originalDate = conveyance.OriginalFile.OriginalFileDate;
                        var record = QueryResult.GetWrfRecord(originalNo);

                        if (record != null)
                        {
                            var existing = originalNo != null ? context.WRF_WRF.Where(x => x.WaterRightFacilityIdFrom == id && x.RelationshipTypeCode == "AWOF").FirstOrDefault() : null;

                            if (existing != null)
                            {
                                existing.WaterRightFacilityIdTo = record.Id;
                                existing.UpdateBy = user;
                                existing.UpdateDt = currentDate;
                            }

                            else
                            {
                                var addItem = new WaterRightFacilityToWaterRightFacility()
                                {
                                    WaterRightFacilityIdFrom = id ?? -1,
                                    WaterRightFacilityIdTo = record.Id,
                                    RelationshipTypeCode = "AWOF",
                                    CreateBy = user,
                                    CreateDt = currentDate
                                };

                                context.WRF_WRF.Add(addItem);

                            }

                            var addActivity = new AwAppActivityTrk()
                            {
                                WaterRightFacilityId=record.Id,
                                ActivityTrackDate=originalDate,
                                ActivityCode="ISSD",
                                CreateDt=currentDate,
                                CreateBy=user
                            };

                            context.AW_APP_ACTIVITY_TRK.Add(addActivity);
                            actionCount++;
                        }
                        else
                        {
                            StatusReport.Add(string.Format("Error: The conveying file number {0} could not be found, ", originalNo));
                        }
                    }
                }

                if (conveyance.ConveyFile != null)
                {
                   
                    foreach (var c in conveyance.ConveyFile)
                    {
                        if (c.DeleteItem == 1)
                        {
                            var deleteItem = context.WRF_WRF.Where(w => w.WaterRightFacilityIdFrom == c.SearchWaterRightFacilityId && w.WaterRightFacilityIdTo == c.ConveyingWaterRightFacilityId && w.RelationshipTypeCode == "AWPF").FirstOrDefault();

                            if (deleteItem != null)
                            {
                                context.WRF_WRF.Remove(deleteItem);
                                actionCount++;
                            }
                            else
                            {
                                StatusReport.Add(string.Format("Error: Could not delete {0}", c.ConveyingFileNo));
                            }
                        }

                        else
                        {
                            var record = QueryResult.GetWrfRecord(c.ConveyingFileNo);
                            if (record != null)
                            {
                                var addItem = new WaterRightFacilityToWaterRightFacility()
                                {
                                    WaterRightFacilityIdFrom = c.SearchWaterRightFacilityId ?? -1,
                                    WaterRightFacilityIdTo = record.Id,
                                    RelationshipTypeCode = "AWPF",
                                    CreateBy = user,
                                    CreateDt = currentDate
                                };
                                context.WRF_WRF.Add(addItem);
                                actionCount++;
                            }
                            else
                            {
                                StatusReport.Add(string.Format("Error: The conveying file number {0} could not be found, ", c.ConveyingFileNo));
                            }
                        }
                        if (actionCount > 0)
                        {
                            context.SaveChangesAsync();
                        }
                    }
                }
                   
            }
                //  var deletes = conveyance.ConveyFile.Where(c => c.DeleteItem == 1);
                // var adds = conveyance.ConveyFile.Where(c => !(c.DeleteItem == 1));

                /*if (deletes != null)
                {
                    var deleteIds = deletes.Select(i => i.ConveyingWaterRightFacilityId).ToArray();
                    var deleteRecords = WaterRightFacilityToWaterRightFacility.GetList(ww => ww.WaterRightFacilityIdFrom == id && deleteIds.Contains(ww.WaterRightFacilityIdTo) && ww.RelationshipTypeCode == "AWPF");

                    foreach(var d in deleteRecords)
                    {
                        WaterRightFacilityToWaterRightFacility.Delete(d);
                    }                        
                }
                if (adds != null)
                {
                    var validAdds = new List<WaterRightFacilityToWaterRightFacility>();
                    foreach (var a in adds)
                    {
                        if (a.ConveyingFileNo != null)
                        {
                            var record = QueryResult.GetWrfRecord(a.ConveyingFileNo);


                            if (record == null)
                            {
                                StatusReport.Add(string.Format("Error: The file number {0} is invalid, ", a.ConveyingFileNo));

                            }
                            else
                            {
                                StatusReport.Add(string.Format("Valid: The file number {0} was added, ", a.ConveyingFileNo));

                                validAdds.Add(new WaterRightFacilityToWaterRightFacility()
                                {
                                    WaterRightFacilityIdFrom = id ?? -1,
                                    WaterRightFacilityIdTo = record.Id,
                                    RelationshipTypeCode = "AWPF",
                                    CreateBy = user,
                                    CreateDt = DateTime.Now
                                });
                            }
                        }
                    }

                    var error = StatusReport.Where(s => s.Contains("Error"));

                    if (!(error != null && error.Count() > 0))
                    {
                        StatusReport = null;
                    }

                    if (validAdds != null && validAdds.Count() > 0)
                    {

                        WaterRightFacilityToWaterRightFacility.AddAll(validAdds);
                    }
                }                    

            }
        }*/
                

            }
        }
    }
}