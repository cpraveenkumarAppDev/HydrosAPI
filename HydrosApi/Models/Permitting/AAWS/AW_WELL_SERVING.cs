using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_WELL_SERVING")]
    public class AW_WELL_SERVING : Repository<AW_WELL_SERVING>
    {
        [Key, Column("WRF_ID", Order = 0)]
        public int WRF_ID { get; set; }

        [Key, Column("WELL_REGISTRY_ID", Order = 1)]
        [StringLength(6)]
        public string WELL_REGISTRY_ID { get; set; }

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