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
        [Column("AMA_DESCR")]
        public string AMA { get; set; }
        [Column("CAMA_CODE")]
        public string Cama_code { get; set; }
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

        [Column("HYDROLOGY")]
        public string Hydrology { get; set; }
        [Column("LEGAL_AVAILABILITY")]
        public string Legal_Availability { get; set; }

        [Column("SECONDARY_PROV_NAME")]
        public string SecondaryProviderName { get; set; }
        [Column("USER_NAME")]
        public string UserName { get; set; }
        [Column("PRIMARY_PROV_WRFID")]
        public int? PrimaryProviderWrfId { get; set; }
        [Column("PRIMARY_PROV_NAME")]
        public string PrimaryProviderName { get; set; }
        [Column("COMPLETE_CORRECT_DT")]
        public DateTime? Complete_Correct { get; set; }
        [Column("RECEIVEDDT")]
        public DateTime? Date_Accepted { get; set; }
        [Column("FIRST_NOTICEDT")]
        public DateTime? First_Notice_Date { get; set; }
        [Column("SEC_NOTICEDT")]
        public DateTime? Second_Notice_Date { get; set; }
        [Column("PUB_COMM_ENDDT")]
        public DateTime? Final_Date_for_Public_Comment { get; set; }

        [Column("PHYSICAL_AVAILABILITY")]
        public string Physical_Availability { get; set; }
        [Column("CONTINUOUS_AVAILABILITY")]
        public string Continuous_Availability { get; set; }
        [Column("CONSISTENCY_WITH_MGMT_PLAN")]
        public string Consistency_With_Mgmt_Plan { get; set; }
        [Column("CONSISTENCY_WITH_MGMT_GOAL")]
        public string Consistency_With_Mgmt_Goal { get; set; }
        [Column("FINANCIAL_CAPABILITY")]
        public string Financial_Capability { get; set; }
        [Column("OWNERSHIP_DOCUMENTS")]
        public string Ownership_Documents { get; set; }
        [Column("DEMAND_CALCULATOR")]
        public string Demand_Calculator { get; set; }
        [Column("OTHER")]
        public string Other { get; set; }
        [Column("WATER_QUALITY")]
        public string Water_Quality { get; set; }
        [Column("DESIGNATION_TERM")]
        public string Designation_Term { get; set; }

        [NotMapped] 
        public string ProcessStatus { get; set; } //Use this for error messages in stored procedure or api calls
    }
}
