namespace HydrosApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using Data;
    using ViewModel;
    using System.Collections.Generic;
    using System.Linq;
    using System.Dynamic;

    [Table("AWS.V_AWS_GENERAL_INFO")]
    public class V_AWS_GENERAL_INFO : Repository<V_AWS_GENERAL_INFO>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_GENERAL_INFO()
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
        public string Cama_code { get; set; }
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

        //public DateTime? APP_STATUS_DT { get; set; }

        [Column("HYDROLOGY")]
        public string Hydrology { get; set; }

        [Column("LEGAL_AVAILABILITY")]
        public string Legal_Availability { get; set; }

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
        public DateTime? Complete_Correct { get; set; }
        [Column("DATEACCEPTED")]
        public DateTime? Date_Accepted { get; set; }
        [Column("DEC_COMPLETE_DT")] 
        public DateTime? Date_Declared_Complete { get; set; }
        [Column("RECEIVEDDT")]
        public DateTime? Date_Received { get; set; }
        [Column("FIRST_NOTICEDT")]
        public DateTime? First_Notice_Date { get; set; }
        [Column("SEC_NOTICEDT")]
        public DateTime? Second_Notice_Date { get; set; }
        [Column("PUB_COMM_ENDDT")]
        public DateTime? Final_Date_for_Public_Comment { get; set; }
        //[Column("DEC_COMPLETE_DT")]
        //public DateTime? Final_Date_for_Public_Comment { get; set; }
        [Column("PHYSICAL_AVAILABILITY")]
        public string Physical_Availability { get; set; }
        [Column("CONTINUOUS_AVAILABILITY")]
        public string Continuous_Availability { get; set; }
        [Column("CONSISTENCY_WITH_MGMT_PLAN")]
        public string Consistency_With_Mgmt_Plan { get; set; }
        [Column("CONSISTENCY_WITH_MGMT_GOAL")]
        public string Consistency_With_Mgmt_Goal { get; set; }
        [Column("FINANCIAL_CAPABILITY")]
        public string Financial_Capability { get; set; }
        [Column("OWNERSHIP_DOCUMENTS")]
        public string Ownership_Documents { get; set; }
        [Column("DEMAND_CALCULATOR")]
        public string Demand_Calculator { get; set; }
        [Column("WATER_QUALITY")]
        public string Water_Quality { get; set; }
        [Column("DESIGNATION_TERM")]
        public int? Designation_Term { get; set; }
        [Column("COUNTY_CODE")]
        public string County_Code { get; set; }
        [Column("COUNTY_DESCR")]
        public string County_Descr { get; set; }
        [Column("REVIEW_PLAT_MPC")]
        public string Review_Plat_MPC { get; set; }
        [Column("CHECK_PLAT_RECORDED")]
        public string Check_Plat_Recorded { get; set; }
        [Column("VERIFY_WTR_PROVIDER_LTR_REC")]
        public string Verify_Wtr_Provider_Ltr_Rec { get; set; }

        [NotMapped]
        public Dictionary<string,bool> Overview { get; set; } //add or remove 

        [NotMapped]
        public List<V_AWS_HYDRO> HydrologyInfo { get; set; } //add or remove 

        [NotMapped]
        public string PWS_ID_Number { get; set; }

        [NotMapped]
        public List<SP_AW_CONV_DIAGRAM> Diagram { get; set; }

        [NotMapped]
        public List<V_CD_AW_APP_FEE_RATES> FeeRates { get; set; }

        [NotMapped]
        public string ProcessStatus { get; set; } //Use this for error messages in stored procedure or api calls 
        //V_CD_AW_APP_FEE_RATES.Get(x => x.PROGRAM_CODE == PermitCertificateConveyanceNumber.Substring(0, 2));

        //from dictionary with field dictionary key names that correspond to the column or column alias names
        public static void SetGeneralInfoCriteriaFromBool(Dictionary<string, bool> criteriaValues, V_AWS_GENERAL_INFO generalInfo)
        {
            foreach(var criteriaValue in criteriaValues)
            {
                var property = generalInfo.GetType().GetProperty(criteriaValue.Key);
                var currentValue=property.GetValue(generalInfo);

                if (property != null && currentValue !=null)
                {
                    property.SetValue(generalInfo, criteriaValue.Value == true ? "Y" : "N");       
                }
            }
        }

        //application.Physical_Availability = application.Physical_Availability == null && paramValues.OverView.Physical_Availability == false ? null: paramValues.OverView.Physical_Availability == true ? "Y" : "N";

        public static void PopulateGeneralInfo(V_AWS_GENERAL_INFO generalInfo)
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
            generalInfo.FeeRates= V_CD_AW_APP_FEE_RATES.GetList(x => x.PROGRAM_CODE == generalInfo.ProgramCode);

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
            }

            foreach(var s in setCriteria)
            {
                var oCriteria = overView.GetType().GetProperty(s.Key);
                if(oCriteria != null)
                {
                    oCriteria.SetValue(overView, s.Value);
                }
            }*/
        }

        public static V_AWS_GENERAL_INFO PopulateGeneralInfoSummary(V_AWS_GENERAL_INFO generalInfo)
        {
            if(generalInfo==null)
            {
                return generalInfo;
            }
           
            return new V_AWS_GENERAL_INFO()
            {
                ProgramCertificateConveyance = generalInfo.ProgramCertificateConveyance,
                WaterRightFacilityId = generalInfo.WaterRightFacilityId,
                Subdivision=generalInfo.Subdivision,
                ProgramCode=generalInfo.ProgramCode,
                Cama_code=generalInfo.Cama_code

            };
        }

        //get only a few fields populated with data
        public static V_AWS_GENERAL_INFO GetGeneralInformationSummary(int wrfId)
        {
            return PopulateGeneralInfoSummary(V_AWS_GENERAL_INFO.Get(g => g.WaterRightFacilityId == wrfId));
        }

        public static V_AWS_GENERAL_INFO GetGeneralInformationSummary(string pcc)
        {
            return PopulateGeneralInfoSummary(V_AWS_GENERAL_INFO.Get(g => g.ProgramCertificateConveyance == pcc));
        }


        //get general information values
        public static V_AWS_GENERAL_INFO GetGeneralInformation(string pcc)
        {
            var generalInfo=V_AWS_GENERAL_INFO.Get(g => g.ProgramCertificateConveyance == pcc);

            if(generalInfo != null)
            {
                PopulateGeneralInfo(generalInfo);
            }

            return generalInfo;
        }

        public static V_AWS_GENERAL_INFO GetGeneralInformation(int wrfId)
        {
            var generalInfo = V_AWS_GENERAL_INFO.Get(g => g.WaterRightFacilityId == wrfId);            

            if (generalInfo != null)
            {
                PopulateGeneralInfo(generalInfo);
            }
            return generalInfo;
        }
        


            /* [NotMapped]
             public string StatusDate {
                 get {                  
                     return APP_STATUS_DT.ToString();
                 }

                 set {
                     this.StatusDate = value;

                     APP_STATUS_DT=DateTime.Parse(value);

                 }            
              }*/
        }
}
