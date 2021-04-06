using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.CD_CUST_TYPE")]
    public class CdCustType : Repository<CdCustType>//CD_CUST_TYPE
    {
        [Key, Column("CODE"), StringLength(4)]
        public string Code { get; set; }//CODE

        [Column("DESCR"), StringLength(40)]
        public string Description { get; set; }//DESCR
    }
}