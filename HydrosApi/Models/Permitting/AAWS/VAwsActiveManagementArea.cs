

namespace HydrosApi.Models.Permitting.AAWS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;


    [Table("AWS.V_AWS_AMA")]
    public class VAwsActiveManagementArea : Repository<VAwsActiveManagementArea>
    {
        [Key, Column("WRFID", Order = 1)]
        public int? WaterRightFacilityId { get; set; }//WRFID

        [Key, Column("PCC", Order = 2), StringLength(4000)]
        public string PCC { get; set; }

        [Column("PROVIDER_SIZE"), StringLength(50)]
        public string ProviderSize { get; set; }//PROVIDER_SIZE

        [Column("GWRIGHTS"), StringLength(20)]
        public string GroundwaterRights { get; set; }//GWRIGHTS

        [Column("SWRIGHTS"), StringLength(20)]
        public string SurfaceWaterRights { get; set; }//SWRIGHTS

        [Column("EXTINGUISH"), StringLength(20)]
        public string Extinguish { get; set; }//EXTINGUISH

        [Column("DRYLOT"), StringLength(5)]
        public string DryLot { get; set; }//DRYLOT

        [Column("CCN"), StringLength(5)]
        public string CCN { get; set; }//Unable to find definition

        [Column("PLAT"), StringLength(5)]
        public string Plat { get; set; }//PLAT

        [Column("EXEMPTCREDITS"), StringLength(5)]
        public string ExemptCredits { get; set; }//EXEMPTCREDITS

        [Column("POORQUALITY_EXEMPT"), StringLength(5)]
        public string PoorQualityExempt { get; set; }//POORQUALITY_EXEMPT

        [Column("NOIREFILE"), StringLength(5)]
        public string NoiReFile { get; set; }//NOIREFILE

        [Column("PROVTARGET"), StringLength(50)]
        public string ProvTarget { get; set; }//PROVTARGET

        [Column("PROVCOMPLY"), StringLength(5)]
        public string ProvComply { get; set; }//PROVCOMPLY

        [Column("ADMINREVIEW"), StringLength(5)]
        public string AdminReview { get; set; }//ADMINREVIEW

        [Column("BASICGWALLOW"), StringLength(50)]
        public string BasicGroundwaterAllow { get; set; }//BASICGWALLOW

        [Column("EXTCREDITS"), StringLength(50)]
        public string ExtCredits { get; set; }//EXTCREDITS

        [Column("STOREDCREDITS"), StringLength(50)]
        public string StoredCredits { get; set; }//STOREDCREDITS

        [Column("TOTALCREDITS"), StringLength(50)]
        public string TotalCredits { get; set; }//TOTALCREDITS

        [Column("TOTALRESDEMAND"), StringLength(50)]
        public string TotalResidentialDemand { get; set; }//TOTALRESDEMAND

        [Column("RES_USE_GPCD")]
        public int? ResidentialUseGPCD { get; set; }//RES_USE_GPCD

        [Column("TOTALNONRES"), StringLength(50)]
        public string TotalNonResidential { get; set; }//TOTALNONRES

        [Column("CONSTRUCDEMAND"), StringLength(50)]
        public string ConstrucDemand { get; set; }//CONSTRUCDEMAND

        [Column("LUDEMAND"), StringLength(50)]
        public string LuDemand { get; set; }//LUDEMAND

        [Column("TOTAL_NON_RES_DEMAND")]
        public decimal? TotalNonResidentialDemand { get; set; }//TOTAL_NON_RES_DEMAND

        [Column("TOTALDEMAND"), StringLength(50)]
        public string TotalDemand { get; set; }//TOTALDEMAND

        [Column("TOTAL_DEMAND_GPCD")]
        public decimal? TotalDemandGPCD { get; set; }//TOTAL_DEMAND_GPCD

        [Column("GWDEMAND"), StringLength(50)]
        public string GroundwaterDemand { get; set; }//GWDEMAND

        [Column("EFFLUENT"), StringLength(50)]
        public string Effluent { get; set; }//EFFLUENT

        [Column("SURFACEWATER"), StringLength(50)]
        public string SurfaceWater { get; set; }//SURFACEWATER

        [Column("CAPWATER"), StringLength(50)]
        public string CapWater { get; set; }//CAPWATER

        [Column("COLORADORIVERWATER"), StringLength(50)]
        public string ColoradoRiverWater { get; set; }//COLORADORIVERWATER

        [Column("CENYRDEMAND"), StringLength(50)]
        public string CenYrDemand { get; set; }//CENYRDEMAND

        [Column("APPLICANT_ESTIMATE"), StringLength(50)]
        public string ApplicantEstimate { get; set; }//APPLICANT_ESTIMATE

        [Column("DISTRIBUTION_LOSS_TOTAL")]
        public decimal? DistributionLossTotal { get; set; }//DISTRIBUTION_LOSS_TOTAL

        [Column("DISTRIBUTION_LOSS_FACTOR")]
        public int? DistributionLossFactor { get; set; }//DISTRIBUTION_LOSS_FACTOR

        [Column("CONSTRUCTION_NUM_LOTS")]
        public int? ConstructionNumberOfLots { get; set; }//CONSTRUCTION_NUM_LOTS

        [Column("CONSTRUCTION_DEMAND")]
        public decimal? ConstructionDemand { get; set; }//CONSTRUCTION_DEMAND

        [Column("CONSTRUCTION_DEMAND_100YR")]
        public decimal? ConstructionDemand100Yr { get; set; }//CONSTRUCTION_DEMAND_100YR

        [Column("CURRENT_DEMAND")]
        public decimal? CurrentDemand { get; set; }//CURRENT_DEMAND

        [Column("CURRENT_DEMAND_YEAR")]
        public int? CurrentDemandYear { get; set; }//CURRENT_DEMAND_YEAR

        [Column("COMMITTED_DEMAND")]
        public decimal? CommittedDemand { get; set; }//COMMITTED_DEMAND

        [Column("PROJECTED_DEMAND")]
        public decimal? ProjectedDemand { get; set; }//PROJECTED_DEMAND

        [Column("PROJECTED_DEMAND_YEAR")]
        public int? ProjectedDemandYear { get; set; }//PROJECTED_DEMAND_YEAR

        [Column("DEMAND_ASSIGNED_OR_RETAINED"), StringLength(1)]
        public string DemandAssignedOrRetained { get; set; }//DEMAND_ASSIGNED_OR_RETAINED

        [Column("DEDICATION_LTSC"), StringLength(1)]
        public string DedicationLongTermStorageCredits { get; set; }//DEDICATION_LTSC

        [Column("EXEMPTCREDITS_AF_YR")]
        public int? ExemptCreditsAfYr { get; set; }

        [Column("POORQUALITY_EXEMPT_AR_YR")]
        public int? PoorQualityExemptArYr { get; set; }
        [Column("GROUNDWATER_AF_YR")]
        public decimal? GroundwaterAfYr { get; set; }//EFFLUENT_AF_YR

        [Column("EFFLUENT_AF_YR")]
        public decimal? EffluentAfYr { get; set; }//EFFLUENT_AF_YR

        [Column("SURFACEWATER_AF_YR")]
        public decimal? SurfaceWaterAfYr { get; set; }//SURFACEWATER_AF_YR

        [Column("CAPWATER_AF_YR")]
        public decimal? CapWaterAfYr { get; set; }//CAPWATER_AF_YR

        [Column("COLORADORIVERWATER_AF_YR")]
        public decimal? ColoradoRiverWaterAfYr { get; set; }//COLORADORIVERWATER_AF_YR


        [Column("AMA_COMPLIANCE_STATUS_DATE")]
        public DateTime? AmaComplianceStatusDate { get; set; }
        [NotMapped]
        public string MemberCAGRD { get; set; }

        [NotMapped]
        public DateTime? DateCAGRD { get; set; }//Demand_Calculator

        public static VAwsActiveManagementArea GetCAGRDInfo(int id)
        {
            var generalInfo = VAwsGeneralInfo.Get(g => g.WaterRightFacilityId == id);
            var AwsAMA = VAwsActiveManagementArea.Get(x => x.WaterRightFacilityId == id);
            if (generalInfo != null)
            {
                AwsAMA.MemberCAGRD = generalInfo.MemberCAGRD;
                AwsAMA.DateCAGRD = generalInfo.DateCAGRD;
            }

            return AwsAMA;
        }
    }
}