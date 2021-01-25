
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

        /// <summary>
        /// aws/getAmaCountyBasin/{ama?}
        /// </summary>
        /// <param name="ama">ama</param>
        /// <returns>AMA, County, Basin, Subbasin list in a Hierarchy (in that order)</returns>
        /// <remarks>
        /// <para>When an ama is not provided, the entire list of all amas are returned</para>   
        /// </remarks>

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpGet,Route("aws/getAmaCountyBasin/{ama?}")]      
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
        [HttpGet,Route("aws/getgeneralInfoByPcc/{id}")]        
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

        [HttpGet,Route("aws/getCommentsByWrfId/{id}")]
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
                var custIdList = WRF_CUST.GetList(x => x.WRF_ID == wrf).Select(x => x.CUST_ID).ToList().Distinct();
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

        /// <summary>
        /// aws/company/{company} Search for a company (COMPANY_LONG_NAME) field.
        /// </summary>
        /// <param name="company">company</param>
        /// <returns>Returns top 20 results with relevance ranking</returns>
        /// <remarks>
        /// <para>Search Ranking Description</para>
        /// if the search string is equal to the result string, pad result with 3 spaces                       
        /// otherwise, prepend the formatted index of the search string within the result string   
        /// </remarks>

        [HttpGet, Route("aws/company/{company}")]
        public IHttpActionResult GetCustomerByCompany(string company)
        {            
            try
            {
                if (company == null)
                {
                    return Ok(new { Message = "Please enter a company name" });
                }
                var customerList = V_AWS_CUSTOMER_LONG_NAME.GetList(co => co.COMPANY_LONG_NAME.ToLower().Contains(company.ToLower()))
                    .Select(result => new
                    {
                        companyRank = company != null && result.COMPANY_LONG_NAME != null ? result.COMPANY_LONG_NAME.ToLower() == company.ToLower()                                          
                        ? new string(' ',3)+result.COMPANY_LONG_NAME.ToLower()
                        : String.Format("{0:D20}{1}",result.COMPANY_LONG_NAME.ToLower().IndexOf(company.ToLower()),result.COMPANY_LONG_NAME.ToLower()) : null,                          

                        result
                    })
                     .OrderBy(o =>o.companyRank)
                     .Select(s => s.result).Take(20);

                var custWrfViewModelList = customerList.Select(x => new Aws_customer_wrf_ViewModel(x));
                return Ok(custWrfViewModelList);                     
            }
            catch //(Exception exception)
            {
                //log error
                return InternalServerError();
            }
        }

        /// <summary>
        /// Advanced Search for Company, First Name, Last Name and Address1 fields.
        /// </summary>
        /// <param name="customer"> V_AWS_CUSTOMER_LONG_NAME customer (JSON object matching view structure) </param>
        /// <returns>Returns top 20 results with relevance ranking</returns>
        /// <remarks>
        /// <para>Search Ranking Description</para>
        /// <para>If the search string is equal to the result string, pad result with 3 spaces                       
        /// otherwise, prepend the formatted index of the search string within the result string</para>
        /// Order by the ranking of the found results
        /// </remarks>

        [HttpGet, Route("aws/customerbyany/")]
        public IHttpActionResult GetCustomerByAny([FromBody] V_AWS_CUSTOMER_LONG_NAME customer)
        {            
            try
            {             
                string firstname = customer.FIRST_NAME;
                string lastname = customer.LAST_NAME;
                string company = customer.COMPANY_LONG_NAME;
                string address1 = customer.ADDRESS1;

                if (firstname == null && lastname == null && company == null && address1 == null)
                {
                    return Ok(new { Message = "At least one search term must be entered (First Name, Last Name, Company Name or Address1/Care of" });                    
                }

                var searchString = String.Format("FirstName={0} LastName={1} Company={2} Address1={3}", firstname, lastname, company, address1);
                var customerList = V_AWS_CUSTOMER_LONG_NAME.GetList(
                  c =>
                      ((company != null && c.COMPANY_LONG_NAME.ToLower().Contains(company.ToLower())) || company == null) &&
                      ((firstname != null && c.FIRST_NAME.ToLower().Contains(firstname.ToLower())) || firstname == null) &&
                      ((lastname != null && c.LAST_NAME.ToLower().Contains(lastname.ToLower())) || lastname == null) &&
                      ((address1 != null && c.ADDRESS1.ToLower().Contains(address1.ToLower())) || address1 == null)
                      ).Select(result => new
                      {
                          companyRank = company != null && result.COMPANY_LONG_NAME != null ?
                          result.COMPANY_LONG_NAME.ToLower() == company.ToLower() ? new string(' ', 3) + result.COMPANY_LONG_NAME.ToLower()
                          : String.Format("{0:D20}{1}", result.COMPANY_LONG_NAME.ToLower().IndexOf(company.ToLower()), result.COMPANY_LONG_NAME.ToLower()) : null,

                          firstNameRank = firstname != null && result.FIRST_NAME != null ?
                          result.FIRST_NAME.ToLower() == firstname.ToLower() ? new string(' ', 3) + result.FIRST_NAME.ToLower()
                          : String.Format("{0:D20}{1}", result.FIRST_NAME.ToLower().IndexOf(firstname.ToLower()), result.FIRST_NAME.ToLower()) : null,

                          lastNameRank = lastname != null && result.LAST_NAME != null ?
                          result.LAST_NAME.ToLower() == lastname.ToLower() ? new string(' ', 3) + result.LAST_NAME.ToLower()
                          : String.Format("{0:D20}{1}", result.LAST_NAME.ToLower().IndexOf(lastname.ToLower()), result.LAST_NAME.ToLower()) : null,

                          addressRank = address1 != null && result.ADDRESS1 != null ?
                          result.ADDRESS1.ToLower() == address1.ToLower() ? new string(' ', 3) + result.ADDRESS1.ToLower()
                          : String.Format("{0:D20}{1}", result.ADDRESS1.ToLower().IndexOf(address1.ToLower()), result.ADDRESS1.ToLower()) : null,   

                          result
                     })                    
                    .OrderBy(o => o.companyRank)
                    .ThenBy(o => o.addressRank)
                    .ThenBy(o => o.firstNameRank)
                    .ThenBy(o => o.lastNameRank)
                    .Select(s=>s.result);                    

                if (!(customerList != null && customerList.Count() > 0))
                {
                    return Ok(new { Message = "No results were found for " + searchString });
                }
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
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                using (var context = new OracleContext())
                {
                    //check for required properties
                    if (!customer.IsValid())
                    {
                        return BadRequest();
                    }
                    customer.Customer.BAD_ADDRESS_FLAG = customer.Customer.BAD_ADDRESS_FLAG == "false" ? "N" : "Y";
                    var rgrCustomer = new CUSTOMER(customer.Customer, userName);
                    rgrCustomer.CREATEBY = User.Identity.Name.Replace("AZWATER0\\", "");
                    rgrCustomer.CREATEDT = DateTime.Now;
                    context.CUSTOMER.Add(rgrCustomer);
                    context.SaveChanges();//need to save and get rgr.customer ID back from DB sequence to use in wrf_cust
                    customer.Customer.CUST_ID = rgrCustomer.ID;

                    foreach(var waterright in customer.Waterrights)
                    {
                        waterright.WRF_ID = wrf;
                        waterright.CUST_ID = rgrCustomer.ID;
                        waterright.LINE_NUM = 1;
                        waterright.CREATEBY = userName;
                        waterright.CREATEDT = DateTime.Now;
                        waterright.IS_ACTIVE = "Y";
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

        [HttpPut, Route("aws/customer/{custId}"), Authorize]
        public IHttpActionResult UpdateCustomer(int custId, Aws_customer_wrf_ViewModel customer)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
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
                            if(prop.Name != "CUST_ID" && prop.Name != "CCT_CODE")//can't update the DB Key
                                prop.SetValue(foundUser, tempVal);
                        }
                    }

                    var rightProps = typeof(WRF_CUST).GetProperties().ToList();
                    foreach(var waterRight in customer.Waterrights)
                    {
                        waterRight.UPDATEBY = userName;
                        waterRight.UPDATEDT = DateTime.Now;
                        var wrf_cust = WRF_CUST.Get(x => x.CUST_ID == waterRight.CUST_ID && x.WRF_ID == waterRight.WRF_ID && x.CCT_CODE == waterRight.CCT_CODE, context);
                        foreach(var prop in rightProps)
                        {
                            var tempValue = prop.GetValue(waterRight);
                            var incomingValue = prop.GetValue(wrf_cust);
                            if (tempValue != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null) && tempValue != incomingValue)
                            {
                                if(prop.Name != "WRF_ID" && prop.Name != "CUST_ID" && prop.Name != "CCT_CODE")
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

        [HttpPost, Route("aws/customer/wrf")]
        public IHttpActionResult CreateWrfcust([FromBody] List<WRF_CUST> wrfcustList)
        {
            try
            {
                using(var context = new OracleContext())
                {
                    foreach (var wrfcust in wrfcustList)
                    {
                        var customerExists = context.CUSTOMER.Where(x => x.ID == wrfcust.CUST_ID).FirstOrDefault() != null ? true : false;
                        var wrfExists = context.WRF_CUST.Where(x => x.WRF_ID == wrfcust.WRF_ID).FirstOrDefault() != null ? true : false;
                        var count = WRF_CUST.GetList(x => x.WRF_ID == wrfcust.WRF_ID && x.CUST_ID == wrfcust.CUST_ID && x.CCT_CODE == wrfcust.CCT_CODE).Count();
                        if (!customerExists || !wrfExists)
                        {
                            return BadRequest("customer or wrf does not exist");
                        }
                        if (count > -1)
                        {
                            context.WRF_CUST.Add(wrfcust);
                        }
                        else
                        {
                            return BadRequest("wrf, cust, custType record already exists");
                        }
                    }
                    context.SaveChanges();
                    return Ok(wrfcustList);
                }

            }
            catch(Exception exception)
            {
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