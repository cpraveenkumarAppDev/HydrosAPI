namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_MAP_UNITS")]
    public partial class CD_MAP_UNITS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_MAP_UNITS()
        {
            CD_LITHO_STRAT = new HashSet<CD_LITHO_STRAT>();
        }

        [Key]
        [StringLength(5)]
        public string CODE { get; set; }

        [StringLength(100)]
        public string AGE { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE1 { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE2 { get; set; }

        public decimal? AGE_MA { get; set; }

        [StringLength(1000)]
        public string NOTES { get; set; }

        public int? SORT_ORDER { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(100)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string UPDATEBY { get; set; }

        [StringLength(5)]
        public string MAP_UNIT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CD_LITHO_STRAT> CD_LITHO_STRAT { get; set; }
    }
}
