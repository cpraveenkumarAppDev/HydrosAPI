namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_TERTIARY_AQUIFER")]
    public partial class CD_TERTIARY_AQUIFER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_TERTIARY_AQUIFER()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        [StringLength(10)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string NAME { get; set; }

        [StringLength(100)]
        public string DESCRIPTION { get; set; }

        public short? STRATIGRAPHIC_POS { get; set; }

        [StringLength(10)]
        public string USGS_CODE { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(5)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(5)]
        public string UPDATEBY { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<LAYER> LAYERS { get; set; }
    }
}
