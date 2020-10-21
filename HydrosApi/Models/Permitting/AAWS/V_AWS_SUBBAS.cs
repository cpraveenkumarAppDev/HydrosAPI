namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;
    using System.Collections.Generic;

    [Table("AWS.V_AWS_SUBBAS")]
    public class V_AWS_SUBBAS : Repository<V_AWS_SUBBAS>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_SUBBAS()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CODE")]
        public string BasinCode { get; set; }
        [Column("BASIN")]
        public string Basin { get; set; }
        [Column("DESCR")]
        public string BasinDescription { get; set; }
    }
}