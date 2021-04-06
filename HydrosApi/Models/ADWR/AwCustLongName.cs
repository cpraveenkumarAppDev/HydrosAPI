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
    public class AwCustLongName : Repository<AwCustLongName>//AW_CUST_LONG_NAME
    {


        // [Key, Column("CUST_ID"),DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("CUST_ID")]
        public int CustId {get; set; }//CUST_ID

        [Key]
        [Column("COMPANY_LONG_NAME")]
        [StringLength(2090)]
        public string CompanyLongName { get; set; }//COMPANY_LONG_NAME


        [Column("CREATEBY")]
        public string CreateBy { get; set; }//CREATEBY
        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT
        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }//UPDATEBY
        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT
    }
}