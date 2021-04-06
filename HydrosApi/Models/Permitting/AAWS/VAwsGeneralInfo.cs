namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;
    using ViewModel;
    using System.Collections.Generic;
    using System.Linq;   

    [Table("AWS.V_AWS_GENERAL_INFO")]
    public class VAwsGeneralInfo : Repository<VAwsGeneralInfo>//V_AWS_GENERAL_INFO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VAwsGeneralInfo()
        {
        }
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int? WaterRightFacilityId { get; set; }

        [Column("PCC")]
        public string ProgramCertificateConveyance { get; set; }

        [Column("SUBDIVISION_NAME")]
        public string Subdivision { get; set; }

        [Column("AMA_DESCR")]
        public string AMA { get; set; }

        [Column("CAMA_CODE")]
        public string AmaCode { get; set; }//Cama_code

        [Column("PROGRAM_CODE")]
        public string ProgramCode { get; set; }

        [Column("FILE_REVIEWER")]
        public string FileReviewer { get; set; }

        [Column("SECONDARY_PROV_SYS")] //replace once LTFDaysRemaining is added to the view
        public string LTFDaysRemaining { get; set; }

        [Column("APP_STATUS_DESCR")]
        public string Status { get; set; }

        [Column("APP_STATUS_DT")]
        public DateTime? StatusDate { get; set; }

        [Column("APP_STATUS_CODE")]
        public string StatusCode { get; set; }
        //public DateTime? APP_STATUS_DT { get; set; }

        [Column("HYDROLOGY")]
        public string Hydrology { get; set; }

        [Column("LEGAL_AVAILABILITY")]
        public string LegalAvailability { get; set; }//Legal_Availability

        [Column("SECONDARY_PROV_NAME")]
        public string SecondaryProviderName { get; set; }

        [Column("SECONDARY_PROV_WRFID")]
        public int? SecondaryProviderWrfId { get; set; }

        [Column("USER_NAME")]
        public string UserName { get; set; }

        [Column("PRIMARY_PROV_WRFID")]
        public int? PrimaryProviderWrfId { get; set; }

        [Column("PRIMARY_PROV_NAME")]
        public string PrimaryProviderName { get; set; }

        [Column("COMPLETE_CORRECT_DT")]
        public DateTime? CompleteCorrect { get; set; }//Complete_Correct

        [Column("DATEACCEPTED")]
        public DateTime? DateAccepted { get; set; }//Date_Accepted

        [Column("DEC_COMPLETE_DT")] 
        public DateTime? DateDeclaredComplete { get; set; }//Date_Declared_Complete

        [Column("RECEIVEDDT")]
        public DateTime? DateReceived { get; set; }//Date_Received

        [Column("FIRST_NOTICEDT")]
        public DateTime? FirstNoticeDate { get; set; }//First_Notice_Date

        [Column("SEC_NOTICEDT")]
        public DateTime? SecondNoticeDate { get; set; }//Second_Notice_Date

        [Column("PUB_COMM_ENDDT")]
        public DateTime? FinalDateForPublicComment { get; set; }//Final_Date_for_Public_Comment

        //[Column("DEC_COMPLETE_DT")]
        //public DateTime? Final_Date_for_Public_Comment { get; set; }
        [Column("PHYSICAL_AVAILABILITY")]
        public string PhysicalAvailability { get; set; }//Physical_Availability

        [Column("CONTINUOUS_AVAILABILITY")]
        public string ContinuousAvailability { get; set; }//Continuous_Availability

        [Column("CONSISTENCY_WITH_MGMT_PLAN")]
        public string ConsistencyWithMgmtPlan { get; set; }//Consistency_With_Mgmt_Plan

        [Column("CONSISTENCY_WITH_MGMT_GOAL")]
        public string ConsistencyWithMgmtGoal { get; set; }//Consistency_With_Mgmt_Goal

        [Column("FINANCIAL_CAPABILITY")]
        public string FinancialCapability { get; set; }//Financial_Capability

        [Column("OWNERSHIP_DOCUMENTS")]
        public string OwnershipDocuments { get; set; }//Ownership_Documents

        [Column("DEMAND_CALCULATOR")]
        public string DemandCalculator { get; set; }//Demand_Calculator

        [Column("WATER_QUALITY")]
        public string WaterQuality { get; set; }//Water_Quality

        [Column("DESIGNATION_TERM")]
        public int? DesignationTerm { get; set; }//Designation_Term

        [Column("COUNTY_CODE")]
        public string CountyCode { get; set; }//County_Code

        [Column("COUNTY_DESCR")]
        public string CountyDescription { get; set; }//County_Descr

        [Column("REVIEW_PLAT_MPC")]
        public string ReviewPlatForMaterialPlatChanges { get; set; }//Review_Plat_for_MPC

        [Column("CHECK_PLAT_RECORDED")]
        public string CheckPlatRecorded { get; set; }//Check_Plat_Recorded

        [Column("VERIFY_WTR_PROVIDER_LTR_REC")]
        public string VerifyWaterProviderLetterReceived { get; set; }//Verify_Water_Provider_Letter_Received

        //[NotMapped]
        //public Dictionary<string,bool> Overview { get; set; } //add or remove 

        [NotMapped]
        public List<VAwsHydro> HydrologyInfo { get; set; } //add or remove 

        [NotMapped]
        public string PWS_ID_Number { get; set; }
        [NotMapped]
        public string SubbasinCode { get; set; }
        [NotMapped]
        public List<SP_AW_CONV_DIAGRAM> Diagram { get; set; }

        [NotMapped]
        public VCdAwAppFeeRates FeeRates { get; set; }

        [NotMapped]
        public string ProcessStatus { get; set; } //Use this for error messages in stored procedure or api calls 

        //========================================================================================================
        //Overloaded Methods below Work for a lot of things
        //========================================================================================================
        //THERE IS AN AUTOMATED PROCESS THAT SETS VALUES USING LIKE-NAMED VARIABLES BETWEEN THE FRONT AND BACK END
        //THIS ONLY WORKS IF THE NAMES CORRESPOND. OTHERWISE YOU WILL HAVE TO MANUALLY ACCOUNT FOR *EVERY* COLUMN
        //DO YOU REALLY WANT TO DO THAT? SERIOUSLY. 


        //== get multiple records and only a few columns to populate the top tabs =======================

       /* public static List<V_AWS_GENERAL_INFO> GetGeneralInformation(List<int> wrfId)
        {
            Dictionary<string, bool> setCriteria = new Dictionary<string, bool>();        
            generalInfo.Diagram = SP_AW_CONV_DIAGRAM.ConveyanceDiagram(generalInfo.ProgramCertificateConveyance);            
            setCriteria.Add("Physical_Availability", generalInfo.Physical_Availability == "Y" ? true : false);
            setCriteria.Add("Hydrology", generalInfo.Hydrology == "Y" ? true : false);
            setCriteria.Add("Continuous_Availability",generalInfo.Continuous_Availability == "Y" ? true : false);
            setCriteria.Add("Legal_Availability", generalInfo.Legal_Availability == "Y" ? true : false);
            setCriteria.Add("Consistency_With_Mgmt_Plan", generalInfo.Consistency_With_Mgmt_Plan == "Y" ? true : false);
            setCriteria.Add("Consistency_With_Mgmt_Goal", generalInfo.Consistency_With_Mgmt_Goal == "Y" ? true : false);
            setCriteria.Add("Water_Quality", generalInfo.Water_Quality == "Y" ? true : false);
            setCriteria.Add("Financial_Capability", generalInfo.Financial_Capability == "Y" ? true : false);
            setCriteria.Add("Demand_Calculator", generalInfo.Demand_Calculator == "Y" ? true : false);
            generalInfo.Overview = setCriteria;

            generalInfo.PWS_ID_Number = generalInfo.PrimaryProviderWrfId != null ? V_AWS_PROVIDER.Get(p => p.PROVIDER_WRF_ID == generalInfo.PrimaryProviderWrfId).PWS_ID_Number : null;
            generalInfo.HydrologyInfo = V_AWS_HYDRO.GetList(h => h.WRFID == generalInfo.WaterRightFacilityId);
            generalInfo.FeeRates= V_CD_AW_APP_FEE_RATES.Get(x => x.PROGRAM_CODE == generalInfo.ProgramCode);*/

           /* var overView = new AWS_OVER_VIEW();
            var overViewProperties = overView.GetType().GetProperties();
            var generalInfoProperties = generalInfo.GetType().GetProperties();

            foreach(var prop in generalInfoProperties)
            {
                var gValue = prop.GetValue(generalInfo);
                var gType = prop.PropertyType.Name;
                var gName = prop.Name;

                var oProp = overView.GetType().GetProperty(gName);
                if(oProp !=null && oProp.PropertyType.Name==gType)
                {
                    oProp.SetValue(overView, gValue);
                }
            }*/

        public static List<VAwsGeneralInfo> GetGeneralInformation(List<string> pcc)
        {
            var general = VAwsGeneralInfo.GetList(g => pcc.Contains(g.ProgramCertificateConveyance));
            return PopulateGeneralInfoSummary(general);
        }
        
        //===== get individual records to populate forms ================================================
        public static VAwsGeneralInfo GetGeneralInformation(string pcc)
        {
            var generalInfo=VAwsGeneralInfo.Get(g => g.ProgramCertificateConveyance == pcc);

            if(generalInfo != null)
            {
                PopulateGeneralInfo(generalInfo);
            }

            return generalInfo;
        }

        public static VAwsGeneralInfo GetGeneralInformation(int wrfId)
        {
            var generalInfo = VAwsGeneralInfo.Get(g => g.WaterRightFacilityId == wrfId);            

            if (generalInfo != null)
            {
                PopulateGeneralInfo(generalInfo);
            }
            return generalInfo;
        }

        public static List<VAwsGeneralInfo> PopulateGeneralInfoSummary(List<VAwsGeneralInfo> generalInfo)
        {
            if (generalInfo == null)
            {
                return generalInfo;
            }

            return generalInfo.Select(g => new VAwsGeneralInfo
            {
                ProgramCertificateConveyance = g.ProgramCertificateConveyance,
                WaterRightFacilityId = g.WaterRightFacilityId,
                Subdivision = g.Subdivision,
                ProgramCode = g.ProgramCode,
                AmaCode = g.AmaCode
            }).ToList();
        }

        //from dictionary with field dictionary key names that correspond to the column or column alias names
        //set values true or false back to Y/N
        public static void SetGeneralInfoCriteriaFromBool(Dictionary<string, bool> criteriaValues, VAwsGeneralInfo generalInfo)
        {
            foreach (var criteriaValue in criteriaValues)
            {
                var property = generalInfo.GetType().GetProperty(criteriaValue.Key);
                var currentValue = property.GetValue(generalInfo);

                if (property != null && currentValue != null)
                {
                    property.SetValue(generalInfo, criteriaValue.Value == true ? "Y" : "N");
                }
            }
        }

        public static void PopulateGeneralInfo(VAwsGeneralInfo generalInfo)
        {
            Dictionary<string, bool> setCriteria = new Dictionary<string, bool>();
            generalInfo.Diagram = SP_AW_CONV_DIAGRAM.ConveyanceDiagram(generalInfo.ProgramCertificateConveyance);
            setCriteria.Add("Physical_Availability", generalInfo.PhysicalAvailability == "Y" && true);
            setCriteria.Add("Hydrology", generalInfo.Hydrology == "Y" && true);
            setCriteria.Add("Continuous_Availability", generalInfo.ContinuousAvailability == "Y" && true);
            setCriteria.Add("Legal_Availability", generalInfo.LegalAvailability == "Y" && true);
            setCriteria.Add("Consistency_With_Mgmt_Plan", generalInfo.ConsistencyWithMgmtPlan == "Y" && true);
            setCriteria.Add("Consistency_With_Mgmt_Goal", generalInfo.ConsistencyWithMgmtGoal == "Y" && true);
            setCriteria.Add("Water_Quality", generalInfo.WaterQuality == "Y" && true);
            setCriteria.Add("Financial_Capability", generalInfo.FinancialCapability == "Y" && true);
            setCriteria.Add("Demand_Calculator", generalInfo.DemandCalculator == "Y" && true);
            setCriteria.Add("Review_Plat_for_MPC", generalInfo.DemandCalculator == "Y" && true);
            setCriteria.Add("Check_Plat_Recorded", generalInfo.DemandCalculator == "Y" && true);
            setCriteria.Add("Verify_Water_Provider_Letter_Received", generalInfo.DemandCalculator == "Y" && true);
            //generalInfo.Overview = setCriteria;

            generalInfo.PWS_ID_Number = generalInfo.PrimaryProviderWrfId != null ? VAwsProvider.Get(p => p.ProviderWaterRightFacilityId == generalInfo.PrimaryProviderWrfId).ProviderPublicWaterSystemId : null;
            var hydrologyInfo = VAwsHydro.Get(h => h.WaterRightFacilityId == generalInfo.WaterRightFacilityId);

            if (hydrologyInfo != null)
            {
                generalInfo.SubbasinCode = hydrologyInfo.SubbasinCode;
            }
            generalInfo.FeeRates = VCdAwAppFeeRates.Get(x => x.ProgramCode == generalInfo.ProgramCode);

        }
    }
}
