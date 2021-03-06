namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;

    [Table("ADJ.LLC_WFRS_ALL")]
    public partial class WATERSHED_FILE_REPORT_SDE : SdeRepository<WATERSHED_FILE_REPORT_SDE>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        [Key]
        public int OBJECTID { get; set; }

        [StringLength(50)]
        public string WFR_NUM { get; set; }

        [StringLength(50)]
        public string WFR_STATUS { get; set; }

        [StringLength(500)]
        public string WFR_OWNER { get; set; }

        [StringLength(500)]
        public string WFR_LESSEE { get; set; }

        [StringLength(500)]
        public string WFR_DESC { get; set; }

        [StringLength(100)]
        public string LSE_NUMBER { get; set; }

        [StringLength(100)]
        public string LSE_NAME { get; set; }

        [StringLength(50)]
        public string BOC { get; set; }

        public string WS { get; set; }
        public int? SW { get; set; }

        [StringLength(100)]
        public string WFR_PARCELS { get; set; }

        [StringLength(100)]
        public string DWR_ID { get; set; }

        [StringLength(100)]
        public string WFR_REGION { get; set; }

        [StringLength(100)]
        public string WFR_SUBREGION { get; set; }

        [StringLength(100)]
        public string WFR_SEQUENCE { get; set; }

        [StringLength(300)]
        public string SOC { get; set; }


        ///get the WFR using its objectid
        public static WATERSHED_FILE_REPORT_SDE WatershedFileReportSDE(int? objectid)
        {
            return WATERSHED_FILE_REPORT_SDE.Get(p => p.OBJECTID == objectid);
        }

        ///get the WFR using its WFR_NUM
        public static WATERSHED_FILE_REPORT_SDE WatershedFileReportSDE(string wfr_num)
        {
            return WATERSHED_FILE_REPORT_SDE.Get(p => p.WFR_NUM == wfr_num);
        }

        ///get all WFRS
        public static List<WATERSHED_FILE_REPORT_SDE> WatershedFileReportSDE()
        {
            return WATERSHED_FILE_REPORT_SDE.GetAll();
        }

        ///get a list of WFR with the provided object ids
        public static List<WATERSHED_FILE_REPORT_SDE> WatershedFileReportSDE(IEnumerable<int> objectids)
        {
            return WATERSHED_FILE_REPORT_SDE.GetList(p => objectids.Contains(p.OBJECTID));
        }

        



    }

    public class WatershedFileReportSDEOptions
    {
        public string ColumnName { get; set; }
        public string SearchValue { get; set; }

        public bool? Sort { get; set; }

        public string SortDirection { get; set; }

        public int? PageNo { get; set; }

        public int? PageRow { get; set; }

        public static List<WATERSHED_FILE_REPORT_SDE> PopulateWatershedFileReport(List<WatershedFileReportSDEOptions> options)
        {
            List<WATERSHED_FILE_REPORT_SDE> report=null;
            if (options==null)
            {
                return WATERSHED_FILE_REPORT_SDE.GetAll().Take(50).ToList();                
            }

             
            return report;
        }
    }
}