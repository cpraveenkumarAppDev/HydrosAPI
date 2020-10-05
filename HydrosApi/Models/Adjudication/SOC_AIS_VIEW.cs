using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi.Models 
{
    [Table("SOC.SOC_AIS_VIEW")]
    public partial class SOC_AIS_VIEW : AdwrRepository<SOC_AIS_VIEW>
    {
        public decimal? ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FILE_NO { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(50)]
        public string USE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string WS { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MAIN_ID { get; set; }

        [StringLength(45)]
        public string PCC { get; set; }

        public DateTime? FILE_DATE { get; set; }

        public DateTime? AMEND_DATE { get; set; }

        [NotMapped]
        [StringLength(4000)]
        public string FILE_LINK
        {
            get
            {
                DocushareService doc = new DocushareService();
                return doc.GetSocDocs("39-" + this.FILE_NO).FirstOrDefault().FileUrl;
            }

            set
            {
                this.FILE_LINK = value;
            }
        }

        //****this is no longer necessary and can be removed eventually
        public static List<SOC_AIS_VIEW> StatementOfClaimView(string socList) //a comma-delimited list
        {
            var db = new ADWRContext();            
            var socMatch = DelimitedColumnHandler.FileInformation(socList).Select(i => i.FileNumber == null ? -1 : int.Parse(i.FileNumber)).ToList();

             
            var soc = db.SOC_AIS_VIEW.Where(s => socMatch.Contains(s.FILE_NO)).Distinct().ToList();

            if (soc == null)
                return null;
            DocushareService docuService = new DocushareService();
            foreach(var item in soc)
            {
                var url = docuService.GetSocDocs("39-" + item.FILE_NO).FirstOrDefault().FileUrl;
                item.FILE_LINK = url;
               
            }

            return soc.Distinct().ToList();
        }
    }
}
