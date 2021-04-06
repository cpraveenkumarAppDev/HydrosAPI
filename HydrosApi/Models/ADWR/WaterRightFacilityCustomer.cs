using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.WRF_CUST")]
    public class WaterRightFacilityCustomer : Repository<WaterRightFacilityCustomer>//WRF_CUST
    {
        [Key, Column("CUST_ID", Order = 1)]
        public int CustomerId { get; set; }//CUST_ID

        [Key, Column("WRF_ID", Order = 0)]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Key, Column("CCT_CODE", Order = 2)]
        [StringLength(4)]
        public string CustomerTypeCode { get; set; }//CCT_CODE

        [Key, Column("LINE_NUM", Order = 3)]
        public int LineNum { get; set; }//LINE_NUM

        [Column("IS_ACTIVE")]
        [StringLength(1)]
        public string IsActive { get; set; }//IS_ACTIVE

        [Column("PRIMARY_MAILING_ADDRESS")]
        [StringLength(1)]
        public string PrimaryMailingAddress { get; set; }//PRIMARY_MAILING_ADDRESS

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