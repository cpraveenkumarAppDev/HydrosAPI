namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_LITHO_STRAT")]
    public partial class CD_LITHO_STRAT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_LITHO_STRAT()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        [StringLength(10)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string AGE { get; set; }

        [StringLength(5)]
        public string MU_CODE { get; set; }

        [StringLength(8)]
        public string GU_CODE { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE1 { get; set; }

        [StringLength(100)]
        public string ROCK_TYPE2 { get; set; }

        [StringLength(1000)]
        public string NOTES { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(100)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string UPDATEBY { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }

        public virtual CD_GEO_UNITS CD_GEO_UNITS { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<LAYER> LAYERS { get; set; }

        public virtual CD_MAP_UNITS CD_MAP_UNITS { get; set; }
    }
}
