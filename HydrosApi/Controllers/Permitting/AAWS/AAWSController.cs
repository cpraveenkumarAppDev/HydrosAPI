
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
    using Models.Permitting.AAWS;
    using System.Collections.Generic;
    using ViewModel.Permitting.AAWS;
    using Models.ADWR;
    using Oracle.ManagedDataAccess.Client;
    using System.Data.Entity.Infrastructure;
    using System.Net.Http;
    using System.Net;

    public class AAWSController : ApiController
    {
        //subset approved by aaws team
        private static string BundleExceptions(Exception exception)
        {
            string fullException = exception.Message;
            if (exception.InnerException != null)
            {
                fullException += BundleExceptions(exception.InnerException);
            }

            return fullException;
        }

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
        /// <param name="amacode">ama</param>
        /// <returns>AMA, County, Basin, Subbasin list in a Hierarchy (in that order)</returns>
        /// <remarks>
        /// <para>When an ama is not provided, the entire list of all amas are returned</para>   
        /// <para>When SubbasinsInAMA=1, then County, Basin, Subbasin can be chosen automatically. 
        /// When SubbasinsInCounty=1, then subbasin can be selected automatically</para> 
        /// </remarks>

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpGet,Route("aws/getAmaCountyBasin/{amacode?}")]      
        public IHttpActionResult GetAMACountyBasin(string amacode=null)
        {
            var infoList = amacode == null ? AW_AMA_COUNTY_BASIN_SUBBAS.GetAll() :
                AW_AMA_COUNTY_BASIN_SUBBAS.GetList(a => a.Cama_code== amacode.ToUpper());

            return Ok(infoList.GroupBy(g=> new {g.AMA, g.Cama_code, g.AMA_INA_TYPE
                , DefaultBasinCode=g.Cama_code.Replace("X","0") != "0" ? g.BasinCode : null //When AMA/INA already has basin/subbasin assigned
                , DefaultBasinName = g.Cama_code.Replace("X", "0") != "0" ? g.BasinName : null})
              .Select(a => new { a.Key.AMA, a.Key.Cama_code, a.Key.AMA_INA_TYPE, a.Key.DefaultBasinCode, a.Key.DefaultBasinName,
                  AMAInfo = a.GroupBy(g => new { g.County_Descr, g.County_Code })
              .Select(c => new { c.Key.County_Descr, c.Key.County_Code,
                  Basin = c.GroupBy(g => new { g.BasinCode, g.BasinName, HasSubbasin = g.SubbasinCode != g.BasinCode && true }).Distinct().OrderBy(o=>o.Key.BasinName)
                .Select(i => new { i.Key.BasinCode, i.Key.BasinName, i.Key.HasSubbasin 
                , Subbasin = i.Select(s => new { s.BasinCode, s.BasinName, s.SubbasinCode, s.SubbasinName }).Distinct().OrderBy(o => o.SubbasinName)
              }).Distinct()
              }).OrderBy(o => o.County_Descr)
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
        [HttpGet, Route("aws/getAwFileByWrfId/{id}")]
        public IHttpActionResult GetAwFileByPcc(int id)
        {
            return Ok(AW_FILE.Get(p => p.WRF_ID == id));
        }
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPut, Route("aws/UpdateAwFile/{id}")]
        public async Task<IHttpActionResult> UpdateAwFile([FromBody] AW_FILE af, int id)
        {
            af.UPDATEBY = User.Identity.Name.Replace("AZWATER0\\", "");
            AW_FILE aw_file;

            using (var context = new OracleContext())
            {
                aw_file = context.AW_FILE.Where(x => x.WRF_ID == id).FirstOrDefault();
                if (aw_file != null)
                {
                    var props = aw_file.GetType().GetProperties().ToList();
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(af);
                        if (value != null)
                        {
                            prop.SetValue(aw_file, value);
                        }
                    }
                    await context.SaveChangesAsync();
                }
                return Ok(aw_file);
            }
        }
        [HttpGet, Route("aws/getAwCity")]
        public IHttpActionResult GetAwCity()
        {
            try
            {
                var awCityList = CD_AW_CITY.GetAll();
                return Ok(awCityList);
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
            }
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
            catch  
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
            catch 
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
            var record = new AW_APP_ACTIVITY_TRK
            {
                ACT_TRK_DT_TIME = DateTime.Now,
                WRF_ID = wrf,
                ActivityCode = "ISSD",
                CREATEBY = User.Identity.Name.Replace(@"AZWATER0\", "")
            };

            try
            {
                AW_APP_ACTIVITY_TRK.Add(record);
                return Ok("Created");
            }
            catch 
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
            catch
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
                codes = CD_AW_APP_ACTIVITY.GetList(x => x.ALSO_FILE_STATUS == "Y");
            }
            catch 
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
            catch  
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

        [HttpPost, Route("aws/customerbyany/")]
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
                      }).OrderBy(o => o.companyRank)
                            .ThenBy(o => o.addressRank)
                            .ThenBy(o => o.firstNameRank)
                            .ThenBy(o => o.lastNameRank)
                            .Select(s => new { s.result });

                if (!(customerList != null && customerList.Count() > 0))
                {
                    return Ok(new { Message = "No results were found for " + searchString });
                }
               
                var custWrfViewModelList = customerList.Select(x => new Aws_customer_wrf_ViewModel(x.result)).Take(20);
              
                return Ok(custWrfViewModelList);
            }
            catch (Exception exception)
            {                
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
                //log error
                //return InternalServerError();
            }
        }

        [HttpGet, Route("aws/customer/{wrf}/{custType?}")]
        public IHttpActionResult GetCustomerByWrf(int wrf, string custType = null)
        {
            try
            {
                var custIdList = WRF_CUST.GetList(x => x.WRF_ID == wrf).Select(x => x.CUST_ID).ToList().Distinct();
                List<Aws_customer_wrf_ViewModel> customerList = new List<Aws_customer_wrf_ViewModel>();
                foreach (var custId in custIdList)
                {
                    customerList.Add(new Aws_customer_wrf_ViewModel(custId, wrf, custType));
                }
                return Ok(customerList);
            }
            catch
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
            catch
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpPost, Route("aws/customer/{wrf}")]
        [Authorize]
        public IHttpActionResult CreateCustomer(int wrf, [FromBody] Aws_customer_wrf_ViewModel customer) //Create a customer or associated the customer with a wrf_id
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                //get Oracle USER_ID if available
                var foundUser = AW_USERS.Get(u => u.EMAIL.ToLower().Replace("@azwater.gov", "") == userName);
                string oracleUserID =  foundUser.USER_ID ?? null;
                var createDt = DateTime.Now;
                string appendCompanyName = "";

                userName =oracleUserID ?? userName; //Set to Oracle ID if possible
                          
                if(customer.Customer== null || customer.Waterrights==null)
                {
                    return BadRequest("Mandatory information for customer was not provided.");
                }   
                
                var existingCustomers = WRF_CUST.GetList(l => l.WRF_ID == wrf);

                if(existingCustomers==null)
                {
                   if(WTR_RIGHT_FACILITY.Get(l => l.ID == wrf)==null)
                    {
                        return BadRequest("The water right facility ID submitted is incorrect or doesn't exist.");
                    }                     
                }
                using (var context = new OracleContext())
                {
                    int? customerID = (int?)customer.Customer.CUST_ID ?? -1;

                    customer.Waterrights.Select(w => {
                        w.WRF_ID = wrf;
                        return w;
                    }).ToList();
                
                    //Customer is new, ensure required data is provided
                    if (!customer.IsValid() && customerID < 1) //if the customer doesn't exist, ensure that the appropriate data is being submitted
                    {
                        return BadRequest(String.Format("THE CUSTOMER WAS NOT CREATED BECAUSE THE FOLLOWING INFORMAION IS MISSING: {0}",customer.IsValidMsg()));
                    }
                    //Customer exists, verify the CCT_CODE is provided
                    else if(customerID > 0 && customer.Waterrights.Count()!=customer.Waterrights.Count(w=>w.CCT_CODE !=null))
                    {
                        return BadRequest("COULD NOT CREATE AN ASSOCIATION BECAUSE THE CCT CODE MUST BE PROVIDED");
                    }
                    
                    //Customer is new, create customer
                    if (customerID < 1) 
                    {
                        if (customer.Customer.COMPANY_LONG_NAME.Length > 60)
                        {
                            appendCompanyName = customer.Customer.COMPANY_LONG_NAME.Substring(60);
                            customer.Customer.COMPANY_LONG_NAME = customer.Customer.COMPANY_LONG_NAME.Substring(0, 60);
                        }

                        customer.Customer.BAD_ADDRESS_FLAG = customer.Customer.BAD_ADDRESS_FLAG == "false" ? "N" : "Y";
                        var rgrCustomer = new CUSTOMER(customer.Customer, userName)
                        {
                            CREATEBY = userName,
                            CREATEDT = createDt
                        };
                        context.CUSTOMER.Add(rgrCustomer);
                        context.SaveChanges();//need to save and get rgr.customer ID back from DB sequence to use in wrf_cust
                    
                        customerID= rgrCustomer.ID;
                        
                        if (customerID < 1)
                        {
                            return BadRequest("ERROR CREATING CUSTOMER");
                        }

                        if (appendCompanyName != "" && customerID > 0) //insert long name value into 
                        {
                            var customerLongName = new AW_CUST_LONG_NAME()
                            {
                                CUST_ID = customerID ?? -1,
                                COMPANY_LONG_NAME = appendCompanyName,
                                CREATEBY = userName,
                                CREATEDT = createDt
                            };
                            context.AW_CUST_LONG_NAME.Add(customerLongName);
                            context.SaveChanges();
                        }
                    }  //customer exists              
                   else
                    {
                        var findCust=CUSTOMER.Get(c => c.ID == customer.Customer.CUST_ID, context);
                        if(findCust==null)
                        {
                            BadRequest("COULD NOT FIND A MATCHING CUSTOMER FOR THE ID ENTERED");
                        }
                        else
                        {
                            customer.Customer=V_AWS_CUSTOMER_LONG_NAME.Get(c => c.CUST_ID == customer.Customer.CUST_ID, context);
                        }
                    }               
                    
                    var waterRights=customer.Waterrights.Select(w =>
                    {
                        w.CUST_ID = customerID ?? -1;                        
                        w.IS_ACTIVE = w.IS_ACTIVE ?? "Y";                      
                        w.LINE_NUM = ((from f in existingCustomers where f.CCT_CODE == w.CCT_CODE && f.WRF_ID==w.WRF_ID select f).Max(l => (int?)l.LINE_NUM) ?? 0) + 1; // Set line num 
                        w.CREATEBY = userName;
                        w.CREATEDT = createDt;
                        return w;
                    }).ToList();

                    if(waterRights==null)
                    {
                        BadRequest("THERE WAS AN UNKNOWN ERROR ADDING THE CUSTOMER");
                    }

                    context.WRF_CUST.AddRange(waterRights);                   
                    context.SaveChanges();                  
                    
                    return Ok(customer);
               }
           }
            catch (Exception exception)
            {
                
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
                //log error
                //return InternalServerError();
            }
        }        

        [HttpPut, Route("aws/customer/{custId}"), Authorize]
        public IHttpActionResult UpdateCustomer(int custId, Aws_customer_wrf_ViewModel customer)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                //get Oracle USER_ID if available
                var foundAwUser = AW_USERS.Get(u => u.EMAIL.ToLower().Replace("@azwater.gov", "") == userName);
                string oracleUserID = foundAwUser.USER_ID ?? null;
                var currentDt = DateTime.Now;
                var requestMethod = ActionContext.Request.Method.ToString().ToUpper(); //POST, DELETE ETC.
               
                userName = oracleUserID ?? userName; //Set to Oracle ID if possible

                using (var context = new OracleContext())
                {
                    var foundUser = V_AWS_CUSTOMER_LONG_NAME.Get(x => x.CUST_ID == custId, context);
                    var allWaterRights = WRF_CUST.GetList(x => x.CUST_ID == custId, context);
                    var foundWaterRights = allWaterRights.FirstOrDefault();

                    //if (requestMethod == "DELETE")
                    //{
                    var deleteMsg = "";

                    //Ensure IS_ACTIVE is "N". Set the CUST_ID value
                    var wrfCust = customer.Waterrights.Where(w => w.IS_ACTIVE == "N" && w.CUST_ID == custId);

                    if (wrfCust != null && wrfCust.Count()>0)
                    {
                        var deleteWrfCust = (from w in allWaterRights
                                             join c in wrfCust on new { w.WRF_ID, w.CUST_ID, w.CCT_CODE, w.LINE_NUM }
                                             equals new { c.WRF_ID, c.CUST_ID, c.CCT_CODE, c.LINE_NUM }
                                             select w).ToList();

                        if (deleteWrfCust == null || deleteWrfCust.Count() == 0)
                        {
                            return BadRequest("CUSTOMER RECORDS COULD NOT BE FOUND");
                        }

                        context.WRF_CUST.RemoveRange(deleteWrfCust); //Delete a customer association from the wrf_cust table
                        context.SaveChanges();
                        deleteMsg = string.Format("DELETED {0} WRF_CUST RECORD(S)", deleteWrfCust.Count());

                        var custCount = WRF_CUST.GetList(x => x.CUST_ID == custId, context).Count();

                        //When all the records have been deleted in WRF_CUST, remove corresponding customer records
                        if (custCount == 0)
                        {
                            var deleteCust = CUSTOMER.Get(c => c.ID == custId, context);
                            context.CUSTOMER.Remove(deleteCust);
                            context.SaveChanges();
                            deleteMsg = "WRF_CUST AND CUSTOMER RECORDS DELETED";

                            var deleteCustLongName = AW_CUST_LONG_NAME.Get(c => c.CUST_ID == custId, context);
                            if (deleteCustLongName != null)
                            {
                                context.AW_CUST_LONG_NAME.Remove(deleteCustLongName);
                                context.SaveChanges();
                                deleteMsg = "WRF_CUST, CUSTOMER AND LONG_NAME RECORDS DELETED";
                            }
                        }
                        return Ok(new { message = deleteMsg });
                    }
                    //return BadRequest("CUSTOMER RECORD COULD NOT BE DELETED BECAUSE " + (wrfCust == null ? "THE SPECIFIED ID WAS NOT FOUND" : "THE ACTIVE STATUS MUST BE SET TO N"));
                    // }

                    var rightsCount = allWaterRights != null ? allWaterRights.Select(w => w.WRF_ID).Distinct().Count() : 0;

                    if(rightsCount > 1)
                    {
                        return BadRequest(string.Format("THIS CUSTOMER IS ASSOCIATED WITH {0} RIGHTS AND CANNOT BE UPDATED",rightsCount));
                    }

                    //get customer properties
                    var custPropList = customer.Customer.GetType().GetProperties().ToList();
                    foreach (var prop in custPropList)
                    {
                        var tempVal = prop.GetValue(customer.Customer);
                        //if the Type is a valueType then make the default object and compare, otherwise it's a ref type and is compared to null
                        if (tempVal != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null))
                        {
                            if (prop.Name != "CUST_ID" && prop.Name != "CCT_CODE")//can't update the DB Key
                                prop.SetValue(foundUser, tempVal);
                        }
                    }

                    var rightProps = typeof(WRF_CUST).GetProperties().ToList();
                    foreach (var waterRight in customer.Waterrights)
                    {
                        var wrf_cust = WRF_CUST.Get(x => x.CUST_ID == waterRight.CUST_ID && x.WRF_ID == waterRight.WRF_ID && x.CCT_CODE == waterRight.CCT_CODE, context);
                        bool changesOccurred = false;
                        foreach (var prop in rightProps)
                        {
                            var tempValue = prop.GetValue(waterRight);
                            var incomingValue = prop.GetValue(wrf_cust);
                            if (tempValue != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null) && tempValue != incomingValue)
                            {
                                if (prop.Name != "WRF_ID" && prop.Name != "CUST_ID" && prop.Name != "CCT_CODE")
                                {
                                    prop.SetValue(wrf_cust, tempValue);
                                    changesOccurred = true;
                                }
                            }
                        }
                        if (changesOccurred)
                        {
                            waterRight.UPDATEBY = userName;
                            waterRight.UPDATEDT = currentDt;
                        }
                    }

                    context.SaveChanges();
                    var rgrCustomer = context.CUSTOMER.Where(x => x.ID == foundUser.CUST_ID).FirstOrDefault();
                    rgrCustomer.UPDATEBY = userName;
                    rgrCustomer.UPDATEDT = currentDt;
                    context.SaveChanges();
                    return Ok(customer);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
                //log error
                //return InternalServerError(exception);
            }
        }

        [HttpPost, Route("aws/customer/wrf")]
        public IHttpActionResult CreateWrfcust([FromBody] List<WRF_CUST> wrfcustList)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                using (var context = new OracleContext())
                {
                    foreach (var wrfcust in wrfcustList)
                    {
                        var customerExists = context.CUSTOMER.Where(x => x.ID == wrfcust.CUST_ID).FirstOrDefault() != null;
                        var wrfExists = context.WRF_CUST.Where(x => x.WRF_ID == wrfcust.WRF_ID).FirstOrDefault() != null;
                        var count = WRF_CUST.GetList(x => x.WRF_ID == wrfcust.WRF_ID && x.CUST_ID == wrfcust.CUST_ID && x.CCT_CODE == wrfcust.CCT_CODE).Count();
                        if (!customerExists || !wrfExists)
                        {
                            return BadRequest("customer or wrf does not exist");
                        }
                        if (count > -1)
                        {
                            wrfcust.CREATEBY = userName;
                            wrfcust.CREATEDT = DateTime.Now;
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
            catch
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

        [HttpGet, Route("aws/hydrobypcc/{pcc}")]
        public IHttpActionResult GetHydroByPcc(string pcc)
        {            
            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            pcc = regex.Replace(pcc, "$1-$2.$3");

            var hydroView = new AwsHydrologyViewModel(pcc);
            return Ok(hydroView);            
        }

        [HttpGet, Route("aws/hydrobyid/{id}")]
        public IHttpActionResult GetHydroByWrfId(int id)
        {
            var hydroView = new AwsHydrologyViewModel(id);
            return Ok(hydroView);
        }
    }
}