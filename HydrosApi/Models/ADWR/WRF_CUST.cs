using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.WRF_CUST")]
    public class WRF_CUST : Repository<WRF_CUST>
    {
        [Key, Column("CUST_ID", Order = 1)]
        public int CUST_ID { get; set; }

        [Key, Column("WRF_ID", Order = 0)]
        public int WRF_ID { get; set; }

        [Column("CCT_CODE")]
        [StringLength(4)]
        public string CCT_CODE { get; set; }

        [Column("LINE_NUM")]
        public int LINE_NUM { get; set; }

        [Column("IS_ACTIVE")]
        [StringLength(1)]
        public string IS_ACTIVE { get; set; }

        [Column("PRIMARY_MAILING_ADDRESS")]
        [StringLength(1)]
        public string PRIMARY_MAILING_ADDRESS { get; set; }

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CREATEBY { get; set; }

        [Column("CREATEDT")]
        public DateTime? CREATEDT { get; set; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UPDATEBY { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UPDATEDT { get; set; }
    }
}