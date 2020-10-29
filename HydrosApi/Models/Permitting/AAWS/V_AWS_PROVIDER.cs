namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;

    [Table("AWS.V_AWS_PROVIDER")]
    public class V_AWS_PROVIDER : Repository<V_AWS_PROVIDER>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_PROVIDER()
        {
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROVIDER_WRF_ID")]
        public int? PROVIDER_WRF_ID { get; set; }
        [Column("PROVIDER_NAME")]
        public string PROVIDER_NAME { get; set; }
        [Column("PROVIDER_PCC")]
        public string PROVIDER_PCC { get; set; }
        [Column("PROVIDER_AMA_CODE")]
        public string PROVIDER_AMA_CODE { get; set; }

        [Column("PROVIDER_PWSID")]
        public string PWS_ID_Number { get; set; }
    }
}