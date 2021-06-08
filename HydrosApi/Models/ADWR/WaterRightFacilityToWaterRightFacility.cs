namespace HydrosApi.Models.ADWR
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
    using Data;


    [Table("RGR.WRF_WRF")]
    public class WaterRightFacilityToWaterRightFacility : Repository<WaterRightFacilityToWaterRightFacility>
    //WRF_WRF
    {
        [Key, Column("WRF_ID_FROM", Order = 0)]
        public int WaterRightFacilityIdFrom { get; set; }//WRF_ID_FROM

        [Key, Column("CWFT_CODE", Order = 1), StringLength(4)]
        public string RelationshipTypeCode { get; set; }//CWFT_CODE

        [Column("WRF_ID_TO")]
        public int WaterRightFacilityIdTo { get; set; }//WRF_ID_TO

        [Column("IS_ACTIVE"), StringLength(1)]
        public string IsActive { get; set; }//IS_ACTIVE

        [Column("CREATEBY"), StringLength(30)]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT

        [Column("UPDATEBY"), StringLength(30)]
        public string UpdateBy { get; set; }//UPDATEBY

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT

        [Column("AR_REPORTING_FLAG"), StringLength(1)]
        public string ArReportingFlag { get; set; }//AR_REPORTING_FLAG
    }
}