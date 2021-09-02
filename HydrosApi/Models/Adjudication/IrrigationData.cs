using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HydrosApi.Models.Adjudication
{
    [Table("ADJ_INV.IRRIGATION_DATA")]
    public class IrrigationData : AdwrRepository<IrrigationData>
    {
        [Key, Column("ID")]       
        public int? Id { get; set; }

        [Column("PWR_ID")]
        public int? ProposedWaterRightId { get; set; }

        [Column("YR")]
        public int? Year { get; set; }

        [Column("CROP")]
        public string Crop { get; set; }

        [Column("WATER_DUTY")]
        public float? WaterDuty { get; set; }

        [Column("REPORTED_VOLUME")]
        public int? ReportedVolumne { get; set; }

        [Column("UNIT")]
        public string Unit { get; set; }

        [Column("REPORTED_BY")]
        public string ReportedBy { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("CREATEBY")]
        public string CreateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }

        [NotMapped]

        public bool? DeleteRecord { get; set; }
    }
}