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
    [Table("AWS.CD_AW_CITY")]
    public class CdAwCity : Repository<CdAwCity>//CD_AW_CITY
    {
        [Key, Column("CODE"), StringLength(4)]
        public string Code { get; set; }//CODE

        [Column("DESCR"), StringLength(40)]
        public string Description { get; set; }//DESCR

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }//UPDATEBY

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT
    }
}