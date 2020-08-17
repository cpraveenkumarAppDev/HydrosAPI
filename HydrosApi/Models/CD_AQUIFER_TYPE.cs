namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_AQUIFER_TYPE")]
    public partial class CD_AQUIFER_TYPE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_AQUIFER_TYPE()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        [StringLength(3)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string DESCRIPTION { get; set; }

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
