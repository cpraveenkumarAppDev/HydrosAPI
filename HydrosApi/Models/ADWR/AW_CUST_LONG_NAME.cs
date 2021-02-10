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
         
        
       // [Key, Column("CUST_ID"),DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int CUST_ID {get; set;}

        [Key]
        [Column("COMPANY_LONG_NAME")]
        [StringLength(2090)]
        public string COMPANY_LONG_NAME { get; set; }


        [Column("CREATEBY")]
        public string CREATEBY { get; set; }
        [Column("CREATEDT")]
        public DateTime? CREATEDT { get; set; }
        [Column("UPDATEBY")]
        public string UPDATEBY { get; set; }
        [Column("UPDATEDT")]
        public DateTime? UPDATEDT { get; set; }
    }
}