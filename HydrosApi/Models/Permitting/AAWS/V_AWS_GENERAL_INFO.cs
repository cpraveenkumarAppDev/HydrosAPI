namespace HydrosApi.Models 
{    
 
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;

    [Table("AWS.V_AWS_GENERAL_INFO")]
    public class V_AWS_GENERAL_INFO : Repository<V_AWS_GENERAL_INFO>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_GENERAL_INFO()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int? WaterRightFacilityId { get; set; }

        [Column("PCC")]
        public string ProgramCertificateConveyance { get; set; }

        [Column("SUBDIVISION_NAME")]
        public string Subdivision { get; set; }

        [Column("PROGRAM_CODE")]
        public string ProgramCode { get; set; }
        [Column("FILE_REVIEWER")]
        public string FileReviewer { get; set; }
        [Column("SECONDARY_PROV_SYS")] //replace once LTFDaysRemaining is added to the view
        public string LTFDaysRemaining { get; set; }
        [Column("APP_STATUS_DESCR")]
        public string Status { get; set; }
        [Column("APP_STATUS_DT")]
        public DateTime? StatusDate { get; set; }
        [Column("SECONDARY_PROV_NAME")]
        public string SecondaryProviderName { get; set; }
        [Column("PRIMARY_PROV_NAME")]
        public string PrimaryProviderName { get; set; }
        [Column("COMPLETE_CORRECT_DT")]
        public DateTime? Complete_Correct { get; set; }
        [Column("RECEIVEDDT")]
        public DateTime? Date_Accepted { get; set; }
    }
}
