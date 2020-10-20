namespace HydrosApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;
    using System.Collections.Generic;

    [Table("AWS.V_AWS_HYDRO")]
    public class V_AWS_HYDRO : Repository<V_AWS_HYDRO>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_HYDRO()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int WRFID { get; set; }
        [Column("PCC")]
        public string PCC { get; set; }
        [Column("SUBBASIN_CODE")]
        public string SUBBASIN_CODE { get; set; }
        [Column("SUBBASIN")]
        public string SUBBASIN { get; set; }
        [Column("AQUIFER")]
        public string AQUIFER { get; set; }
        [Column("ANALYSIS_METHOD")]
        public string ANALYSIS_METHOD { get; set; }
    }
}