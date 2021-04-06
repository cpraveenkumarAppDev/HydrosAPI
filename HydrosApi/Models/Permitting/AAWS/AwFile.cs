using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("AWS.AW_FILE")]
    public class AwFile : Repository<AwFile>//AW_FILE
    {
        [Column("WRF_ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Column("CACY_CODE")]
        [StringLength(4)]
        public string CountyCode { get; set; }//CACY_CODE

        [Column("CACT_CODE")]
        [StringLength(4)]
        public string CityCode { get; set; }//CACT_CODE

        [Column("CAPA_CODE")]
        [StringLength(4)]
        public string PlattingAuthorityCode { get; set; }//CAPA_CODE

        [Column("CASB_CODE")]
        [StringLength(4)]
        public string SubbasinCode { get; set; }//CASB_CODE

        [Column("ORIG_CERT_DATE")]
        public DateTime? OriginalCertificateDate { get; set; }//ORIG_CERT_DATE

        [Column("PREV_SUBDIVISION")]
        [StringLength(120)]
        public string PreviousSubdivision { get; set; }//PREV_SUBDIVISION

        [Column("PREV_FILENUM")]
        [StringLength(15)]
        public string PreviousFileNumber { get; set; }//PREV_FILENUM

        [Column("NUM_OF_LOTS")]
        public int? NumberOfLots { get; set; }//NUM_OF_LOTS

        [Column("ANALYSIS_DWR_NUM")]
        [StringLength(15)]
        public string AnalysisDwrNumber { get; set; }//ANALYSIS_DWR_NUM

        [Column("OWNER_NAME")]
        [StringLength(2000)]
        public string OwnerName { get; set; }//OWNER_NAME

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

        [Column("FILE_REVIEWER")]
        [StringLength(30)]
        public string FileReviewer { get; set; }//FILE_REVIEWER
    }
}