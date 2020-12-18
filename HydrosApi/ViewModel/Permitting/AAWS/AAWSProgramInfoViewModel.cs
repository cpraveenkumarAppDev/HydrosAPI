namespace HydrosApi.ViewModel
{

    using Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using HydrosApi.Data;
    using Oracle.ManagedDataAccess.Client;
    using HydrosApi.Services;
    using WebApi.OutputCache.Core.Time;
    using HydrosApi.Models.Permitting.AAWS;
    using HydrosApi.Models.ADWR;

    public class AAWSProgramInfoViewModel
    {
        public AWS_OVER_VIEW OverView { get; set; }
        public string ProgramCertificateConveyance { get; set; }
        public string Subdivision { get; set; }
        public string ProgramCode { get; set; }
        public string Cama_code { get; set; }
        public int? WaterRightFacilityId { get; set; }
         
        public V_CD_AW_APP_FEE_RATES FeeRates { get; set; }
        public List<V_AWS_SUBBAS> SubbasinList { get; set; }
        public List<SP_AW_CONV_DIAGRAM> Diagram { get; set; }
        public V_AWS_HYDRO Hydrology { get; set; }
        public List<CD_AMA_INA> AmaIna { get; set; }
        public List<CD_AMA_INA> AmaList { get; set; }
        public List<CD_AMA_INA> InaList { get; set; }
        public List<V_AWS_COUNTY_BASIN> CountyBasinList { get; set; }
        public static AAWSProgramInfoViewModel GetData(string PermitCertificateConveyanceNumber)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            AWS_OVER_VIEW AAWSProgramInfoViewModelOverView = new AWS_OVER_VIEW();
            V_AWS_HYDRO Hydrology = V_AWS_HYDRO.Get(p => p.PCC == PermitCertificateConveyanceNumber);
            List<V_AWS_PROVIDER> lists = V_AWS_PROVIDER.GetAll();
            AAWSProgramInfoViewModelOverView.SubbasinList = V_AWS_SUBBAS.GetAll();
            AAWSProgramInfoViewModel.CountyBasinList= V_AWS_COUNTY_BASIN.GetAll().OrderBy(x => x.COUNTY_NAME).ToList();
            

            try
            {
                var GeneralInfo = V_AWS_GENERAL_INFO.Get(p => p.ProgramCertificateConveyance == PermitCertificateConveyanceNumber);
                
                AAWSProgramInfoViewModel.ProgramCertificateConveyance = PermitCertificateConveyanceNumber;
                AAWSProgramInfoViewModel.WaterRightFacilityId = GeneralInfo.WaterRightFacilityId;
                AAWSProgramInfoViewModel.ProgramCode = GeneralInfo.ProgramCode;
                AAWSProgramInfoViewModel.Subdivision = GeneralInfo.Subdivision;
                AAWSProgramInfoViewModel.Diagram = SP_AW_CONV_DIAGRAM.ConveyanceDiagram(PermitCertificateConveyanceNumber);
                AAWSProgramInfoViewModel.FeeRates = V_CD_AW_APP_FEE_RATES.Get(x => x.PROGRAM_CODE == PermitCertificateConveyanceNumber.Substring(0, 2));
                AAWSProgramInfoViewModel.Cama_code = GeneralInfo.Cama_code;
                //OverView data
                AAWSProgramInfoViewModelOverView.PrimaryProviderName = GeneralInfo.PrimaryProviderName;
                AAWSProgramInfoViewModelOverView.AMA = GeneralInfo.AMA;
                AAWSProgramInfoViewModelOverView.Physical_Availability = GeneralInfo.Physical_Availability == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.SecondaryProviderWrfId = GeneralInfo.SecondaryProviderWrfId;
                AAWSProgramInfoViewModelOverView.PrimaryProviderWrfId = GeneralInfo.PrimaryProviderWrfId;

                if (lists != null)
                {
                    var stuff = lists.Where(p => p.PROVIDER_WRF_ID == AAWSProgramInfoViewModelOverView.PrimaryProviderWrfId).FirstOrDefault();
                    AAWSProgramInfoViewModelOverView.PWS_ID_Number = stuff != null ? stuff.PWS_ID_Number : "";
                }

                AAWSProgramInfoViewModelOverView.SecondaryProviderName = GeneralInfo.SecondaryProviderName;
                AAWSProgramInfoViewModelOverView.Date_Accepted = GeneralInfo.Date_Accepted;
                AAWSProgramInfoViewModelOverView.Date_Received = GeneralInfo.Date_Received;
                AAWSProgramInfoViewModelOverView.Complete_Correct = GeneralInfo.Complete_Correct;

                AAWSProgramInfoViewModelOverView.Physical_Availability = GeneralInfo.Physical_Availability== "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Hydrology = GeneralInfo.Hydrology == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Continuous_Availability = GeneralInfo.Continuous_Availability == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Legal_Availability = GeneralInfo.Legal_Availability == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Consistency_With_Mgmt_Plan = GeneralInfo.Consistency_With_Mgmt_Plan == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Consistency_With_Mgmt_Goal = GeneralInfo.Consistency_With_Mgmt_Goal == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Water_Quality = GeneralInfo.Water_Quality == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Financial_Capability = GeneralInfo.Financial_Capability == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Ownership_Documents = GeneralInfo.Ownership_Documents== "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Demand_Calculator = GeneralInfo.Demand_Calculator== "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Review_Plat_MPC = GeneralInfo.Review_Plat_MPC == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Check_Plat_Recorded = GeneralInfo.Check_Plat_Recorded == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Verify_Wtr_Provider_Ltr_Rec = GeneralInfo.Verify_Wtr_Provider_Ltr_Rec == "Y" ? true : false;

                AAWSProgramInfoViewModelOverView.Designation_Term = GeneralInfo.Designation_Term;
                AAWSProgramInfoViewModelOverView.First_Notice = GeneralInfo.First_Notice_Date;
                AAWSProgramInfoViewModelOverView.Second_Notice = GeneralInfo.Second_Notice_Date;
                AAWSProgramInfoViewModelOverView.Date_Declared_Complete = GeneralInfo.Date_Declared_Complete;
                AAWSProgramInfoViewModelOverView.Final_Date = GeneralInfo.Final_Date_for_Public_Comment;
                AAWSProgramInfoViewModelOverView.ProvidersList = lists;
                AAWSProgramInfoViewModelOverView.SubbasinCode = Hydrology.SUBBASIN_CODE;
                AAWSProgramInfoViewModel.OverView = AAWSProgramInfoViewModelOverView;
                //Hydrology data
                AAWSProgramInfoViewModel.Hydrology = Hydrology;

                var ama_ina_codes = CD_AMA_INA.GetAll();

                AAWSProgramInfoViewModel.AmaIna = ama_ina_codes;
                AAWSProgramInfoViewModel.AmaList = ama_ina_codes.Where(x => x.AMA_INA_TYPE == "AMA").ToList();
                AAWSProgramInfoViewModel.InaList = ama_ina_codes.Where(x => x.AMA_INA_TYPE == "INA").ToList();
                AAWSProgramInfoViewModelOverView.County = GeneralInfo.County_Descr;


               return AAWSProgramInfoViewModel;
            }
            catch (FileNotFoundException e)
            {
                // FileNotFoundExceptions are handled here.
                return AAWSProgramInfoViewModel;
            }
            catch (Exception exception)
            {
#if DEBUG
#else
                EmailService.Message(exception);
                return null;
#endif
                return AAWSProgramInfoViewModel;
            }
        }

        public static AAWSProgramInfoViewModel OnUpdate(AAWSProgramInfoViewModel paramValues, string user)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            using (var ctx = new OracleContext())
            {
                var application = ctx.V_AWS_GENERAL_INFO.Where(p => p.ProgramCertificateConveyance == paramValues.ProgramCertificateConveyance).FirstOrDefault<V_AWS_GENERAL_INFO>();
                
                application.Physical_Availability = application.Physical_Availability == null && paramValues.OverView.Physical_Availability == false ? null: paramValues.OverView.Physical_Availability == true ? "Y" : "N";
                application.Hydrology = application.Hydrology == null && paramValues.OverView.Hydrology == false ? null : paramValues.OverView.Hydrology == true ? "Y" : "N";
                application.Continuous_Availability = application.Continuous_Availability == null && paramValues.OverView.Continuous_Availability == false ? null : paramValues.OverView.Continuous_Availability == true ? "Y" : "N";
                application.Legal_Availability = application.Legal_Availability == null && paramValues.OverView.Legal_Availability == false ? null : paramValues.OverView.Legal_Availability == true ? "Y" : "N";
                application.Consistency_With_Mgmt_Plan = application.Consistency_With_Mgmt_Plan == null && paramValues.OverView.Consistency_With_Mgmt_Plan == false ? null : paramValues.OverView.Consistency_With_Mgmt_Plan == true ? "Y" : "N";
                application.Consistency_With_Mgmt_Goal = application.Consistency_With_Mgmt_Goal == null && paramValues.OverView.Consistency_With_Mgmt_Goal == false ? null : paramValues.OverView.Consistency_With_Mgmt_Goal == true ? "Y" : "N";
                application.Water_Quality = application.Water_Quality == null && paramValues.OverView.Water_Quality == false ? null : paramValues.OverView.Water_Quality == true ? "Y" : "N";
                application.Financial_Capability = application.Financial_Capability == null && paramValues.OverView.Financial_Capability == false ? null : paramValues.OverView.Financial_Capability == true ? "Y" : "N";
                application.Ownership_Documents = application.Ownership_Documents == null && paramValues.OverView.Ownership_Documents == false ? null : paramValues.OverView.Ownership_Documents == true ? "Y" : "N";
                application.Demand_Calculator = application.Demand_Calculator == null && paramValues.OverView.Demand_Calculator == false ? null : paramValues.OverView.Demand_Calculator == true ? "Y" : "N";
                application.Review_Plat_MPC = application.Review_Plat_MPC == null && paramValues.OverView.Review_Plat_MPC == false ? null : paramValues.OverView.Review_Plat_MPC == true ? "Y" : "N";
                application.Check_Plat_Recorded = application.Check_Plat_Recorded == null && paramValues.OverView.Check_Plat_Recorded == false ? null : paramValues.OverView.Check_Plat_Recorded == true ? "Y" : "N";
                application.Verify_Wtr_Provider_Ltr_Rec = application.Verify_Wtr_Provider_Ltr_Rec == null && paramValues.OverView.Verify_Wtr_Provider_Ltr_Rec == false ? null : paramValues.OverView.Verify_Wtr_Provider_Ltr_Rec == true ? "Y" : "N";

                application.Designation_Term = paramValues.OverView.Designation_Term;                
                application.PrimaryProviderWrfId = paramValues.OverView.PrimaryProviderWrfId;
                application.SecondaryProviderWrfId = paramValues.OverView.SecondaryProviderWrfId;
                application.First_Notice_Date = paramValues.OverView.First_Notice;
                application.Date_Received = paramValues.OverView.Date_Received;
                application.Date_Accepted = paramValues.OverView.Date_Accepted;
                application.Second_Notice_Date = paramValues.OverView.Second_Notice;
                application.Final_Date_for_Public_Comment = paramValues.OverView.Final_Date;
                application.Date_Declared_Complete = paramValues.OverView.Date_Declared_Complete;
                application.Complete_Correct = paramValues.OverView.Complete_Correct;
                application.UserName = user;
                application.Subdivision = paramValues.Subdivision;
                application.Cama_code = paramValues.Cama_code;
                if(paramValues.OverView.County != null)
                {
                application.County_Code = CD_AW_COUNTY.GetAll().Where(x => x.DESCR.ToUpper() == paramValues.OverView.County.ToUpper()).Select(x => x.CODE).FirstOrDefault();
                }
                //Hydrology data
                var Hydrology = ctx.V_AWS_HYDRO.Where(p => p.PCC == paramValues.ProgramCertificateConveyance).FirstOrDefault<V_AWS_HYDRO>();
                Hydrology.SUBBASIN_CODE = paramValues.OverView.SubbasinCode;

                ctx.SaveChanges();

                return AAWSProgramInfoViewModel;
            }

            //var application = V_AWS_GENERAL_INFO.UpdateSome(new V_AWS_GENERAL_INFO()
            //{                
            //    Hydrology = paramValues.OverView.Hydrology == true ? "Y" : "N",
            //    Legal_Availability = paramValues.OverView.Legal_Availability == true ? "Y" : "N",
            //    PrimaryProviderWrfId = paramValues.OverView.PrimaryProviderWrfId,
            //    UserName = user

            //}, p => p.ProgramCertificateConveyance == paramValues.ProgramCertificateConveyance);

            //var Hydrology = V_AWS_HYDRO.UpdateSome(new V_AWS_HYDRO()
            //{
            //    SUBBASIN_CODE = paramValues.OverView.SubbasinCode
            //}, p => p.PCC == paramValues.ProgramCertificateConveyance);         

            //return AAWSProgramInfoViewModel;
        }
    }
}