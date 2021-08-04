using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_WELL_SERVING")]
    public class VAwsWellServing : Repository<VAwsWellServing>//V_AWS_WELL_SERVING
    {
        [Key, Column("ID")]
        public int Id { get; set; }//ID

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Column("WELL_REGISTRY_ID")]
        [StringLength(6)]
        public string WellRegistryId { get; set; }//WELL_REGISTRY_ID

        [Column("PT_PERMIT_NUMBER")]
        public int? PermitNumber { get; set; }//PT_PERMIT_NUMBER

        [Column("ACRE_FEET_ANNUM")]
        public int? AcreFeetAnnualNumber { get; set; }//ACRE_FEET_ANNUM

        [Column("PRMT_CODE")]
        [StringLength(2)]
        public string PermitCode { get; set; }//PRMT_CODE

        [Column("PERMIT_CODE_DESCR")]
        [StringLength(80)]
        public string PermitCodeDescription { get; set; }//PERMIT_CODE_DESCR

        [Column("CADASTRAL")]
        [StringLength(20)]
        public string Cadastral { get; set; }//CADASTRAL

        [Column("PCC")]
        [StringLength(20)]
        public string PCC { get; set; }

        [NotMapped]
        public string PermitCodeDescriptionLower
        {
            get
            {
                if(PermitCodeDescription != null)
                {
                    PermitCodeDescription = QueryResult.TitleFormat(PermitCodeDescription);
                    return PermitCodeDescription;

                }

                return null;
            }

            set
            {
                this.PermitCodeDescriptionLower = value;
            }
        }
    }
}