namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADJ_INV.WELLS_VIEW")]
    public partial class WELLS_VIEW
    {
        public decimal? ID { get; set; }

        [StringLength(2)]
        public string PROGRAM { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string FILE_NO { get; set; }

        [StringLength(61)]
        public string OWNER { get; set; }

        [StringLength(93)]
        public string CADASTRAL { get; set; }

        [StringLength(11)]
        public string PCC { get; set; }

        [StringLength(9)]
        public string REGISTRY_ID { get; set; }

        [StringLength(2)]
        public string WSHD_CODE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string WELL_TYPE { get; set; }

        public DateTime? APPLICATION_DATE { get; set; }

        public DateTime? INSTALL_DATE { get; set; }
    }
}
