using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_WELL_SERVING")]
    public class V_AWS_WELL_SERVING : Repository<V_AWS_WELL_SERVING>
    {
        [Key, Column("ID")]
        public int ID { get; set; }

        [Column("WRF_ID")]
        public int WRF_ID { get; set; }

        [Column("WELL_REGISTRY_ID")]
        [StringLength(6)]
        public string WELL_REGISTRY_ID { get; set; }

        [Column("PT_PERMIT_NUMBER")]
        public int? PT_PERMIT_NUMBER { get; set; }

        [Column("ACRE_FEET_ANNUM")]
        public int? ACRE_FEET_ANNUM { get; set; }

        [Column("PRMT_CODE")]
        [StringLength(2)]
        public string PRMT_CODE { get; set; }

        [Column("PERMIT_CODE_DESCR")]
        [StringLength(80)]
        public string PERMIT_CODE_DESCR { get; set; }

        [Column("CADASTRAL")]
        [StringLength(20)]
        public string CADASTRAL { get; set; }

        [Column("PCC")]
        [StringLength(20)]
        public string PCC { get; set; }
    }
}