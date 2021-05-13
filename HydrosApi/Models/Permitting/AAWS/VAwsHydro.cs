namespace HydrosApi.Models.Permitting.AAWS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema; 
    using Data;
    using System.Text.RegularExpressions;

    [Table("AWS.V_AWS_HYDRO")]
    public class VAwsHydro : Repository<VAwsHydro>//V_AWS_HYDRO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAwsHydro()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int WaterRightFacilityId { get; set; }//WRFID

        [Column("PCC")]
        public string PCC { get; set; }

        [Column("SUBBASIN_CODE")]
        public string SubbasinCode { get; set; }//SUBBASIN_CODE

        [Column("SUBBASIN")]
        public string Subbasin { get; set; }//SUBBASIN

        [Column("AQUIFER")]
        public string Aquifer { get; set; }//AQUIFER

        [Column("ANALYSIS_METHOD")]
        public string AnalysisMethod { get; set; }//ANALYSIS_METHOD

        [Column("WATER_LEVEL")]
        public string WaterLevel { get; set; }//WATER_LEVEL

        [Column("WL_CHANGE")]
        public string WaterLevelChange { get; set; }//WL_CHANGE

        [Column("SATURATED_THICKNESS")]
        public string SaturatedThickness { get; set; }//SATURATED_THICKNESS

        [Column("WL_CHANGE_100YR")]
        public decimal? WaterLevelChange100YR { get; set; }//WL_CHANGE_100YR

        [Column("MAX_GWDEPTH_100YR")]
        public decimal? MaxGroundwaterDepth100YR { get; set; }//MAX_GWDEPTH_100YR

        [Column("MAX_LOC_100YR")]
        public string MaxLocation100YR { get; set; }//MAX_LOC_100YR

        [Column("SW_SOURCE")]
        public string SurfaceWaterSource { get; set; }//SW_SOURCE

        [Column("FIRM_YIELD")]
        public decimal? FirmYield { get; set; }//FIRM_YIELD

        [Column("MEDIAN_FLOW")]
        public decimal? MedianFlow { get; set; }//MEDIAN_FLOW

        [Column("PHY_AVAIL_BASEDON_PAD")]
        public string PhysAvailBasedOnPrevIssPhysAvailDem { get; set; }//PHY_AVAIL_BASEDON_PAD

        [Column("PAD_REFERENCE_FILE")]
        public string ProofOfPhysAvailReferenceFile { get; set; }//PAD_REFERENCE_FILE

        [Column("PHY_AVAIL_BASEDON_ANALYSIS")]
        public string PhysAvailBasedOnPrevAnalysis { get; set; }//PHY_AVAIL_BASEDON_ANALYSIS

        [Column("ANALYSIS_ISSUED_YEAR")]
        public string AnalysisIssuedYear { get; set; }//ANALYSIS_ISSUED_YEAR

        [Column("ORIG_WTR_PHY_AVAIL")]
        public decimal? OriginalWaterPhysAvail { get; set; }//ORIG_WTR_PHY_AVAIL

        [Column("BAL_WTR_PHY_AVAIL")]
        public decimal? BalanceWaterPhysAvail { get; set; }//BAL_WTR_PHY_AVAIL

        [Column("HYDRO_STUDY_INCLUDED")]
        public string HydroStudyIncluded { get; set; }//HYDRO_STUDY_INCLUDED

        [Column("HYDRO_DATA_ON_FILE")]
        public string HydroDataOnFile { get; set; }//HYDRO_DATA_ON_FILE

        [Column("TRANSMISSIVITY")]
        public string Transmissivity { get; set; }//TRANSMISSIVITY

        [Column("SPECIFIC_YIELD")]
        public string SpecificYield { get; set; }//SPECIFIC_YIELD

        [Column("GW_STORAGE")]
        public string GroundwaterStorage { get; set; }//GW_STORAGE

        [Column("RECHARGE")]
        public string Recharge { get; set; }//RECHARGE

        [Column("GW_FLUX")]
        public string GroundwaterFlux { get; set; }//GW_FLUX

        [Column("MIN_GWDEPTH_100YR")]
        public decimal? MinGroundwaterDepth100Yr { get; set; }//MIN_GWDEPTH_100YR

        [Column("MIN_LOC_100YR")]
        public string MinLocation100Yr { get; set; }//MIN_LOC_100YR

        [Column("RECHARGE_IMPACT")]
        public string RechargeImpact { get; set; }//RECHARGE_IMPACT

        [Column("CRITERIA_MET_RECHARGE")]
        public string CriteriaMetRecharge { get; set; }//CRITERIA_MET_RECHARGE

        [Column("REGISTERED_GEOLOGIST_REQ")]
        public string RegisteredGeologistRequired { get; set; }//REGISTERED_GEOLOGIST_REQ

        [Column("HYDRO_STUDY_SUBMITTED")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime? HydroStudySubmitted { get; set; }//HYDRO_STUDY_SUBMITTED

        [Column("IMPACT_ANALYSIS_SUBMITTED")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ImpactAnalysisSubmitted { get; set; }//IMPACT_ANALYSIS_SUBMITTED

        [Column("APPLICANT_PROJECTED_DEMAND")]
        public string _ApplicantProjectedDemand { get; set; }

        [Column("ADWR_PROJECTED_DEMAND")]
        public string _ADWRProjectedDemand { get; set; }

        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Column("METHOD_USED")]
        public string MethodUsed { get; set; }

        [NotMapped]
        public decimal? ApplicantProjectedDemand {
            get => !string.IsNullOrEmpty(_ApplicantProjectedDemand) ? (decimal?)decimal.Parse(Regex.Replace(_ApplicantProjectedDemand, @"[^0-9.]+", "")) : null;
            set => _ApplicantProjectedDemand = value.ToString();
        }

        [NotMapped]
        public decimal? ADWRProjectedDemand
        {
            get => !string.IsNullOrEmpty(_ADWRProjectedDemand) ? (decimal?)decimal.Parse(Regex.Replace(_ADWRProjectedDemand, @"[^0-9.]+", "")) : null;
            set => _ADWRProjectedDemand = value.ToString();
        }
    }

    
}