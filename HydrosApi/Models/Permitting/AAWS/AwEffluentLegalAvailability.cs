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
    [Table("AWS.AW_EFFLUENT_LEGAL_AVAILABILITY")]
    public class AwEffluentLegalAvailability : Repository<AwEffluentLegalAvailability>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }

        [Column("EFF_TYPE")]
        [StringLength(20)]
        public string EffluentType { get; set; }

        [Column("CONTRACT_NAME")]
        [StringLength(50)]
        public string ContractName { get; set; }

        [Column("AMOUNT")]
        public decimal? Amount { get; set; }

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CreateBy { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }
    }
}