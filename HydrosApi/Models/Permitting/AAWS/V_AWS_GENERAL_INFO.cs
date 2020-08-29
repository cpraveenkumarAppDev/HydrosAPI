using AdwrApi.Controllers.Permitting.AAWS;
using HydrosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdwrApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_GENERAL_INFO")]
    public class V_AWS_GENERAL_INFO
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

        [Column("PGM_DESCR")]
        public string ProgramDescription { get; set; }
        [Column("AMA_DESCR")]
        public string AMADescription { get; set; }
        [Column("PRIMARY_PROV_NAME")]
        public string PrimaryProviderName { get; set; }
        [NotMapped]
        public AWS_OVER_VIEW OverView { get; set; }
    }
}