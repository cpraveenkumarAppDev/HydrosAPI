namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using HydrosApi.Data;
    using HydrosApi.Models.Adjudication;

    [Table("ADJ_INV.WATERSHED_FILE_REPORT")]
    public partial class WATERSHED_FILE_REPORT : AdwrRepository<WATERSHED_FILE_REPORT>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ID { get; set; }

        public int? OBJECTID { get; set; }

        [StringLength(100)]
        public string WFR_NUM { get; set; }

        [NotMapped]
        public List<SOC_AIS_VIEW> StatementOfClaim { get; set; }

        [NotMapped]
        public string BOC { get; set; }

        [NotMapped]
        public List<EXPLANATIONS> Explanation { get; set; }
        [NotMapped]
        public List<WELLS_VIEW> Well { get; set; }
        [NotMapped]
        public List<SW_AIS_VIEW> Surfacewater { get; set; }
        [NotMapped]
        public List<FILE> FileList { get; set; }
        [NotMapped]
        public List<AISPODS> PointOfDiversion { get; set; }
        [NotMapped]
        public List<PROPOSED_WATER_RIGHT> ProposedWaterRight { get; set; }

        [NotMapped]
        public List<ExplanationType> ExplanationTypeList { get; set; }

        //[NotMapped]
        //public  WATERSHED_FILE_REPORT_SDE WatershedFileReportSDE { get; set; }

        public static bool WfrExists(string wfrnum)
        {
            var wfr = Get(w => w.WFR_NUM == wfrnum);

            return wfr != null ? true : false;
        }

        public static WATERSHED_FILE_REPORT WatershedFileReportByObjectId(int? id)
        {
            var wfr = new WATERSHED_FILE_REPORT();
            //var wfr = wfrX != null ? wfrX : Get(p => p.OBJECTID == id);

            var wfrData = Get(w => w.OBJECTID == id);

            if(wfrData != null)
            {
                wfr = wfrData;
            }

            //WFR-18-A19008005DAB-01

            var wfrSde = WATERSHED_FILE_REPORT_SDE.WatershedFileReportSDE(id);

           

            

            if(wfrData == null && wfrSde != null)
            {              
                wfr.OBJECTID = wfrSde.OBJECTID;
                wfr.WFR_NUM = wfrSde.WFR_NUM;
               // wfr.WatershedFileReportSDE = WATERSHED_FILE_REPORT_SDE.WatershedFileReportSDE(id);
            }

            if (wfr.ID != null)
            {
                wfr.Explanation = EXPLANATIONS.GetList(p => p.WFR_ID == wfr.ID);
                wfr.FileList = FILE.GetList(p => p.WFR_ID == wfr.ID);
                //wfr.pods = WFR_POD.GetList(p => p.WFR_ID == wfr.ID);
                wfr.PointOfDiversion = WFR_POD.GetList(p => p.WFR_ID == wfr.ID).Select(p => p.PointOfDiversion).ToList();
                wfr.ProposedWaterRight = PROPOSED_WATER_RIGHT.GetList(p => p.WFR_ID == wfr.ID);
                wfr.ExplanationTypeList = ExplanationType.GetAll();
            }

            char[] delimiters = new[] { ',', ';'};

            if (wfrSde != null)
            {

                if (wfrSde.SOC != null)
                {
                    var soc = FileFromStringList.GetFileFromStringList(wfrSde.SOC, new[] { ',', ';' });
                    wfr.StatementOfClaim = soc?.Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.NumericFileNo)).Distinct().ToList();
                }

                if (wfrSde.BOC != null)
                {
                    var bocList = FileFromStringList.GetFileFromStringList(wfrSde.BOC, new[] { ',', ';' });
                    var wellList = bocList?.Where(p => p.Program == "55" || p.Program == "35");
                    var swList = bocList?.Where(p => p.Program != "55" && p.Program != "35");
                    wfr.Well = wellList?.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.FileNo && s.PROGRAM == f.Program)).ToList();
                    wfr.Surfacewater = swList?.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.NumericFileNo)).ToList();
                }

                /*
                wfr.SOC = wfrSde.SOC == null ? null :
                     (from s in wfrSde.SOC.Split(delimiters)
                      select new
                      {
                          program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                          file_no = int.Parse((s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", ""))
                      }).Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.file_no)).Where(c => c != null).Distinct().ToList();
                if (wfrSde.BOC != null)
                {
                    var bocList = (from p in (from s in wfrSde.BOC.Split(delimiters)
                                              select new
                                              {
                                                  program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                                                  file_no = (s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", "")
                                              })
                                   select new
                                   {
                                       p.program,
                                       p.file_no,
                                       numeric_file_no = int.Parse(p.file_no == null ? "0" : p.file_no.ToString()),
                                       registry_id = p.program + "-" + p.file_no
                                   }).Distinct();
                    var wellList = bocList.Where(p => p.program == "55" || p.program == "35");
                    var swList = bocList.Where(p => p.program != "55" && p.program != "35");

                    wfr.Well = wellList == null ? null :
                        wellList.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.file_no && s.PROGRAM == f.program)).Where(c => c != null).ToList();
                    wfr.Surfacewater = swList == null ? null : swList.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.numeric_file_no)).Where(c => c != null).ToList();
                */
                //}                
            }
            return wfr;
        }


        
    }
}
