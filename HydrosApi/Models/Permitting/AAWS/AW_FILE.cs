using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("AWS.AW_FILE")]
    public class AW_FILE : Repository<AW_FILE>
    {
        [Column("WRF_ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WRF_ID { get; set; }

        [Column("CACY_CODE")]
        [StringLength(4)]
        public string CACY_CODE { get; set; }

        [Column("CACT_CODE")]
        [StringLength(4)]
        public string CACT_CODE { get; set; }

        [Column("CAPA_CODE")]
        [StringLength(4)]
        public string CAPA_CODE { get; set; }

        [Column("CASB_CODE")]
        [StringLength(4)]
        public string CASB_CODE { get; set; }

        [Column("ORIG_CERT_DATE")]
        public DateTime? ORIG_CERT_DATE { get; set; }

        [Column("PREV_SUBDIVISION")]
        [StringLength(120)]
        public string PREV_SUBDIVISION { get; set; }

        [Column("PREV_FILENUM")]
        [StringLength(15)]
        public string PREV_FILENUM { get; set; }

        [Column("NUM_OF_LOTS")]
        public int? NUM_OF_LOTS { get; set; }

        [Column("ANALYSIS_DWR_NUM")]
        [StringLength(15)]
        public string ANALYSIS_DWR_NUM { get; set; }

        [Column("OWNER_NAME")]
        [StringLength(2000)]
        public string OWNER_NAME { get; set; }

        [Column("PREV_FILENUM")]
        [StringLength(15)]
        public string PREV_FILENUM { get; set; }

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

        [Column("FILE_REVIEWER")]
        [StringLength(30)]
        public string FILE_REVIEWER { get; set; }
    }
}