namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;

    [Table("AWS.V_AWS_PROVIDER")]
    public class VAwsProvider : Repository<VAwsProvider>//V_AWS_PROVIDER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAwsProvider()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROVIDER_WRF_ID")]
        public int? ProviderWaterRightFacilityId { get; set; }//PROVIDER_WRF_ID

        [Column("PROVIDER_NAME")]
        public string ProviderName { get; set; }//PROVIDER_NAME

        [Column("PROVIDER_PCC")]
        public string ProviderPCC { get; set; }//PROVIDER_PCC

        [Column("PROVIDER_AMA_CODE")]
        public string ProviderAmaCode { get; set; }//PROVIDER_AMA_CODE

        [Column("PROVIDER_PWSID")]
        public string ProviderPublicWaterSystemId { get; set; }//PWS_ID_Number
    }
}