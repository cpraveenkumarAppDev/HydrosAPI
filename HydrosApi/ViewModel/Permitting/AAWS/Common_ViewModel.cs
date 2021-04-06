using HydrosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.ViewModel.Permitting.AAWS
{
    public class Common_ViewModel
    {
        public List<VAwsProvider> ProvidersList { get; set; }

        public Common_ViewModel()
        {
            this.ProvidersList = VAwsProvider.GetAll();
        }
    }
}