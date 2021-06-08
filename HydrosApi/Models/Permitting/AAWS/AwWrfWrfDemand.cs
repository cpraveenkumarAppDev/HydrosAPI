namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System;
    


    [Table("AWS.AW_WRF_WRF_DEMAND")]
    public class AwWrfWrfDemand : Repository<AwWrfWrfDemand>//AW_WRF_WRF_DEMAND
    {

        [Key, Column("WRF_ID_TO", Order = 0)]
        public int WaterRightFacilityIdTo { get; set; }//WRF_ID_TO

        [Key, Column("WRF_ID_FROM", Order = 1)]
        public int WaterRightFacilityIdFrom { get; set; }//WRF_ID_FROM

        [Column("WTR_DEMAND")]
        public decimal? WaterDemand { get; set; }//WTR_DEMAND

        [Key, Column("CWFT_CODE", Order = 2), StringLength(4)]
        public string RelationshipTypeCode { get; set; }//CWFT_CODE

        [Column("CREATEBY"), StringLength(30)]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT

        [Column("UPDATEBY"), StringLength(30)]
        public string UpdateBy { get; set; }//UPDATEBY

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT

        [NotMapped, StringLength(20)]
        public string AssociatedPCC { get; set; }//ASSOCIATED_PCC

        [NotMapped, StringLength(20)]
        public string AvailabilityType { get; set; }//AVAILABILITY_TYPE

    }
}