namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;
    using System.Collections.Generic;

    [Table("AWS.V_AWS_SUBBAS")]
    public class VAwsSubbasin : Repository<VAwsSubbasin>//V_AWS_SUBBAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAwsSubbasin()
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