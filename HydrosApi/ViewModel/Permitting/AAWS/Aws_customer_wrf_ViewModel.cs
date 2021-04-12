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
        public VAwsCustomerLongName Customer { get; set; }
        public List<WaterRightFacilityCustomer> Waterrights { get; set; }
        public List<string> PccList { get; set; }
        public int WaterRightsCount { get; set; }
      

        public Aws_customer_wrf_ViewModel()
        {

        }
        /// <summary>
        /// Use when creating a new customer and wrf_cust relation, no wrf lookup as new customers own't have any
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="wrfCustList"></param>
        public Aws_customer_wrf_ViewModel(VAwsCustomerLongName customer, List<WaterRightFacilityCustomer> wrfCustList)
        {
            this.Customer = customer;
            this.Waterrights = wrfCustList;
            this.WaterRightsCount = wrfCustList != null ? wrfCustList.Count() : 0;
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
                this.Customer = VAwsCustomerLongName.Get(x => x.CustomerId == custId);
                var wrfCust = WaterRightFacilityCustomer.GetList(w => w.CustomerId == Customer.CustomerId);                

                if (custType == null)
                {
                    this.Waterrights = wrfCust.Where(x => x.WaterRightFacilityId == wrf).ToList();
                }
                else
                {
                    this.Waterrights = wrfCust.Where(x=>x.WaterRightFacilityId == wrf && x.CustomerTypeCode.ToLower() == custType.ToLower()).ToList();                    
                }

                if(wrfCust != null)
                {
                    this.WaterRightsCount = wrfCust.Count();
                    this.PccList = (from w in wrfCust
                                   join c in VAwsCustomer.GetList(x => x.CustomerId == Customer.CustomerId) on w.WaterRightFacilityId equals c.WaterRightFacilityId into validCustomers
                                   from v in validCustomers.DefaultIfEmpty()
                                   select new
                                   {
                                       //w.WRF_ID,
                                       //AWS_WRF_ID = v== null ? 0 : v.WRF_ID,
                                       PCC = WaterRightFacility.Get(f => f.Id == w.WaterRightFacilityId).PCC + (v == null ? "*" : "")
                                   }).Distinct().Select(x => x.PCC).ToList();
                    
                }
                //var wrfId=WRF_CUST.GetList(w => w.CUST_ID == Customer.CUST_ID).Select(w => w.WRF_ID).ToList();
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
        public Aws_customer_wrf_ViewModel(VAwsCustomerLongName customer)
        {
            this.Customer = customer;
            this.Waterrights = WaterRightFacilityCustomer.GetList(x => x.CustomerId == customer.CustomerId).OrderBy(x => x.WaterRightFacilityId).ToList();
            this.WaterRightsCount = this.Waterrights != null ? this.Waterrights.Count() : 0;
        }
                       
        public string IsValidMsg()
        {
            var msg = "";

            //if (this.Customer.ADDRESS1 == null)
            //{
            //    msg += " ADDRESS1, ";
            //}
            //if (this.Customer.CITY == null)
            //{
            //    msg += " CITY, ";
            //}
            //if (this.Customer.STATE == null)
            //{
            //    msg += " STATE, "; 
            //}
            

            if (this.Customer.CompanyLongName == null && (this.Customer.FirstName == null || this.Customer.LastName == null))
                {
                msg += " COMPANY NAME or FIRST NAME and LAST NAME, ";
            }
            if (this.Waterrights.Count > 0)
            {
                foreach (var waterright in this.Waterrights)
                {
                    if (waterright.CustomerTypeCode == null || waterright.CustomerTypeCode == "" || waterright.WaterRightFacilityId == 0)
                    {
                        msg += " CUSTOMER TYPE CODE or WATER RIGHT ID, ";
                    }
                }
            }
            else
            {
                msg += " CUSTOMER INFORMATION, ";
            }

            if(msg !="")
            {
                return msg.Substring(0, msg.Length - 2);
            }
            return null;
        }

        public bool IsValid()
        {
            var isValid = true;
           /* if (this.Customer.Address1 == null)
            {
                isValid = false;
            }
            if (this.Customer.City == null)
            {
                isValid = false;
            }
            if (this.Customer.State == null)
            {
                isValid = false;
            }*/
            if (this.Customer.CompanyLongName == null && (this.Customer.FirstName == null || this.Customer.LastName == null))
            {
                isValid = false;
            }
            if(this.Waterrights.Count > 0)
            {
                foreach (var waterright in this.Waterrights)
                {
                    if (waterright.CustomerTypeCode == null || waterright.CustomerTypeCode == "" || waterright.WaterRightFacilityId == 0)
                    {
                        isValid = false;
                    }
                }
            }
            else
            {
                isValid = false;
            }
            
            return isValid;
        }
    }
}