namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Runtime.InteropServices;
    using Data;


    [Table("AWS.V_AWS_WRF_WRF_DEMAND")]
    public class VAwsWrfWrfDemand : Repository<VAwsWrfWrfDemand>
    {

        [Column("WRF_ID")]
        public int? WaterRightFacilityId { get; set; }

        [Key, Column("REF_WRF_ID")]
        public int? ReferenceWaterRightFacilityId { get; set; }

        [Column("REF_PCC"), StringLength(14)]
        public string ReferencePCC { get; set; }

        [Column("WTR_DEMAND")]
        public decimal? WaterDemand { get; set; }

        [NotMapped]
        public string DemandType => WaterDemand >= 0 ? "Source" : WaterDemand < 0 ? "Use" : null;
       
        [NotMapped]
        public long? DerivedId => WaterRightFacilityId != null && ReferenceWaterRightFacilityId != null ?
            (long?)long.Parse(String.Format("{0}{1}{2}", WaterDemand > 0 ? 1 : WaterDemand < 0 ? -1 : 0, WaterRightFacilityId, ReferenceWaterRightFacilityId)) : null;


    }
}