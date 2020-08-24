namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADJ_INV.EXPLANATION")]
    public partial class EXPLANATION
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? WFR_ID { get; set; }

        public int? PWR_ID { get; set; }

        [StringLength(50)]
        public string EXP_TYPE { get; set; }

        [StringLength(100)]
        public string LOCATION { get; set; }

        [Column("EXPLANATION")]
        [StringLength(2000)]
        public string EXPLANATION1 { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }
    }
}
