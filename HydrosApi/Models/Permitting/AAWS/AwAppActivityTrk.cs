using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_APP_ACTIVITY_TRK")]
    public class AwAppActivityTrk : Repository<AwAppActivityTrk>//AW_APP_ACTIVITY_TRK
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//ID

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Column("CAAA_CODE")]
        public string ActivityCode { get; set; }

        [Column("ACT_TRK_DT_TIME")]
        public DateTime? ActivityTrackDate { get; set; }//ACT_TRK_DT_TIME

        [Column("CREATEBY")]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime CreateDt  { get; set; }//CREATEDT
    }
}