using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi
{

    [Table("ADJ_INV.WELLS_VIEW")]
    public partial class WELLS_VIEW : AdwrRepository<WELLS_VIEW>
    {

        [NotMapped]
        [StringLength(4000)]
        public string FILE_LINK
        {
            get
            {
                DocushareService doc = new DocushareService();
                return doc.getWellDocs(this.REGISTRY_ID).FileUrl;
            }

            set
            {
                this.FILE_LINK = value;
            }

        }
        [StringLength(11)]
        public string PCC { get; set; }
        [StringLength(61)]
        public string OWNER { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string WELL_TYPE { get; set; }
        public DateTime? APPLICATION_DATE { get; set; }

        public decimal? ID { get; set; }

        [StringLength(2)]
        public string PROGRAM { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string FILE_NO { get; set; }

        [StringLength(93)]
        public string CADASTRAL { get; set; }



        [StringLength(9)]
        public string REGISTRY_ID { get; set; }

        [StringLength(2)]
        public string WSHD_CODE { get; set; }

      

        

        public DateTime? INSTALL_DATE { get; set; }

        [NotMapped]
        public string StatusMsg { get; set; }

        //this is no longer necessary and can be removed eventually
        /*public static List<WELLS_VIEW> WellsView(string wellList)
        {
            var db = new ADWRContext();
            var wellMatch = DelimitedColumnHandler.FileInformation(wellList).Where(i=>i.FileType=="55"||i.FileType=="35").Select(i => i.FileNumber).ToList();

            if (wellMatch == null)
                return null;
             
            var well = db.WELLS_VIEW.Where(w => wellMatch.Contains(w.FILE_NO)).Distinct().ToList();

            if (well == null)
                return null;
            DocushareService docuService = new DocushareService();
            foreach (var item in well)
            {                
                item.FILE_LINK = docuService.getWellDocs(item.REGISTRY_ID).FileUrl;
            }

            return well;
        }*/
    }
}
