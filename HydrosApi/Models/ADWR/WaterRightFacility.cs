using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.WTR_RIGHT_FACILITY")]
    public class WaterRightFacility : Repository<WaterRightFacility>//WTR_RIGHT_FACILITY
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }//ID

        [Column("CAMA_CODE")]
        public string ActiveManagementAreaCode { get; set; }//AmaCode

        [Column("CFST_CODE")]
        public string FileStatusCode { get; set; }

        [Column("PROGRAM_CODE")]
        public string Program { get; set; }

        [Column("CERT_NO")]
        public string Certificate { get; set; }

        [Column("CONV_NO")]
        public string Conveyance { get; set; }

        [Column("ACC_DOCKET_NO")]
        public string AzCorpComDocketNumber { get; set; }

        [Column("ACC_DOCKET_DT")]
        public DateTime? AzCorpComDocketDate { get; set; }

        [Column("ACC_COMPLIANCE")]
        public string AzCorpCompliance { get; set; }

        [Column("ACC_COMPLIANCE_STATUS_DATE")]
        public DateTime? AzCorpComplianceStatusDate { get; set; }

        [Column("NAME")]
        public string WaterRightFacilityName { get; set; }

        public string PCC { get => $"{this.Program}-{this.Certificate}.{this.Conveyance}"; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

    }
}