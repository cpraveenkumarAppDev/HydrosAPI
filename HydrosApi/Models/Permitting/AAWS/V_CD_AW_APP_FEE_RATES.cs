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
    public class V_CS_AW_APP_FEE_RATES : Repository<V_CS_AW_APP_FEE_RATES>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROGRAM_CODE")]
        public int PROGRAM_CODE { get; set; }

        [Column("PROGRAM_DESCR")]
        public string PROGRAM_DESCR { get; set; }

        [Column("ASSURED_OR_ADEQUATE_CODE")]
        public string ASSURED_OR_ADEQUATE_CODE { get; set; }
    }
}