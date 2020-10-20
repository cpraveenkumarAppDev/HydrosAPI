namespace HydrosApi.ViewModel
{

    using Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using HydrosApi.Data;
    using Oracle.ManagedDataAccess.Client;

    public class AAWSProgramInfoViewModel
    {
        public AWS_OVER_VIEW OverView { get; set; }
        public string ProgramCertificateConveyance { get; set; }
        public string Subdivision { get; set; }
        public V_CD_AW_APP_FEE_RATES FeeRates { get; set; }

        public List<SP_AW_CONV_DIAGRAM> Diagram {get; set; }
        public static AAWSProgramInfoViewModel GetData(string PermitCertificateConveyanceNumber)
        {
            AAWSProgramInfoViewModel programInfoViewModel = new AAWSProgramInfoViewModel();
            AWS_OVER_VIEW programInfoViewModelOverView = new AWS_OVER_VIEW();

            try
            {
                var GeneralInfo = V_AWS_GENERAL_INFO.Get(p => p.ProgramCertificateConveyance == PermitCertificateConveyanceNumber);

                programInfoViewModel.ProgramCertificateConveyance = PermitCertificateConveyanceNumber;
                programInfoViewModelOverView.PrimaryProviderName = GeneralInfo.PrimaryProviderName;
                programInfoViewModel.Subdivision = GeneralInfo.Subdivision;
                programInfoViewModelOverView.SecondaryProviderName = GeneralInfo.SecondaryProviderName;
                programInfoViewModelOverView.Date_Accepted = GeneralInfo.Date_Accepted;
                programInfoViewModelOverView.Complete_Correct = GeneralInfo.Complete_Correct;
                programInfoViewModel.OverView = programInfoViewModelOverView;
                programInfoViewModel.Diagram= SP_AW_CONV_DIAGRAM.ConveyanceDiagram(PermitCertificateConveyanceNumber);
                return programInfoViewModel;
            }
            catch (FileNotFoundException e)
            {
                // FileNotFoundExceptions are handled here.
                return programInfoViewModel;
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