namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using SharedUtilities;
    using HydrosApi.Models.Adjudication;
    using HydrosApi.Data;

    [Table("ADWR.SW_AIS_VIEW")]
    public partial class SW_AIS_VIEW
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
        [StringLength(4000)]
        public string FILE_LINK
        {
            get; set;
        }

        public static List<SW_AIS_VIEW> SurfaceWaterView(string swList) //a comma-delimited list
        {
            var db = new ADWRContext();
            var swMatch = DelimitedColumnHandler.FileInformation(swList).Where(i=>i.FileType != "55" && i.FileType != "35").Select(i => i.FileNumber == null ? -1 : int.Parse(i.FileNumber)).ToList();

            var sw = db.SW_AIS_VIEW.Where(s => swMatch.Contains(s.ART_APPLI_NO ?? -1)).ToList();

            foreach (var item in sw)
            {

                if (item.ART_APPLI_NO != null)
                {
                    var swFile = DocuShareManager.GetFileLink(item.ART_APPLI_NO.ToString(), "", "SW");
                    item.FILE_LINK = swFile;
                }
            }

            return sw;

        }
    }
}
