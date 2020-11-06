namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using HydrosApi.Data;

    [Table("ADJ_INV.WATERSHED_FILE_REPORT")]
    public partial class WATERSHED_FILE_REPORT: AdwrRepository<WATERSHED_FILE_REPORT>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int OBJECTID { get; set; }

        [StringLength(100)]
        public string WFR_NUM { get; set; }

        [NotMapped]
        public List<SOC_AIS_VIEW> SOC { get; set; }

        [NotMapped]
        public string BOC { get; set; }

        [NotMapped]
        public List<EXPLANATIONS> Explanations { get; set; }
        [NotMapped]
        public List<WELLS_VIEW> Well { get; set; }
        [NotMapped]
        public List<SW_AIS_VIEW> Surfacewater { get; set; }
        [NotMapped]
        public List<FILE> FileList { get; set; }

        public static WATERSHED_FILE_REPORT WatershedFileReportByObjectId(int id)
        {
            var wfr = WATERSHED_FILE_REPORT.Get(p => p.OBJECTID == id);
            var wfrSde = WATERSHED_FILE_REPORT_SDE.WatershedFileReportSDE(id);
            wfr.Explanations = EXPLANATIONS.GetList(p => p.WFR_ID == wfr.ID);
            wfr.FileList = FILE.GetList(p => p.WFR_ID == id);
            wfr.SOC = wfrSde.SOC == null ? null:
                 (from s in wfrSde.SOC.Split(',')
                  select new
                  {
                      program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                      file_no = int.Parse((s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", ""))
                  }).Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.file_no)).Distinct().ToList();
            if (wfrSde.BOC != null)
            {
                var bocList = (from p in (from s in wfrSde.BOC.Split(',')
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
                    wellList.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.file_no && s.PROGRAM == f.program)).ToList();

                wfr.Surfacewater = swList == null ? null : swList.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.numeric_file_no)).ToList();
            };
            return wfr;
        }
    }
}
