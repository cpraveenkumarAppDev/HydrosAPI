namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_FORMATIONS")]
    public partial class CD_FORMATIONS
    {
        [Key]
        [StringLength(100)]
        public string FORMATION { get; set; }

        public decimal? AGE_MA { get; set; }

        [Required]
        [StringLength(5)]
        public string MU_CODE { get; set; }

        [StringLength(25)]
        public string GU_CODE { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE1 { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE2 { get; set; }

        [StringLength(1000)]
        public string NOTES { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(5)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(5)]
        public string UPDATEBY { get; set; }
    }
}
