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

        public VAwsWrfWrfDemand()
        {

        }

        [Column("WRF_ID")]
        public int? WaterRightFacilityId { get; set; }

        [Key, Column("REF_WRF_ID")]
        public int? ReferenceWaterRightFacilityId { get; set; }

        [Column("REF_PCC"), StringLength(14)]
        public string ReferencePCC { get; set; }

        [Column("WTR_DEMAND")]
        public decimal? WaterDemand { get; set; }


        [Column("AVAILABILITY_TYPE")]

        public string AvailabilityType { get; set; }

    }
}



    
