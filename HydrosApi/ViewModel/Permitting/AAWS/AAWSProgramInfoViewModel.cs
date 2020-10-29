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

    public class AAWSProgramInfoViewModel
    {
        public AWS_OVER_VIEW OverView { get; set; }
        public string ProgramCertificateConveyance { get; set; }
        public string Subdivision { get; set; }
        public string ProgramCode { get; set; }
        public int? WaterRightFacilityId { get; set; }
         
        public V_CD_AW_APP_FEE_RATES FeeRates { get; set; }
        public List<V_AWS_SUBBAS> SubbasinList { get; set; }
        public List<SP_AW_CONV_DIAGRAM> Diagram { get; set; }
        public V_AWS_HYDRO Hydrology { get; set; }
        public List<string> AmaList { get; set; }
        public List<string> InaList { get; set; }
        public static AAWSProgramInfoViewModel GetData(string PermitCertificateConveyanceNumber)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            AWS_OVER_VIEW AAWSProgramInfoViewModelOverView = new AWS_OVER_VIEW();
            V_AWS_HYDRO Hydrology = V_AWS_HYDRO.Get(p => p.PCC == PermitCertificateConveyanceNumber);
            List<V_AWS_PROVIDER> lists = V_AWS_PROVIDER.GetAll();
            List<V_AWS_SUBBAS> SubbasinList = V_AWS_SUBBAS.GetAll();
            

            try
            {
                var GeneralInfo = V_AWS_GENERAL_INFO.Get(p => p.ProgramCertificateConveyance == PermitCertificateConveyanceNumber);
                
                AAWSProgramInfoViewModel.ProgramCertificateConveyance = PermitCertificateConveyanceNumber;
                AAWSProgramInfoViewModel.WaterRightFacilityId = GeneralInfo.WaterRightFacilityId;
                AAWSProgramInfoViewModel.ProgramCode = GeneralInfo.ProgramCode;
                AAWSProgramInfoViewModel.Subdivision = GeneralInfo.Subdivision;
                AAWSProgramInfoViewModel.Diagram = SP_AW_CONV_DIAGRAM.ConveyanceDiagram(PermitCertificateConveyanceNumber);
                AAWSProgramInfoViewModel.FeeRates = V_CD_AW_APP_FEE_RATES.Get(x => x.PROGRAM_CODE == PermitCertificateConveyanceNumber.Substring(0, 2));
                //OverView data
                AAWSProgramInfoViewModelOverView.PrimaryProviderName = GeneralInfo.PrimaryProviderName;
                AAWSProgramInfoViewModelOverView.AMA = GeneralInfo.AMA;
                AAWSProgramInfoViewModelOverView.SecondaryProviderWrfId = GeneralInfo.SecondaryProviderWrfId;
                AAWSProgramInfoViewModelOverView.PrimaryProviderWrfId = GeneralInfo.PrimaryProviderWrfId != null ? (int)GeneralInfo.PrimaryProviderWrfId : 0;

                if (lists != null)
                {
                    AAWSProgramInfoViewModelOverView.PWS_ID_Number = lists.Where(p => p.PROVIDER_WRF_ID == AAWSProgramInfoViewModelOverView.PrimaryProviderWrfId).FirstOrDefault().PWS_ID_Number;
                }

                AAWSProgramInfoViewModelOverView.SecondaryProviderName = GeneralInfo.SecondaryProviderName;
                AAWSProgramInfoViewModelOverView.Date_Accepted = GeneralInfo.Date_Accepted;
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
                AAWSProgramInfoViewModelOverView.Other = GeneralInfo.Other == "Y" ? true : false;

                AAWSProgramInfoViewModelOverView.Designation_Term = GeneralInfo.Designation_Term;
                AAWSProgramInfoViewModelOverView.First_Notice = GeneralInfo.First_Notice_Date;
                AAWSProgramInfoViewModelOverView.Second_Notice = GeneralInfo.Second_Notice_Date;
                AAWSProgramInfoViewModelOverView.Final_Date = GeneralInfo.Final_Date_for_Public_Comment;
                AAWSProgramInfoViewModelOverView.ProvidersList = lists;
                AAWSProgramInfoViewModelOverView.SubbasinCode = Hydrology.SUBBASIN_CODE;
                AAWSProgramInfoViewModelOverView.SubbasinList = SubbasinList;
                AAWSProgramInfoViewModel.OverView = AAWSProgramInfoViewModelOverView;
                //Hydrology data
                AAWSProgramInfoViewModel.Hydrology = Hydrology;

                var ama_ina_codes = CD_AMA_INA.GetAll();

                AAWSProgramInfoViewModel.AmaList = ama_ina_codes.Where(x => x.AMA_INA_TYPE == "AMA").Select(x => x.DESCR).OrderBy(x => x).ToList();
                AAWSProgramInfoViewModel.InaList = ama_ina_codes.Where(x => x.AMA_INA_TYPE == "INA").Select(x => x.DESCR).OrderBy(x => x).ToList();


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
                
                application.Physical_Availability = paramValues.OverView.Physical_Availability == true ? "Y" : "N";
                application.Hydrology = paramValues.OverView.Hydrology == true ? "Y" : "N";
                application.Continuous_Availability = paramValues.OverView.Continuous_Availability == true ? "Y" : "N";
                application.Legal_Availability = paramValues.OverView.Legal_Availability == true ? "Y" : "N";
                application.Consistency_With_Mgmt_Plan=paramValues.OverView.Consistency_With_Mgmt_Plan == true ? "Y" : "N";
                application.Consistency_With_Mgmt_Goal = paramValues.OverView.Consistency_With_Mgmt_Goal == true ? "Y" : "N";
                application.Water_Quality = paramValues.OverView.Water_Quality == true ? "Y" : "N";
                application.Financial_Capability = paramValues.OverView.Financial_Capability == true ? "Y" : "N";
                application.Ownership_Documents = paramValues.OverView.Ownership_Documents == true ? "Y" : "N";
                application.Demand_Calculator = paramValues.OverView.Demand_Calculator == true ? "Y" : "N";
                application.Other = paramValues.OverView.Other == true ? "Y" : "N";

                application.Designation_Term = paramValues.OverView.Designation_Term;                
                application.PrimaryProviderWrfId = paramValues.OverView.PrimaryProviderWrfId;
                application.SecondaryProviderWrfId = paramValues.OverView.SecondaryProviderWrfId;
                application.UserName = user;
                application.Subdivision = paramValues.Subdivision;
                application.Cama_code = CD_AMA_INA.GetAll().Where(x => x.DESCR == paramValues.OverView.AMA).FirstOrDefault().CODE;
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