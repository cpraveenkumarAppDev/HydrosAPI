﻿
namespace HydrosApi.Controllers
{
    using System;
    using System.Web.Http;    
    using System.Threading.Tasks;
    using Models;
    using HydrosApi.ViewModel;    
    using System.Text.RegularExpressions;
    using HydrosApi.Data;
    using System.Linq;
    using HydrosApi.Models.Permitting.AAWS;
    using System.Collections.Generic;
    using HydrosApi.ViewModel.Permitting.AAWS;
    using HydrosApi.Models.ADWR;
    using Oracle.ManagedDataAccess.Client;
    using System.Data.Entity.Infrastructure;

    public class AAWSController : ApiController
    {
        //subset approved by aaws team
        private readonly Dictionary<string, string> AwsCustomerCodes = new Dictionary<string, string> { { "AS", "ASSIGNEE" }, { "BY", "BUYER" }, { "C", "CONTACT PARTY" }, { "CH", "CERTIFICATE HOLDER" }, { "CN", "CONSULTANT" }, { "MR", "MUNICIPAL REPRESENTATIVE" }, { "O", "OWNER" }, { "AP", "APPLICANT" } }; 

        // GET: AAWS
        //IRR-29-A16011018CBB-01
        [Route("aws/getgeneralInfo/")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {
            return Ok(V_AWS_GENERAL_INFO.GetAll());
        }

        [Route("aws/getgeneralInfo/{name}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfo(string name = null)
        {
            return Ok(V_AWS_GENERAL_INFO.Get(p => p.FileReviewer == name));
        }
        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [Route("aws/getgeneralInfoById/{id}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfoById(string id)
        {
            //this will format any pcc as long as the pattern is two numbers, six numbers, four numbers in it
            //so it can be all numbers or have characters as long as the character are in the correct locations
            //I'm sorry to change it
            
            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            var pcc = regex.Replace(id, "$1-$2.$3");
            // var pcc = regex.Replace("~", ".");
            //var found = AAWSProgramInfoViewModel.GetData(pcc);
            var found = V_AWS_GENERAL_INFO.GetGeneralInformation(pcc);
            return Json(found);
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [Route("aws/getAmaCountyBasin/{ama?}")]
        [HttpGet]
        public IHttpActionResult GetAMACountyBasin(string ama=null)
        {
            var infoList = ama == null ? AW_AMA_COUNTY_BASIN_SUBBAS.GetAll() :
                AW_AMA_COUNTY_BASIN_SUBBAS.GetList(a => a.AMA.ToUpper() == ama.ToUpper());

            return Ok(infoList.GroupBy(g => g.AMA)
               .Select(a => new { AMA = a.Key, AMAInfo = a.GroupBy(g => g.COUNTY)
               .Select(c => new { County = c.Key, Basin = c.GroupBy(g => new { g.BASIN_ABBR, g.BASIN_NAME })
               .Select(i => new { BasinAbbr = i.Key.BASIN_ABBR, BasinName = i.Key.BASIN_NAME
                    , Subbasin = i.Select(s => new { SubbasinAbbr=s.SUBBASIN_ABBR, SubbasinName=s.SUBBASIN_NAME }).Distinct() 
               })}).Distinct()
               }).OrderBy(o=>o.AMA != "OUTSIDE OF AMA OR INA" ? "_"+o.AMA : o.AMA).ToList());
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [Route("aws/getgeneralInfoByPcc/{id}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfoByPcc(string id)
        {
            //this will format any pcc as long as the pattern is two numbers, six numbers, four numbers in it
            //so it can be all numbers or have characters as long as the character are in the correct locations
            //I'm sorry to change it

            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            var pcc = regex.Replace(id, "$1-$2.$3");
            // var pcc = regex.Replace("~", ".");
            var found = V_AWS_GENERAL_INFO.GetGeneralInformation(pcc);
            return Ok(found);
        }

        [Route("aws/getCommentsByWrfId/{id}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfoByPcc(int id)
        {

            //Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            //var pcc = regex.Replace(id, "$1-$2.$3");
            // var pcc = regex.Replace("~", ".");
            var found = AWS_CommentsVM.GetComments(id);
            return Ok(found);
        }

        [Route("aws/GetNewAWSRight")]
        [HttpGet]
        public IHttpActionResult GetNewApplicationCredentials()
        {
            return Json(new AWSNewAppViewModel());
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpGet, Route("aws/diagram/{id}")]
        public IHttpActionResult GetConveyanceDiagram(string id) //New conveyance
        {

            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            var pcc = regex.Replace(id, "$1-$2.$3");
            return Ok(SP_AW_CONV_DIAGRAM.ConveyanceDiagram(pcc));

        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newconv")]
        public async Task<IHttpActionResult> AddNewConveyance([FromBody] SP_AW_INS paramValues) //New conveyance
        {                 
            var conveyance = await Task.FromResult(SP_AW_INS.CreateNewFile(paramValues, "conveyance"));

            if(conveyance==null)
            {
                return Ok(new { message = "A new conveyance was not created" });
               
            }
            else if(conveyance.Result.ProcessStatus != null)
            {
                return Ok(new { message = conveyance.Result.ProcessStatus });               
            }
             
            return Ok(conveyance);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newapp")]
        public async Task<IHttpActionResult> AddNewApplication([FromBody] SP_AW_INS paramValues) //New file
        {            
            var application=await Task.FromResult(SP_AW_INS.CreateNewFile(paramValues, "newApplication"));

            if (application == null)
            {
                return Ok(new { message = "A new application was not created" });
            }
            else if (application.Result.ProcessStatus != null)
            {
                return Ok(new { message = application.Result.ProcessStatus });
            }

            return Ok(application);
             
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPut, Route("aws/updateapp")]
        public async Task<IHttpActionResult> UpdateApp([FromBody] GenInfoWrapper paramValues)  //Send all form values
        {
            var user = User.Identity.Name;
            paramValues.Overview.UserName = user.Replace("AZWATER0\\", "");
            V_AWS_GENERAL_INFO genInfo;
            using(var context = new OracleContext())
            {
                genInfo = context.V_AWS_GENERAL_INFO.Where(x => x.ProgramCertificateConveyance == paramValues.Overview.ProgramCertificateConveyance).FirstOrDefault();
                var props = genInfo.GetType().GetProperties().ToList();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(paramValues.Overview);
                    if(value != null)
                    {
                        prop.SetValue(genInfo, value);
                        if (prop.Name == "SubbasinCode")
                        {
                            var hydro = context.V_AWS_HYDRO.Where(x => x.PCC == paramValues.Overview.ProgramCertificateConveyance).FirstOrDefault().SUBBASIN_CODE = value.ToString();
                        }
                    }
                }

                await context.SaveChangesAsync();
            }

            return Ok(genInfo);
        }

       /* [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPut, Route("aws/updateapp")]
        public IHttpActionResult UpdateApp([FromBody] AAWSProgramInfoViewModel paramValues) //New file
        {

            var user = User.Identity.Name;
            var savedApplication = AAWSProgramInfoViewModel.OnUpdate(paramValues, user.Replace("AZWATER0\\",""));
           

            return Ok(savedApplication);
        }*/

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/{wrf}/{activity}")]
        public IHttpActionResult GetActivity(int wrf, string activity)
        {
            var activities = new AW_APP_ACTIVITY_TRK();
            try
            {
                activities = AW_APP_ACTIVITY_TRK.GetList(x => x.WRF_ID == wrf && x.ActivityCode == activity).FirstOrDefault();
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(activities);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/issued/{wrf}")]
        public IHttpActionResult GetActivity(int wrf)
        {
            List<AW_APP_ACTIVITY_TRK> activities = new List<AW_APP_ACTIVITY_TRK>();
            try
            {
                activities = AW_APP_ACTIVITY_TRK.GetList(x => x.WRF_ID == wrf && new List<string>{"ISSD", "IADQ", "IIAD"}.Contains(x.ActivityCode)).OrderByDescending(x => x.CREATEDT).ToList();
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(activities.FirstOrDefault());
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPost, Route("aws/activity/{wrf}/{activityCode}")]
        public IHttpActionResult UpdateActivity(int wrf, string activityCode)
        {
            var record = new AW_APP_ACTIVITY_TRK();
            record.ACT_TRK_DT_TIME = DateTime.Now;
            record.WRF_ID = wrf;
            record.ActivityCode = "ISSD";
            record.CREATEBY = User.Identity.Name.Replace(@"AZWATER0\", "");
            try
            {
                AW_APP_ACTIVITY_TRK.Add(record);
                return Ok("Created");
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/{pcc}")]
        public IHttpActionResult GetAllActivity(int pcc)
        {
            List<AW_APP_ACTIVITY_TRK> activities = null;
            var context = new OracleContext();
            object activitiesDescribed;
            try
            {
                activities = AW_APP_ACTIVITY_TRK.GetList(x => x.WRF_ID == pcc);
                activitiesDescribed = activities.Join(context.CD_AW_APP_ACTIVITY,
                    act => act.ActivityCode,
                    code => code.CODE,
                    (act, code) => new
                    {
                        Activity = code.DESCR,
                        Date = act.CREATEDT
                    });
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(activitiesDescribed);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/codes")]
        public IHttpActionResult GetActivityCodes()
        {
            List<CD_AW_APP_ACTIVITY> codes;
            try
            {
                codes = CD_AW_APP_ACTIVITY.GetAll();
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(codes);
        }
        [HttpGet, Route("aws/getCommon")]
        public IHttpActionResult GetCommonAWS()
        {
            try
            {
                var info = new Common_ViewModel();
                return Ok(info);
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpGet, Route("aws/customer/{wrf}/{custType?}")]
        public IHttpActionResult GetCustomerByWrf(int wrf, string custType = null)
        {
            try
            {
                var custIdList = WRF_CUST.GetList(x => x.WRF_ID == wrf).Select(x => x.CUST_ID).ToList();
                List<Aws_customer_wrf_ViewModel> customerList = new List<Aws_customer_wrf_ViewModel>();
                foreach(var custId in custIdList)
                {
                    customerList.Add(new Aws_customer_wrf_ViewModel(custId, wrf, custType));
                }
                return Ok(customerList);
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpGet, Route("aws/customer/")]
        public IHttpActionResult GetCustomerByName(string firstName = null, string lastName = null)
        {
            try
            {
                using (var context = new OracleContext())
                {
                    var customerList = new List<V_AWS_CUSTOMER_LONG_NAME>();
                    if (firstName != null)
                    {
                        var query = context.V_AWS_CUSTOMER_LONG_NAME.Where(x => x.FIRST_NAME.ToLower().Contains(firstName.ToLower()));
                        if (lastName != null)
                        {
                            customerList = query.Where(x => x.LAST_NAME.ToLower().Contains(lastName.ToLower())).ToList();
                        }
                        else
                        {
                            customerList = query.ToList();
                        }
                    }
                    else if (lastName != null)
                    {
                        customerList = context.V_AWS_CUSTOMER_LONG_NAME.Where(x => x.LAST_NAME.ToLower().Contains(lastName.ToLower())).ToList();
                    }

                    var custWrfViewModelList = customerList.Select(x => new Aws_customer_wrf_ViewModel(x));

                    return Ok(custWrfViewModelList);
                }
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpGet, Route("aws/company/{company}")]
        public IHttpActionResult GetCustomerByCompany(string company)
        {            
            try
            {                 
                var customerList = V_AWS_CUSTOMER_LONG_NAME.GetList(co => co.COMPANY_LONG_NAME.Contains(company.ToUpper()));
                var custWrfViewModelList = customerList.Select(x => new Aws_customer_wrf_ViewModel(x));
                return Ok(custWrfViewModelList);
               
            }
            catch //(Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpPost, Route("aws/customer/{wrf}")]
        [Authorize]
        public IHttpActionResult CreateCustomer(int wrf, [FromBody] Aws_customer_wrf_ViewModel customer)
        {
            try
            {                
                using (var context = new OracleContext())
                {
                    //check for required properties
                    if (!customer.IsValid())
                    {
                        return BadRequest();
                    }
                    customer.Customer.BAD_ADDRESS_FLAG = customer.Customer.BAD_ADDRESS_FLAG == "false" ? "N" : "Y";
                    var rgrCustomer = new CUSTOMER(customer.Customer, User.Identity.Name.Replace("AZWATER0\\", ""));
                    context.CUSTOMER.Add(rgrCustomer);
                    context.SaveChanges();//need to save and get rgr.customer ID back from DB sequence to use in wrf_cust
                    customer.Customer.CUST_ID = rgrCustomer.ID;

                    foreach(var waterright in customer.Waterrights)
                    {
                        waterright.WRF_ID = wrf;
                        waterright.CUST_ID = rgrCustomer.ID;
                        context.WRF_CUST.Add(waterright);
                    }
                    context.SaveChanges();

                    return Ok(customer);
                }
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpPatch, Route("aws/customer/{custId}")]
        public IHttpActionResult UpdateCustomer(int custId, Aws_customer_wrf_ViewModel customer)
        {
            try
            {

                using (var context = new OracleContext())
                {
                    var foundUser = V_AWS_CUSTOMER_LONG_NAME.Get(x => x.CUST_ID == custId, context);
                    var foundWaterRights = WRF_CUST.Get(x => x.CUST_ID == custId, context);
                    //get customer properties
                    var custPropList = customer.Customer.GetType().GetProperties().ToList();
                    foreach (var prop in custPropList)
                    {
                        var tempVal = prop.GetValue(customer.Customer);
                        //if the Type is a valueType then make the default object and compare, otherwise it's a ref type and is compared to null
                        if (tempVal != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null))
                        {
                            if(prop.Name != "CUST_ID")//can't update the DB Key
                                prop.SetValue(foundUser, tempVal);
                        }
                    }

                    var rightProps = typeof(WRF_CUST).GetProperties().ToList();
                    foreach(var waterRight in customer.Waterrights)
                    {
                        var wrf_cust = WRF_CUST.Get(x => x.CUST_ID == waterRight.CUST_ID && x.WRF_ID == waterRight.WRF_ID, context);
                        foreach(var prop in rightProps)
                        {
                            var tempValue = prop.GetValue(waterRight);
                            var incomingValue = prop.GetValue(wrf_cust);
                            if (tempValue != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null) && tempValue != incomingValue)
                            {
                                if(prop.Name != "WRF_ID" && prop.Name != "CUST_ID")
                                    prop.SetValue(wrf_cust, tempValue);
                            }
                        }
                    }                    

                    context.SaveChanges();

                    return Ok(customer);
                }
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpGet, Route("aws/customer/types/")]
        public IHttpActionResult GetCustomerTypeCodes()
        {
            var custCodeList = AwsCustomerCodes.Select(item => item.Key).ToList();
            var codes = CD_CUST_TYPE.GetList(x => custCodeList.Contains(x.CODE));
            return Ok(codes);
        }
    }
}