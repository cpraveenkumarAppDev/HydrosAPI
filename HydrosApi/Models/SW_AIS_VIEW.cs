namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
    }
}
