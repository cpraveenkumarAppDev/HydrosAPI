namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;

    [Table("AWS.V_AWS_OAWS")]
    public class VAwsOaws : Repository<VAwsOaws>//V_AWS_OAWS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAwsOaws()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int? WaterRightFacilityId { get; set; }

        [Column("FAKE_ID")]
        public int? FakeId { get; set; }

        [Column("PCC")]
        public string ProgramCertificateConveyance { get; set; }

        [Column("APP_COMPLETE")]
        public string ApplicationComplete { get; set; }//APP_COMPLETE

        [Column("APP_ACCEPTED")]
        public string ApplicationAccepted { get; set; }//APP_ACCEPTED

        [Column("CORRECT_DT")]
        public DateTime? CorrectDate { get; set; }//CORRECT_DT

        [Column("CONVEYING_FILE")]
        public string ConveyingFile { get; set; } //CONVEY FROM PCC

        [Column("ORIGINAL_FILE")]
        public string OriginalFile { get; set; } 

        [Column("ORIGINAL_DATE")]
        public DateTime? OriginalDate { get; set; } //ISSUE DATE OF ORIGINAL FILE




    }
}