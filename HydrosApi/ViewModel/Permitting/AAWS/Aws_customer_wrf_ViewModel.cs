using HydrosApi.Models.ADWR;
using HydrosApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.ViewModel.Permitting.AAWS
{
    public class Aws_customer_wrf_ViewModel
    {
        public V_AWS_CUSTOMER_LONG_NAME Customer { get; set; }
        public List<WRF_CUST> Waterrights { get; set; }

        public Aws_customer_wrf_ViewModel()
        {

        }

        public Aws_customer_wrf_ViewModel(int custId, int wrf, string custType)
        {
            try
            {
                this.Customer = V_AWS_CUSTOMER_LONG_NAME.Get(x => x.CUST_ID == custId);
                if(custType == null)
                {
                    this.Waterrights = WRF_CUST.GetList(x => x.WRF_ID == wrf && x.CUST_ID == custId);
                }
                else
                {
                    this.Waterrights = WRF_CUST.GetList(x => x.WRF_ID == wrf && x.CUST_ID == custId && x.CCT_CODE.ToLower() == custType.ToLower());
                }
            }
            catch(Exception exception)
            {
                var a = 1;
            }
        }
    }
}