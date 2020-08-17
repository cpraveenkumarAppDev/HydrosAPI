namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.LOG_EVENTS")]
    public partial class LOG_EVENTS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LOG_EVENTS()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Required]
        [StringLength(15)]
        public string WELL_ID { get; set; }

        [Required]
        [StringLength(15)]
        public string WELL_SOURCE { get; set; }

        [Required]
        [StringLength(5)]
        public string LOG_TYPE_CODE { get; set; }

        public DateTime? LOG_DATE { get; set; }

        [StringLength(5)]
        public string LOG_QUALITY_CODE { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(100)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string UPDATEBY { get; set; }

        [StringLength(500)]
        public string DRILLER_COMMENTS { get; set; }

        public virtual CD_LOG_QUALITY CD_LOG_QUALITY { get; set; }

        public virtual CD_LOG_TYPE CD_LOG_TYPE { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<LAYER> LAYERS { get; set; }
    }
}
