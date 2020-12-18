using HydrosApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.ViewModel.Permitting.AAWS
{
    public class Common_ViewModel
    {
        public List<V_AWS_PROVIDER> ProvidersList { get; set; }

        public Common_ViewModel()
        {
            this.ProvidersList = V_AWS_PROVIDER.GetAll();
        }
    }
}