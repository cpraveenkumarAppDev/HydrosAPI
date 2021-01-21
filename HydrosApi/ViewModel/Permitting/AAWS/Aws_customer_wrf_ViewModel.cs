using HydrosApi.Models.ADWR;
using HydrosApi.Models.Permitting.AAWS;
using HydrosApi.Services;
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
        /// <summary>
        /// Use when creating a new customer and wrf_cust relation, no wrf lookup as new customers own't have any
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="wrfCust"></param>
        public Aws_customer_wrf_ViewModel(V_AWS_CUSTOMER_LONG_NAME customer, List<WRF_CUST> wrfCustList)
        {
            this.Customer = customer;
            this.Waterrights = wrfCustList;
        }
        /// <summary>
        /// finds customer by ID and CustomerType and queries wrf_cust relations by wrf
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="wrf"></param>
        /// <param name="custType"></param>
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
                var sentOkay = EmailService.Message("appdev@azwater.gov", $"{Environment.MachineName} - HydrosAPI", $"{exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }
        /// <summary>
        /// Yields all wrf_cust relations
        /// </summary>
        /// <param name="customer"></param>
        public Aws_customer_wrf_ViewModel(V_AWS_CUSTOMER_LONG_NAME customer)
        {
            this.Customer = customer;
            this.Waterrights = WRF_CUST.GetList(x => x.CUST_ID == customer.CUST_ID).OrderBy(x => x.WRF_ID).ToList();
        }

        public bool IsValid()
        {
            var isValid = true;
            if (this.Customer.ADDRESS1 == null)
            {
                isValid = false;
            }
            if (this.Customer.CITY == null)
            {
                isValid = false;
            }
            if (this.Customer.STATE == null)
            {
                isValid = false;
            }
            if (this.Customer.COMPANY_LONG_NAME == null && (this.Customer.FIRST_NAME == null || this.Customer.LAST_NAME == null))
            {
                isValid = false;
            }

            foreach(var waterright in this.Waterrights)
            {
                if(waterright.CCT_CODE == null || waterright.CCT_CODE == "" ||waterright.WRF_ID == 0 )
                {
                    isValid = false;
                }
            }
            return isValid;
        }
    }
}