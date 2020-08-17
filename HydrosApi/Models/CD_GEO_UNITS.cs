namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_GEO_UNITS")]
    public partial class CD_GEO_UNITS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_GEO_UNITS()
        {
            CD_LITHO_STRAT = new HashSet<CD_LITHO_STRAT>();
        }

        [Key]
        [StringLength(8)]
        public string CODE { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }

        public decimal? AGE_MYA { get; set; }

        [StringLength(100)]
        public string SUPEREON { get; set; }

        [StringLength(100)]
        public string EON { get; set; }

        [StringLength(100)]
        public string ERA { get; set; }

        [StringLength(100)]
        public string PERIOD_SYSTEM { get; set; }

        [StringLength(100)]
        public string EPOCH_SERIES { get; set; }

        public int? SORT_ORDER { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(100)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string UPDATEBY { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CD_LITHO_STRAT> CD_LITHO_STRAT { get; set; }
    }
}
