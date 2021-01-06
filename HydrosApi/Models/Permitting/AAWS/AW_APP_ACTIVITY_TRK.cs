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
    public class AW_APP_ACTIVITY_TRK : Repository<AW_APP_ACTIVITY_TRK>
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column("WRF_ID")]
        public int WRF_ID { get; set; }
        [Column("CAAA_CODE")]
        public string ActivityCode { get; set; }
        [Column("ACT_TRK_DT_TIME")]
        public DateTime? ACT_TRK_DT_TIME { get; set; }
        [Column("CREATEBY")]
        public string CREATEBY { get; set; }
        [Column("CREATEDT")]
        public DateTime CREATEDT  { get; set; }
    }
}