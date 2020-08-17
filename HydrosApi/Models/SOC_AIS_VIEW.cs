namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
    }
}
