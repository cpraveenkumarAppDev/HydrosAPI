
using HydrosApi.Controllers.Permitting.AAWS;
using HydrosApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace HydrosApi.ViewModel.AAWS
{
    public class AAWSProgramInfoViewModel
    {
        public AWS_OVER_VIEW OverView { get; set; }
        public string ProgramCertificateConveyance { get; set; }
        public static AAWSProgramInfoViewModel GetData(string PermitCertificateConveyanceNumber)
        {
            AAWSProgramInfoViewModel AAWSProgramInfoViewModel = new AAWSProgramInfoViewModel();
            AWS_OVER_VIEW AAWSProgramInfoViewModelOverView = new AWS_OVER_VIEW();

            var GeneralInfo = V_AWS_GENERAL_INFO.Get(p => p.ProgramCertificateConveyance == PermitCertificateConveyanceNumber);

            AAWSProgramInfoViewModel.ProgramCertificateConveyance = PermitCertificateConveyanceNumber;
            AAWSProgramInfoViewModelOverView.PrimaryProviderName = GeneralInfo.PrimaryProviderName;
            AAWSProgramInfoViewModelOverView.SecondaryProviderName = GeneralInfo.SecondaryProviderName;
            AAWSProgramInfoViewModelOverView.Date_Accepted = GeneralInfo.Date_Accepted;
            AAWSProgramInfoViewModelOverView.Complete_Correct = GeneralInfo.Complete_Correct;

            AAWSProgramInfoViewModel.OverView = AAWSProgramInfoViewModelOverView;
            return AAWSProgramInfoViewModel;
        }
    }
}