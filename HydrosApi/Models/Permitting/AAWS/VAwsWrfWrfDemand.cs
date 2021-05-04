namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;


    [Table("AWS.V_AWS_WRF_WRF_DEMAND")]
    public class VAwsWrfWrfDemand : Repository<VAwsWrfWrfDemand>
    {

        [Key, Column("WRF_ID")]
        public int? WaterRightFacilityId { get; set; } 

        [Key, Column("REF_WRF_ID")]
        public int? ReferenceWaterRightFacilityId { get; set; } 

        [Column("REF_PCC"), StringLength(14)]        
        public string ReferencePCC { get; set; } 

        [Column("WTR_DEMAND")]
        public decimal? WaterDemand { get; set; }

        [Key,NotMapped]
        public string DemandType => WaterDemand >= 0 ? "Source" : WaterDemand < 0 ? "Use" : null;

       



    }
}