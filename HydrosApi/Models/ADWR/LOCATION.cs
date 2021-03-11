using HydrosApi.Data;
using HydrosApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.LOCATION")]
    public class LOCATION : Repository<LOCATION>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("WRF_ID")]
        public int WRF_ID { get; set; }

        [Column("WHOLE_TOWNSHIP")]
        public int? WHOLE_TOWNSHIP { get; set; }

        [Column("WHOLE_RANGE")]
        public int? WHOLE_RANGE { get; set; }

        [Column("QUAD_CODE")]
        [StringLength(4)]
        public string QUAD_CODE { get; set; }

        [Column("HALF_TOWNSHIP")]
        [StringLength(1)]
        public string HALF_TOWNSHIP { get; set; }

        [Column("HALF_RANGE")]
        [StringLength(1)]
        public string HALF_RANGE { get; set; }

        [Column("SECTION")]
        [StringLength(10)]
        public string SECTION { get; set; }

        [Column("QTR_160_ACRE")]
        [StringLength(10)]
        public string QTR_160_ACRE { get; set; }

        [Column("QTR_40_ACRE")]
        [StringLength(10)]
        public string QTR_40_ACRE { get; set; }

        [Column("QTR_10_ACRE")]
        [StringLength(10)]
        public string QTR_10_ACRE { get; set; }

        [Column("QTR_2_ACRE")]
        [StringLength(10)]
        public string QTR_2_ACRE { get; set; }

        [Column("LOT_NO")]
        [StringLength(10)]
        public string LOT_NO { get; set; }

        [Column("IS_ACTIVE")]
        [StringLength(1)]
        public string IS_ACTIVE { get; set; }

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string COMMENTS { get; set; }

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CREATEBY { get; set; }

        [Column("CREATEDT")]
        public DateTime? CREATEDT { get; set; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UPDATEBY { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UPDATEDT { get; set; }

    }
}