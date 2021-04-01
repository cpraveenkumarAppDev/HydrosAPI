namespace HydrosApi.Models.Permitting.AAWS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    using Data;

    [Table("AWS.V_AWS_HYDRO")]
    public class V_AWS_HYDRO : Repository<V_AWS_HYDRO>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_HYDRO()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int WRFID { get; set; }
        [Column("PCC")]
        public string PCC { get; set; }
        [Column("SUBBASIN_CODE")]
        public string SUBBASIN_CODE { get; set; }
        [Column("SUBBASIN")]
        public string SUBBASIN { get; set; }
        [Column("AQUIFER")]
        public string AQUIFER { get; set; }
        [Column("ANALYSIS_METHOD")]
        public string ANALYSIS_METHOD { get; set; }

        [Column("WATER_LEVEL")]
        public string WATER_LEVEL { get; set; }
        [Column("WL_CHANGE")]
        public string WL_CHANGE { get; set; }
        [Column("SATURATED_THICKNESS")]
        public string SATURATED_THICKNESS { get; set; }
        [Column("WL_CHANGE_100YR")]
        public decimal? WL_CHANGE_100YR { get; set; }
        [Column("MAX_GWDEPTH_100YR")]
        public decimal? MAX_GWDEPTH_100YR { get; set; }
        [Column("MAX_LOC_100YR")]
        public string MAX_LOC_100YR { get; set; }
        [Column("SW_SOURCE")]
        public string SW_SOURCE { get; set; }
        [Column("FIRM_YIELD")]
        public decimal? FIRM_YIELD { get; set; }
        [Column("MEDIAN_FLOW")]
        public decimal? MEDIAN_FLOW { get; set; }
        [Column("PHY_AVAIL_BASEDON_PAD")]
        public string PHY_AVAIL_BASEDON_PAD { get; set; }
        [Column("PAD_REFERENCE_FILE")]
        public string PAD_REFERENCE_FILE { get; set; }
        [Column("PHY_AVAIL_BASEDON_ANALYSIS")]
        public string PHY_AVAIL_BASEDON_ANALYSIS { get; set; }
        [Column("ANALYSIS_ISSUED_YEAR")]
        public string ANALYSIS_ISSUED_YEAR { get; set; }
        [Column("ORIG_WTR_PHY_AVAIL")]
        public decimal? ORIG_WTR_PHY_AVAIL { get; set; }
        [Column("BAL_WTR_PHY_AVAIL")]
        public decimal? BAL_WTR_PHY_AVAIL { get; set; }
        [Column("HYDRO_STUDY_INCLUDED")]
        public string HYDRO_STUDY_INCLUDED { get; set; }
        [Column("HYDRO_DATA_ON_FILE")]
        public string HYDRO_DATA_ON_FILE { get; set; }
        [Column("TRANSMISSIVITY")]
        public string TRANSMISSIVITY { get; set; }
        [Column("SPECIFIC_YIELD")]
        public string SPECIFIC_YIELD { get; set; }
        [Column("GW_STORAGE")]
        public string GW_STORAGE { get; set; }
        [Column("RECHARGE")]
        public string RECHARGE { get; set; }
        [Column("GW_FLUX")]
        public string GW_FLUX { get; set; }
        [Column("MIN_GWDEPTH_100YR")]
        public decimal? MIN_GWDEPTH_100YR { get; set; }
        [Column("MIN_LOC_100YR")]
        public string MIN_LOC_100YR { get; set; }
        [Column("RECHARGE_IMPACT")]
        public string RECHARGE_IMPACT { get; set; }
        [Column("CRITERIA_MET_RECHARGE")]
        public string CRITERIA_MET_RECHARGE { get; set; }

        [Column("REGISTERED_GEOLOGIST_REQ")]
        public string REGISTERED_GEOLOGIST_REQ { get; set; }

        [Column("HYDRO_STUDY_SUBMITTED")]
        public DateTime? HYDRO_STUDY_SUBMITTED { get; set; }

        [Column("IMPACT_ANALYSIS_SUBMITTED")]
        public DateTime? IMPACT_ANALYSIS_SUBMITTED { get; set; }

    }
}