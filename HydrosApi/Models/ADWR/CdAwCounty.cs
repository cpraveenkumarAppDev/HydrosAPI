using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("AWS.CD_AW_COUNTY")]
    public class CdAwCounty : Repository<CdAwCounty>//CD_AW_COUNTY
    {
        [Key]
        [Column("CODE")]
        public string Code { get; set; }//CODE

        [Column("DESCR")]
        public string Description { get; set; }//DESCR
    }
}