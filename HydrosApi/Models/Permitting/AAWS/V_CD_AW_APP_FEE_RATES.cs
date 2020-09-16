using HydrosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdwrApi.Models.Permitting.AAWS
{
    [Table("AWS.V_CD_AW_APP_FEE_RATES")]
    public class V_CD_AW_APP_FEE_RATES : Repository<V_CD_AW_APP_FEE_RATES>
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROGRAM_CODE")]
        public int PROGRAM_CODE { get; set; }

        [Column("PROGRAM_DESCR")]
        public string PROGRAM_DESCR { get; set; }

        [Column("ASSURED_OR_ADEQUATE_CODE")]
        public string ASSURED_OR_ADEQUATE_CODE { get; set; }

        [Column("BASIC_FEE")]
        public int BASIC_FEE { get; set; }

        [Column("ADD_FEE_RATE")]
        public int ADD_FEE_RATE { get; set; }

        [Column("SUBSTRACTOR")]
        public int SUBSTRACTOR { get; set; }

        [Column("MAX_FEE")]
        public int MAX_FEE { get; set; }

        [Column("LOT_AF")]
        public string LOT_AF { get; set; }

        [Column("NO_FEE_AFTER_0906")]
        public string NO_FEE_AFTER_0906 { get; set; }

        [Column("PUB_NOTICE_FEE")]
        public string PUB_NOTICE_FEE { get; set; }

    }
}