﻿namespace HydrosApi.ViewModel
{

    using Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using HydrosApi.Data;
    using Oracle.ManagedDataAccess.Client;
    using HydrosApi.Services;

    public class AAWSProgramInfoViewModel
    {
        public AWS_OVER_VIEW OverView { get; set; }
        public string ProgramCertificateConveyance { get; set; }
        public string Subdivision { get; set; }
        public V_CD_AW_APP_FEE_RATES FeeRates { get; set; }

        public List<SP_AW_CONV_DIAGRAM> Diagram {get; set; }
        public static AAWSProgramInfoViewModel GetData(string PermitCertificateConveyanceNumber)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            AWS_OVER_VIEW AAWSProgramInfoViewModelOverView = new AWS_OVER_VIEW();

            try
            {
                var GeneralInfo = V_AWS_GENERAL_INFO.Get(p => p.ProgramCertificateConveyance == PermitCertificateConveyanceNumber);

                AAWSProgramInfoViewModel.ProgramCertificateConveyance = PermitCertificateConveyanceNumber;
                AAWSProgramInfoViewModelOverView.PrimaryProviderName = GeneralInfo.PrimaryProviderName;
                AAWSProgramInfoViewModel.Subdivision = GeneralInfo.Subdivision;
                AAWSProgramInfoViewModelOverView.SecondaryProviderName = GeneralInfo.SecondaryProviderName;
                AAWSProgramInfoViewModelOverView.Date_Accepted = GeneralInfo.Date_Accepted;
                AAWSProgramInfoViewModelOverView.Complete_Correct = GeneralInfo.Complete_Correct;
                AAWSProgramInfoViewModelOverView.Hydrology = GeneralInfo.Hydrology == "Y" ? true : false;
                AAWSProgramInfoViewModelOverView.Legal_Availability = GeneralInfo.Legal_Availability == "Y" ? true : false;
                AAWSProgramInfoViewModel.FeeRates = V_CD_AW_APP_FEE_RATES.Get(x => x.PROGRAM_CODE == PermitCertificateConveyanceNumber.Substring(0, 2));
                AAWSProgramInfoViewModel.OverView = AAWSProgramInfoViewModelOverView;
                AAWSProgramInfoViewModel.Diagram= SP_AW_CONV_DIAGRAM.ConveyanceDiagram(PermitCertificateConveyanceNumber);
                return AAWSProgramInfoViewModel;
            }
            catch (FileNotFoundException e)
            {
                // FileNotFoundExceptions are handled here.
                return AAWSProgramInfoViewModel;
            }
            catch(Exception exception)
            {
                EmailService.Message(exception);
            }
        }

        public static AAWSProgramInfoViewModel OnUpdate(AAWSProgramInfoViewModel paramValues)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            using (var ctx = new OracleContext())
            {
                var application = ctx.V_AWS_GENERAL_INFO.Where(p => p.ProgramCertificateConveyance == paramValues.ProgramCertificateConveyance).FirstOrDefault<V_AWS_GENERAL_INFO>();
                application.Hydrology = paramValues.OverView.Hydrology == true ? "Y" : "N";
                application.Legal_Availability = paramValues.OverView.Legal_Availability == true ? "Y" : "N";
                ctx.SaveChanges();

                return AAWSProgramInfoViewModel;
            }
        }
    }
}