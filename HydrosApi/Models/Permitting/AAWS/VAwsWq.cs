namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;

    [Table("AWS.V_AWS_WQ")]
    public class VAwsWq : Repository<VAwsWq>//V_AWS_WQ
    {
        [Key]
        [Column("WRFID")]
        public int WaterRightFacilityId { get; set; }//WRFID

        [Column("PCC")]
        public string PCC { get; set; }

        [Column("CAPW_PWSID")]
        public string DeqPublicWaterSystemId { get; set; }//CAPW_PWSID

        [Column("NEW_PROVIDER")]
        public string NewProvider { get; set; }//NEW_PROVIDER

        [Column("SAFE_WTR")]
        public string SafeWater { get; set; }//SAFE_WTR

        [Column("CONTAMINATION")]
        public string Contamination { get; set; }//CONTAMINATION

        [Column("WQ_CHANGES")]
        public string WqChanges { get; set; }//WQ_CHANGES

        [Column("WQ_NOT_PROVEN")]
        public string WqNotProven { get; set; }//WQ_NOT_PROVEN

        [Column("MEET_DRINK_WTR_STD")]
        public string MeetsDrinkingWaterStandards { get; set; }//MEET_DRINK_WTR_STD

        [Column("MTGN_MGRT_ANALYSIS")]
        public string MitigationAndMigrationAnalysisSubmitted { get; set; }//MTGN_MGRT_ANALYSIS

    }
}