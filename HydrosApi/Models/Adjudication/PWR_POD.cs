namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ADJ_INV.PWR_POD")]

    public partial class PWR_POD
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? PWR_ID { get; set; }

        public int? POD_ID { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [NotMapped]
        public virtual PROPOSED_WATER_RIGHT PROPOSED_WATER_RIGHT { get; set; }
    }
}
