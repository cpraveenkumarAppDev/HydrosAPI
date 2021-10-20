
namespace HydrosApi.Models.Adjudication
{
    using Data;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;


    [Table("ADJ_INV.IRRIGATION_DATA")]
    public partial class IrrigationData : AdwrRepository<IrrigationData>
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
        public decimal? WaterDuty { get; set; }

        [Column("REPORTED_VOLUME")]
        public decimal? ReportedVolume { get; set; }

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