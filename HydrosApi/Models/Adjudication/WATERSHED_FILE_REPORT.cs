namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using HydrosApi.Data;

    [Table("ADJ_INV.WATERSHED_FILE_REPORT")]
    public partial class WATERSHED_FILE_REPORT: AdwrRepository<WATERSHED_FILE_REPORT>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? OBJECTID { get; set; }

        [StringLength(100)]
        public string WFR_NUM { get; set; }    
        
        [NotMapped]
        public List<EXPLANATIONS> Explanations { get; set; }
        [NotMapped]
        public List<FILE> FileList { get; set; }

        public static WATERSHED_FILE_REPORT WatershedFileReport(int id)
        {
            var wfr = WATERSHED_FILE_REPORT.Get(p => p.ID == id);
            wfr.Explanations = EXPLANATIONS.GetList(p => p.WFR_ID == id);
            wfr.FileList = FILE.GetList(p => p.WFR_ID == id);
            return wfr;
        }
    }
}
