using HydrosApi.Services;
using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi.Models
{ 

    [Table("ADWR.SW_AIS_VIEW")]
    public partial class SW_AIS_VIEW:AdwrRepository<SW_AIS_VIEW>
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(6)]
        public string ART_PROGRAM { get; set; }

        public int? ART_APPLI_NO { get; set; }

        public int? ART_CONVY_NO { get; set; }

        [StringLength(40)]
        public string NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ART_IDNO { get; set; }

        [StringLength(88)]
        public string PCC { get; set; }

        [StringLength(6)]
        public string WS_CODE { get; set; }

        public DateTime? ART_FILE_DATE { get; set; }

        [StringLength(4000)]
        public string USE { get; set; }

        [NotMapped]         
        public List<string> FILE_LINK
        {
            get
            {
                DocushareService doc = new DocushareService();
                return doc.getSurfaceWaterDocs(this.ART_PROGRAM + "-" + this.ART_APPLI_NO).Select(f => f.FileUrl).Distinct().ToList();
            }

            set
            {
                this.FILE_LINK = value;
            }
        }

        //this is no longer necessary and can be removed eventually
        public static List<SW_AIS_VIEW> SurfaceWaterView(string swList) //a comma-delimited list
        {
            
            var swMatch = DelimitedColumnHandler.FileInformation(swList).Where(i=>i.FileType != "55" && i.FileType != "35").ToList();
            var sw = new List<SW_AIS_VIEW>();

            DocushareService docuService = new DocushareService();
            foreach (var item in swMatch)
            {
                int fileNo = int.Parse(item.FileNumber);
                var swItem = SW_AIS_VIEW.GetList(s => s.ART_APPLI_NO == fileNo && s.ART_PROGRAM == item.FileType);
                var swFile=docuService.getSurfaceWaterDocs(item.FileType+"-"+item.FileNumber).Select(f => f.FileUrl).Distinct();
                swItem.Select(d => { d.FILE_LINK = swFile.ToList(); return d; }).ToList();
              
                sw.AddRange(swItem); //<== it might be possible to have multiple documents related to the one item 

            }

            return sw;

        }
    }
}
