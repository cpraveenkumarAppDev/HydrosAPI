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
    public class CD_AW_COUNTY : Repository<CD_AW_COUNTY>
    {
        [Key]
        public string CODE { get; set; }

        public string DESCR { get; set; }
    }
}