namespace HydrosApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;

    [Table("AWS.V_AWS_OAWS")]
    public class V_AWS_OAWS : Repository<V_AWS_OAWS>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_OAWS()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int? WaterRightFacilityId { get; set; }

        [Column("PCC")]
        public string ProgramCertificateConveyance { get; set; }

        [Column("APP_COMPLETE")]
        public string APP_COMPLETE { get; set; }

        [Column("APP_ACCEPTED")]
        public string APP_ACCEPTED { get; set; }

        [Column("CORRECT_DT")]
        public DateTime? CORRECT_DT { get; set; }

    }
}