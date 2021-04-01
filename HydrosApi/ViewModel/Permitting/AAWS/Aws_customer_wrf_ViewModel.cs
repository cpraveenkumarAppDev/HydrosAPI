﻿using HydrosApi.Models.ADWR;
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
        public Aws_customer_wrf_ViewModel(V_AWS_CUSTOMER_LONG_NAME customer, List<WRF_CUST> wrfCustList)
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
                this.Customer = V_AWS_CUSTOMER_LONG_NAME.Get(x => x.CUST_ID == custId);
                var wrfCust = WRF_CUST.GetList(w => w.CUST_ID == Customer.CUST_ID);                

                if (custType == null)
                {
                    this.Waterrights = wrfCust.Where(x => x.WRF_ID == wrf).ToList();
                }
                else
                {
                    this.Waterrights = wrfCust.Where(x=>x.WRF_ID == wrf && x.CCT_CODE.ToLower() == custType.ToLower()).ToList();                    
                }

                if(wrfCust != null)
                {
                    this.WaterRightsCount = wrfCust.Count();
                    this.PccList = (from w in wrfCust
                                   join c in V_AWS_CUSTOMER.GetList(x => x.CUST_ID == Customer.CUST_ID) on w.WRF_ID equals c.WRF_ID into validCustomers
                                   from v in validCustomers.DefaultIfEmpty()
                                   select new
                                   {
                                       //w.WRF_ID,
                                       //AWS_WRF_ID = v== null ? 0 : v.WRF_ID,
                                       PCC = WTR_RIGHT_FACILITY.Get(f => f.ID == w.WRF_ID).PCC + (v == null ? "*" : "")
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
        public Aws_customer_wrf_ViewModel(V_AWS_CUSTOMER_LONG_NAME customer)
        {
            this.Customer = customer;
            this.Waterrights = WRF_CUST.GetList(x => x.CUST_ID == customer.CUST_ID).OrderBy(x => x.WRF_ID).ToList();
            this.WaterRightsCount = this.Waterrights != null ? this.Waterrights.Count() : 0;
        }
                       
        public string IsValidMsg()
        {
            var msg = "";

            if (this.Customer.ADDRESS1 == null)
            {
                msg += " ADDRESS1, ";
            }
            if (this.Customer.CITY == null)
            {
                msg += " CITY, ";
            }
            if (this.Customer.STATE == null)
            {
                msg += " STATE, "; 
            }
            if (this.Customer.COMPANY_LONG_NAME == null && (this.Customer.FIRST_NAME == null || this.Customer.LAST_NAME == null))
            {
                msg += " COMPANY NAME or FIRST NAME and LAST NAME, ";
            }
            if (this.Waterrights.Count > 0)
            {
                foreach (var waterright in this.Waterrights)
                {
                    if (waterright.CCT_CODE == null || waterright.CCT_CODE == "" || waterright.WRF_ID == 0)
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
            if(this.Waterrights.Count > 0)
            {
                foreach (var waterright in this.Waterrights)
                {
                    if (waterright.CCT_CODE == null || waterright.CCT_CODE == "" || waterright.WRF_ID == 0)
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