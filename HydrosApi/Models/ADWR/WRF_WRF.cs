using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.WRF_WRF")]
    public class WRF_WRF
    {
        [Key, Column("WRF_ID_FROM", Order = 0)]
        public int WRF_ID_FROM { get; set; }

        [Key, Column("CWFT_CODE", Order = 1), StringLength(4)]
        public string CWFT_CODE { get; set; }

        [Column("WRF_ID_TO")]
        public int WRF_ID_TO { get; set; }

        [Column("IS_ACTIVE"), StringLength(1)]
        public string IS_ACTIVE { get; set; }

        [Column("CREATEBY"), StringLength(30)]
        public string CREATEBY { get; set; }

        [Column("CREATEDT")]
        public DateTime? CREATEDT { get; set; }

        [Column("UPDATEBY"), StringLength(30)]
        public string UPDATEBY { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UPDATEDT { get; set; }

        [Column("AR_REPORTING_FLAG"), StringLength(1)]
        public string AR_REPORTING_FLAG { get; set; }
    }
}