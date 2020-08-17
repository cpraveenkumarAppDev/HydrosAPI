namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.CD_DRILLERS_LOG")]
    public partial class CD_DRILLERS_LOG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_DRILLERS_LOG()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        [StringLength(10)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string DESCRIPTION { get; set; }

        [StringLength(25)]
        public string CATEGORY { get; set; }

        public short? SY { get; set; }

        public int? K { get; set; }

        public int? COLOR { get; set; }

        public byte? PATTERN { get; set; }

        public byte[] PATTERN_IMAGE { get; set; }

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
