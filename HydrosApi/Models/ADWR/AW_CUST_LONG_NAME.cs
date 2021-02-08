using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("AWS.AW_CUST_LONG_NAME")]
    public class AW_CUST_LONG_NAME : Repository<AW_CUST_LONG_NAME>
    {
        public int CUST_ID {get; set;}
        public string COMPANY_LONG_NAME { get; set; }
        public string CREATEBY { get; set; }
        public DateTime? CREATEDT { get; set; }
        public string UPDATEBY { get; set; }
        public DateTime? UPDATEDT { get; set; }
    }
}