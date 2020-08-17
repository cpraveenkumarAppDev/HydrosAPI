namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_LITHO_STRAT_TYPE")]
    public partial class CD_LITHO_STRAT_TYPE
    {
        [Key]
        [StringLength(1)]
        public string CODE { get; set; }

        [StringLength(25)]
        public string DESCRIPTION { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(5)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(5)]
        public string UPDATEBY { get; set; }
    }
}
