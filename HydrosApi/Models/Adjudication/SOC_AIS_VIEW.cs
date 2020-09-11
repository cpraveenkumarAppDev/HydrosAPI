namespace HydrosApi 
{
    
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    [Table("SOC.SOC_AIS_VIEW")]
    public partial class SOC_AIS_VIEW 
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
            get; set;

        }

        public static List<SOC_AIS_VIEW> StatementOfClaimView(string socList) //a comma-delimited list
        {
            var db = new ADWRContext();            
            var socMatch = DelimitedColumnHandler.FileInformation(socList).Select(i => i.FileNumber == null ? -1 : int.Parse(i.FileNumber)).ToList();

             
            var soc = db.SOC_AIS_VIEW.Where(s => socMatch.Contains(s.FILE_NO)).Distinct().ToList();

            if (soc == null)
                return null;

            foreach(var item in soc)
            {
                //var socFile = DocuShareManager.GetFileLink("39-" + item.FILE_NO.ToString(), "", "SOC");
                //item.FILE_LINK = socFile;
            }

            return soc.Distinct().ToList();

        }

    }
}
