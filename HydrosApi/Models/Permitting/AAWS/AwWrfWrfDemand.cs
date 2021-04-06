namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System.Collections.Generic;
    using System.Linq;
    using Models.ADWR;


    [Table("AWS.AW_WRF_WRF_DEMAND")]
    public class AwWrfWrfDemand : Repository<AwWrfWrfDemand>//AW_WRF_WRF_DEMAND
    {       
        
        [Key,Column("WRF_ID_TO")]
        public int WaterRightFacilityIdTo { get; set; }//WRF_ID_TO

        [Column("WRF_ID_FROM")]
        public int WaterRightFacilityIdFrom { get; set; }//WRF_ID_FROM

        [Column("WTR_DEMAND")]
        public decimal? WaterDemand { get; set; }//WTR_DEMAND

        [NotMapped, StringLength(14)]
        public string AssociatedPCC { get; set; }//ASSOCIATED_PCC

        [NotMapped, StringLength(20)]
        public string AvailabilityType { get; set; }//AVAILABILITY_TYPE


    }
}