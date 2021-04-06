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
    public class Location : Repository<Location>//LOCATION
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//ID

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Column("WHOLE_TOWNSHIP")]
        public int? WholeTownship { get; set; }//WHOLE_TOWNSHIP

        [Column("WHOLE_RANGE")]
        public int? WholeRange { get; set; }//WHOLE_RANGE

        [Column("QUAD_CODE")]
        [StringLength(4)]
        public string QuadCode { get; set; }//QUAD_CODE

        [Column("HALF_TOWNSHIP")]
        [StringLength(1)]
        public string HalfTownship { get; set; }//HALF_TOWNSHIP

        [Column("HALF_RANGE")]
        [StringLength(1)]
        public string HalfRange { get; set; }//HALF_RANGE

        [Column("SECTION")]
        [StringLength(10)]
        public string Section { get; set; }//SECTION

        [Column("QTR_160_ACRE")]
        [StringLength(10)]
        public string Qtr160Acre { get; set; }//QTR_160_ACRE

        [Column("QTR_40_ACRE")]
        [StringLength(10)]
        public string Qtr40Acre { get; set; }//QTR_40_ACRE

        [Column("QTR_10_ACRE")]
        [StringLength(10)]
        public string Qtr10Acre { get; set; }//QTR_10_ACRE

        [Column("QTR_2_ACRE")]
        [StringLength(10)]
        public string Qtr2Acre { get; set; }//QTR_2_ACRE

        [Column("LOT_NO")]
        [StringLength(10)]
        public string LotNo { get; set; }//LOT_NO

        [Column("IS_ACTIVE")]
        [StringLength(1)]
        public string IsActive { get; set; }//IS_ACTIVE

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string Comments { get; set; }//COMMENTS

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