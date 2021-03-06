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
    using HydrosApi.Services;
    using HydrosApi.ViewModel.ADWR;

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

        /*private static string GetBestUsername(string user)
        {
            string userName = user.Replace("AZWATER0\\", "");
            //get Oracle USER_ID if available
            var foundUser = AwUsers.Get(u => u.Email.ToLower().Replace("@azwater.gov", "") == userName);
            string oracleUserID = foundUser != null ? foundUser.UserId : null;            
            return oracleUserID ?? userName; //Set to Oracle ID if possible
        }*/

        // GET: AAWS
        //IRR-29-A16011018CBB-01
        [Route("aws/getgeneralInfo/")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfo()
        {

            return Ok(VAwsGeneralInfo.GetAll());
        }

        [Route("aws/getgeneralInfo/{name}")]
        [HttpGet]
        public IHttpActionResult GetGeneralInfo(string name = null)
        {

            return Ok(VAwsGeneralInfo.Get(p => p.FileReviewer == name));
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
            var found = VAwsGeneralInfo.GetGeneralInformation(pcc);
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
        [HttpGet, Route("aws/getAmaCountyBasin/{amacode?}")]
        public IHttpActionResult GetAMACountyBasin(string amacode = null)
        {
            try
            {


                var infoList = amacode == null ? AwAmaCountyBasinSubbasin.GetAll() :
                    AwAmaCountyBasinSubbasin.GetList(a => a.AmaCode == amacode.ToUpper());

                return Ok(infoList.GroupBy(g => new
                {
                    g.AMA,
                    g.AmaCode,
                    g.AmaInaType
                    ,
                    DefaultBasinCode = g.AmaCode.Replace("X", "0") != "0" ? g.BasinCode : null //When AMA/INA already has basin/subbasin assigned
                    ,
                    DefaultBasinName = g.AmaCode.Replace("X", "0") != "0" ? g.BasinName : null
                })
                  .Select(a => new
                  {
                      a.Key.AMA,
                      a.Key.AmaCode,
                      a.Key.AmaInaType,
                      a.Key.DefaultBasinCode,
                      a.Key.DefaultBasinName,
                      AMAInfo = a.GroupBy(g => new { g.County, g.CountyCode })
                  .Select(c => new
                  {
                      c.Key.County,
                      c.Key.CountyCode,
                      Basin = c.GroupBy(g => new { g.BasinCode, g.BasinName, HasSubbasin = g.SubbasinCode != g.BasinCode && true }).Distinct().OrderBy(o => o.Key.BasinName)
                    .Select(i => new
                    {
                        i.Key.BasinCode,
                        i.Key.BasinName,
                        i.Key.HasSubbasin
                    ,
                        Subbasin = i.Select(s => new { s.BasinCode, s.BasinName, s.SubbasinCode, s.SubbasinName }).Distinct().OrderBy(o => o.SubbasinName)
                    }).Distinct()
                  }).OrderBy(o => o.County)
                  }).OrderBy(o => o.AMA != "OUTSIDE OF AMA OR INA" ? "_" + o.AMA : o.AMA).ToList());
            }
            
          catch (Exception exception)
            {

                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpGet, Route("aws/getgeneralInfoByPcc/{id}")]
        public IHttpActionResult GetGeneralInfoByPcc(string id)
        {
            //this will format any pcc as long as the pattern is two numbers, six numbers, four numbers in it
            //so it can be all numbers or have characters as long as the character are in the correct locations
            //I'm sorry to change it
            try
            {
                //throw new InvalidOperationException("Logfile cannot be read-only");
                Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
                var pcc = regex.Replace(id, "$1-$2.$3");
                // var pcc = regex.Replace("~", ".");
                var found = VAwsGeneralInfo.GetGeneralInformation(pcc);
                return Ok(found);
            }
            catch (Exception exception)
            {

                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }
        [HttpGet, Route("aws/getCommentTypes")]
        public IHttpActionResult GetCommentTypes()
        {
            //Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            //var pcc = regex.Replace(id, "$1-$2.$3");
            // var pcc = regex.Replace("~", ".");
            var found = CdAwCommentType.GetAll();
            return Ok(found);
        }
        [HttpGet, Route("aws/getCommentsByWrfId/{id}")]
        public IHttpActionResult GetCommentsByWrfId(int id)
        {
            //Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            //var pcc = regex.Replace(id, "$1-$2.$3");
            // var pcc = regex.Replace("~", ".");
            var found = CommentsViewModel.AwsGetComments(id);
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
            return Ok(AwFile.Get(p => p.WaterRightFacilityId == id));
        }
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPost, Route("aws/SaveNewComment/{id}")]
        public async Task<IHttpActionResult> SaveNewComment([FromBody] CommentsViewModel comment, int id)
        {
            var user = User.Identity.Name.Replace("AZWATER0\\", "");
            comment.FileManager = user;
            var saved = CommentsViewModel.AddAWSComment(comment, user);
            return Ok(saved);
        }
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPut, Route("aws/UpdateAwFile/{id}")]
        public async Task<IHttpActionResult> UpdateAwFile([FromBody] AwFile af, int id)
        {
            af.UpdateBy = User.Identity.Name.Replace("AZWATER0\\", "");
            AwFile aw_file;

            using (var context = new OracleContext())
            {
                try
                {
           
                    aw_file = context.AW_FILE.Where(x => x.WaterRightFacilityId == id).FirstOrDefault();
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
                    else
                    {
                        throw new InvalidOperationException("No Aw File found for WaterRightFacilityId");
                    }
                    return Ok(aw_file);
                }
                catch (Exception exception)
                {
                    return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
                }
            }
        }
       
        [HttpGet, Route("aws/getAwCity")]
        public IHttpActionResult GetAwCity()
        {
            try
            {
                var awCityList = CdAwCity.GetAll();
                return Ok(awCityList);
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [HttpGet, Route("aws/getConsistManage/{id}")]
        public IHttpActionResult GetConsistManage(int id)
        {
            try
            {
                return Ok(new AwsConsistencyViewModel(id));
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [HttpPut, Route("aws/ConsistManage/{id}")]
        public IHttpActionResult UpdateConsistManage(int id, AwsConsistencyViewModel consistManage)
        {
            try
            {

                //throw new InvalidOperationException("Logfile cannot be read-only");
                return Ok(new AwsConsistencyViewModel(id, consistManage));
            }
            catch (Exception exception)
            {
                return BadRequest(QueryResult.BundleExceptions(exception));
            }
        }

        [HttpGet, Route("aws/getWellServingById/{id}")]
        public IHttpActionResult GetWellServingById(int id)
        {
            List<VAwsWellServing> wellServingList;
            try
            {
                wellServingList = VAwsWellServing.GetList(x => x.WaterRightFacilityId == id);
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(wellServingList);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/addWellServing/{wrf}/{well}")]
        public IHttpActionResult AddWellServing(int wrf, string well)
        {
            var record = new AwWellServing
            {
                CreateDt = DateTime.Now,
                WaterRightFacilityId = wrf,
                WellRegistryId = well,
                CreateBy = User.Identity.Name.Replace(@"AZWATER0\", "")
            };

            try
            {
                AwWellServing.Add(record);
                return Ok("Created");
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [HttpGet, Route("aws/getLongTermStorageCreditsById/{id}")]
        public IHttpActionResult GetLongTermStorageCreditsById(int id)
        {
            List<VAwsLongTermStorageCredits> longTermStorageCreditsList;
            try
            {
                longTermStorageCreditsList = VAwsLongTermStorageCredits.GetList(x => x.WaterRightFacilityId == id);
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(longTermStorageCreditsList);
        }

        [HttpGet, Route("aws/getLegalAvailabilityById/{wrf}")]
        public IHttpActionResult GetLegalAvailabilityById(int wrf)
        {
            List<AwLegalAvailability> LegalAvailabilityList;
            try
            {
                LegalAvailabilityList = AwLegalAvailability.GetList(x => x.WaterRightFacilityId == wrf);
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(LegalAvailabilityList);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/updateLegalAvailability")]
        public async Task<IHttpActionResult> UpdateLegalAvailability([FromBody] List<AwLegalAvailability> la)
        {
            try
            {
                var msg = "";

                if (!(la != null && la.Count() > 0))
                    return Ok();

                AwLegalAvailability existing = new AwLegalAvailability();
                var updatedList = new List<AwLegalAvailability>();
              
                int actionCount = 0;
                int wrf = la.FirstOrDefault().WaterRightFacilityId;

                var d = new Dictionary<string, string>();

                using (var context = new OracleContext())
                {
                    foreach (var legalAvail in la)
                    {
                        int addCount = 0;
                        //check for a valid PCC
                        //if (legalAvail.Section == "SW")
                        if (new[] { "SW", "G", "ST", "R", "L", "E", "W" }.Contains(legalAvail.Section))
                            if (legalAvail.ProviderReceiverId == null && legalAvail.PCC != null) 
                            {                                 
                                //throw new InvalidOperationException("Legal Availability("+ legalAvail.Section + ") Invalid PCC number");
                                return BadRequest("Error: PCC not found");
                            }
                    
                            existing = context.AW_LEGAL_AVAILABILITY.Where(x => x.Id == legalAvail.Id).FirstOrDefault();

                            //make sure something has been added
                            if (!(existing==null && legalAvail.EffluentType == null && legalAvail.ContractName == null && legalAvail.Amount == null && legalAvail.GroundwaterUseType == null &&
                               legalAvail.WaterTypeCode == null && legalAvail.AreaOfImpact == null && legalAvail.ContractNumber == null && legalAvail.SurfaceWaterType == null &&
                               legalAvail.StorageFacilityName == null && legalAvail.PledgedAmount == null && legalAvail.ProviderReceiverId==null))
                            {
                                addCount++;
                            }

                            if (existing == null )//add new record
                            { 
                          
                            existing = new AwLegalAvailability
                            {
                                
                                CreateBy = User.Identity.Name.Replace("AZWATER0\\", ""),
                                WaterRightFacilityId = legalAvail.WaterRightFacilityId,
                                EffluentType = legalAvail.EffluentType,
                                ContractName = legalAvail.ContractName,
                                Amount = legalAvail.Amount,
                                GroundwaterUseType = legalAvail.GroundwaterUseType,
                                ProviderReceiverId = legalAvail.ProviderReceiverId,
                                WaterTypeCode = legalAvail.WaterTypeCode,
                                AreaOfImpact = legalAvail.AreaOfImpact,
                                ContractNumber = legalAvail.ContractNumber,
                                SurfaceWaterType = legalAvail.SurfaceWaterType,
                                Section = legalAvail.Section,
                                StorageFacilityName = legalAvail.StorageFacilityName,
                                PledgedAmount = legalAvail.PledgedAmount
                            };

                            if(addCount > 0)
                            {
                                actionCount++;
                                context.AW_LEGAL_AVAILABILITY.Add(existing);
                               
                            }
                        }
                        else//update existing record
                        {
                            actionCount++;
                            var props = existing.GetType().GetProperties().ToList();
                          
                            foreach (var prop in props)
                            {
                                var prevValue = prop.GetValue(existing);
                                var value = prop.GetValue(legalAvail);

                                //make sure something has changed
                                if (!Object.Equals(prevValue, value) && (prop.Name != "Id") && (prop.Name != "WaterRightFacilityId")                                    
                                && (prop.Name != "UpdateBy") && (prop.Name != "UpdateDt")// && (prop.Name != "PCC") 
                                && (prop.Name != "CreateBy") && (prop.Name != "CreateDt") && (prop.Name != "Section"))
                                {
                                    
                                    prop.SetValue(existing, value);
                                }
                            }
                            existing.UpdateBy = User.Identity.Name.Replace("AZWATER0\\", "");
                            updatedList.Add(existing);     
                        }
                    }

                    if (actionCount > 0) //make sure SOMETHING changed
                    {
                        await context.SaveChangesAsync();
                        var newLegalRecords = AwLegalAvailability.GetList(l => l.WaterRightFacilityId == wrf);
                        return Ok(newLegalRecords);
                    }
                    else
                    {
                        return BadRequest("Nothing was changed because no values were provided.");
                    }
                }
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        //[HttpPost, Route("aws/newLegalAvailability/{wrf:int}")]
        //public IHttpActionResult AddLegalAvailability([FromBody] List<AwLegalAvailability> la, int wrf)
        //{
        //    try
        //    {
        //        using (var context = new OracleContext())
        //        {
        //            var newList = new List<AwLegalAvailability>();
        //            foreach(var legalAvail in la)
        //            {
        //                var LegalAvailability = new AwLegalAvailability
        //                {
        //                    CreateBy = User.Identity.Name.Replace("AZWATER0\\", ""),
        //                    WaterRightFacilityId = wrf,
        //                    EffluentType = legalAvail.EffluentType,
        //                    ContractName = legalAvail.ContractName,
        //                    Amount = legalAvail.Amount,
        //                    GroundwaterUseType = legalAvail.GroundwaterUseType,
        //                    ProviderReceiverId = legalAvail.ProviderReceiverId,
        //                    WaterTypeCode = legalAvail.WaterTypeCode,
        //                    AreaOfImpact = legalAvail.AreaOfImpact,
        //                    ContractNumber = legalAvail.ContractNumber,
        //                    SurfaceWaterType = legalAvail.SurfaceWaterType
        //                };
        //                newList.Add(LegalAvailability);
        //            }

        //            context.AW_LEGAL_AVAILABILITY.AddRange(newList);
        //            context.SaveChanges();
        //            return Ok(newList);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        return InternalServerError(exception);
        //    }
        //}

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpGet, Route("aws/amaDetail/{wrfid:int}")]
        public IHttpActionResult GetAmaDetail(int wrfid)
        {
            try
            {
                using (var context = new OracleContext())
                {
                    var found = context.V_AWS_AMA.Where(x => x.WaterRightFacilityId == wrfid).FirstOrDefault();
                    return Ok(found);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }



         
        [HttpGet, Route("aws/conveyance/{id}")]
        public IHttpActionResult GetConveyance(int? id) //New conveyance
        {
            try
            {
                if(id==null)
                {
                    return BadRequest("Id was not submitted for this request");
                }
                return Ok(new AwsConveyViewModel(id));
            }


             catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/saveconveyance/{id}")]
        public IHttpActionResult SaveConveyance(int id, [FromBody] AwsConveyViewModel conveyance) //New conveyance
        {
            try
            {                 
                var user = new GetBestUsername(User.Identity.Name).UserName;  //Set to Oracle ID if possible
                var convey = new AwsConveyViewModel(id, conveyance, user);

                //use "successfulActions" if you want to return mixed results (successful and unsuccessful actions)
                //you will need to accommodate in the AwsConveyViewModel to savechanges even when some are unsuccessful
                var successfulActions = convey.StatusReport != null ? convey.StatusReport.Where(s => !(s.Value.ToString().StartsWith("Error"))) : null;
                var errors = convey.StatusReport != null ? convey.StatusReport.Where(s => s.Value.ToString().StartsWith("Error")) : null;

                if(errors != null && errors.Count() > 0)
                {
                    var msg = String.Join(", ", errors.Select(v => v.Value.ToString()));
                    return BadRequest(msg);
                }
                
                return Ok(convey);
            }

            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
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

            if (conveyance == null)
            {
                return Ok(new { message = "A new conveyance was not created" });
            }
            else if (conveyance.Result.ProcessStatus != null)
            {
                return Ok(new { message = conveyance.Result.ProcessStatus });
            }
            return Ok(conveyance);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS & Recharge")]
        [HttpPost, Route("aws/newapp")]
        public async Task<IHttpActionResult> AddNewApplication([FromBody] SP_AW_INS paramValues) //New file
        {
            var application = await Task.FromResult(SP_AW_INS.CreateNewFile(paramValues, "newApplication"));

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
            VAwsGeneralInfo genInfo;
            using (var context = new OracleContext())
            {
                genInfo = context.V_AWS_GENERAL_INFO.Where(x => x.ProgramCertificateConveyance == paramValues.Overview.ProgramCertificateConveyance).FirstOrDefault();
                var props = genInfo.GetType().GetProperties().ToList();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(paramValues.Overview);
                    if ((value != null) && (prop.Name != "PCC"))
                    {
                        prop.SetValue(genInfo, value);
                        if (prop.Name == "SubbasinCode")
                        {
                            var hydro = context.V_AWS_HYDRO.Where(x => x.PCC == paramValues.Overview.ProgramCertificateConveyance).FirstOrDefault().SubbasinCode = value.ToString();
                        }
                    }
                }
                await context.SaveChangesAsync();
            }

            return Ok(genInfo);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/get42Parent/{pcc42}")]
        public async Task<IHttpActionResult> Get42Parent(string pcc42)
        {
            try
            {
                using (var context = new OracleContext())
                {
                    var conveyanceInfo = new ConveyanceInfo(context);
                    var parent = conveyanceInfo.Get42Parent(new PCC(pcc42));
                    return Ok(parent);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/getconveyanceCount/{pcc28}")]
        public async Task<IHttpActionResult> GetConveyanceCount(string pcc28)
        {
            try
            {
                using (var context = new OracleContext())
                {
                    var conveyanceInfo = new ConveyanceInfo(context);
                    var checkedPCC = new PCC(pcc28);

                    if (checkedPCC.Program == "42")
                    {
                        checkedPCC = conveyanceInfo.Get42Parent(checkedPCC);
                    }
                    var foundPCCs = conveyanceInfo.Get42ConveyanceCount(checkedPCC);
                    return Ok(foundPCCs);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/{wrf}/{activity}")]
        public IHttpActionResult GetActivity(int wrf, string activity)
        {
            var activities = new AwAppActivityTrk();
            try
            {
                activities = AwAppActivityTrk.GetList(x => x.WaterRightFacilityId == wrf && x.ActivityCode == activity).FirstOrDefault();
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(activities);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/issued/{wrf}")]
        public IHttpActionResult GetActivity(int wrf)
        {
            List<AwAppActivityTrk> activities = new List<AwAppActivityTrk>();
            try
            {
                activities = AwAppActivityTrk.GetList(x => x.WaterRightFacilityId == wrf && new List<string> { "ISSD", "IADQ", "IIAD" }.Contains(x.ActivityCode)).OrderByDescending(x => x.CreateDt).ToList();
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(activities.FirstOrDefault());
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPost, Route("aws/activity/{wrf}/{activityCode}")]
        public IHttpActionResult UpdateActivity(int wrf, string activityCode)
        {
            var record = new AwAppActivityTrk
            {
                ActivityTrackDate = DateTime.Now,
                WaterRightFacilityId = wrf,
                ActivityCode = "ISSD",
                CreateBy = User.Identity.Name.Replace(@"AZWATER0\", "")
            };

            try
            {
                AwAppActivityTrk.Add(record);
                return Ok("Created");
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/{pcc}")]
        public IHttpActionResult GetAllActivity(int pcc)
        {
            List<AwAppActivityTrk> activities = null;
            var context = new OracleContext();
            object activitiesDescribed;
            try
            {
                activities = AwAppActivityTrk.GetList(x => x.WaterRightFacilityId == pcc);
                activitiesDescribed = activities.Join(context.CD_AW_APP_ACTIVITY,
                    act => act.ActivityCode,
                    code => code.Code,
                    (act, code) => new
                    {
                        Activity = code.Description,
                        Date = act.CreateDt
                    });
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
            return Ok(activitiesDescribed);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/activity/codes")]
        public IHttpActionResult GetActivityCodes()
        {
            List<CdAwAppActivity> codes;
            try
            {
                codes = CdAwAppActivity.GetList(x => x.AlsoFileStatus == "Y");
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
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
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
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
                var customerList = VAwsCustomerLongName.GetList(co => co.CompanyLongName.ToLower().Contains(company.ToLower()))
                    .Select(result => new
                    {
                        companyRank = company != null && result.CompanyLongName != null ? result.CompanyLongName.ToLower() == company.ToLower()
                        ? new string(' ', 3) + result.CompanyLongName.ToLower()
                        : String.Format("{0:D20}{1}", result.CompanyLongName.ToLower().IndexOf(company.ToLower()), result.CompanyLongName.ToLower()) : null,

                        result
                    })
                     .OrderBy(o => o.companyRank)
                     .Select(s => s.result).Take(20);

                var custWrfViewModelList = customerList.Select(x => new Aws_customer_wrf_ViewModel(x));
                return Ok(custWrfViewModelList);
            }
            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
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
        public IHttpActionResult GetCustomerByAny([FromBody] VAwsCustomerLongName customer)
        {
            try
            {
                string firstname = customer.FirstName;
                string lastname = customer.LastName;
                string company = customer.CompanyLongName;
                string address1 = customer.Address1;

                if (firstname == null && lastname == null && company == null && address1 == null)
                {
                    return Ok(new { Message = "At least one search term must be entered (First Name, Last Name, Company Name or Address1/Care of" });
                }

                var searchString = String.Format("FirstName={0} LastName={1} Company={2} Address1={3}", firstname, lastname, company, address1);
                var customerList = VAwsCustomerLongName.GetList(
                  c =>
                      ((company != null && c.CompanyLongName.ToLower().Contains(company.ToLower())) || company == null) &&
                      ((firstname != null && c.FirstName.ToLower().Contains(firstname.ToLower())) || firstname == null) &&
                      ((lastname != null && c.LastName.ToLower().Contains(lastname.ToLower())) || lastname == null) &&
                      ((address1 != null && c.Address1.ToLower().Contains(address1.ToLower())) || address1 == null)
                      ).Select(result => new
                      {
                          companyRank = company != null && result.CompanyLongName != null ?
                          result.CompanyLongName.ToLower() == company.ToLower() ? new string(' ', 3) + result.CompanyLongName.ToLower()
                          : String.Format("{0:D20}{1}", result.CompanyLongName.ToLower().IndexOf(company.ToLower()), result.CompanyLongName.ToLower()) : null,

                          firstNameRank = firstname != null && result.FirstName != null ?
                          result.FirstName.ToLower() == firstname.ToLower() ? new string(' ', 3) + result.FirstName.ToLower()
                          : String.Format("{0:D20}{1}", result.FirstName.ToLower().IndexOf(firstname.ToLower()), result.FirstName.ToLower()) : null,

                          lastNameRank = lastname != null && result.LastName != null ?
                          result.LastName.ToLower() == lastname.ToLower() ? new string(' ', 3) + result.LastName.ToLower()
                          : String.Format("{0:D20}{1}", result.LastName.ToLower().IndexOf(lastname.ToLower()), result.LastName.ToLower()) : null,

                          addressRank = address1 != null && result.Address1 != null ?
                          result.Address1.ToLower() == address1.ToLower() ? new string(' ', 3) + result.Address1.ToLower()
                          : String.Format("{0:D20}{1}", result.Address1.ToLower().IndexOf(address1.ToLower()), result.Address1.ToLower()) : null,

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
                var custIdList = WaterRightFacilityCustomer.GetList(x => x.WaterRightFacilityId == wrf).Select(x => x.CustomerId).ToList().Distinct();
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
                    var customerList = new List<VAwsCustomerLongName>();
                    if (firstName != null)
                    {
                        var query = context.V_AWS_CUSTOMER_LONG_NAME.Where(x => x.FirstName.ToLower().Contains(firstName.ToLower()));
                        if (lastName != null)
                        {
                            customerList = query.Where(x => x.LastName.ToLower().Contains(lastName.ToLower())).ToList();
                        }
                        else
                        {
                            customerList = query.ToList();
                        }
                    }
                    else if (lastName != null)
                    {
                        customerList = context.V_AWS_CUSTOMER_LONG_NAME.Where(x => x.LastName.ToLower().Contains(lastName.ToLower())).ToList();
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
                /* string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                 //get Oracle USER_ID if available
                 var foundUser = AwUsers.Get(u => u.Email.ToLower().Replace("@azwater.gov", "") == userName);
                 string oracleUserID =  foundUser.UserId ?? null;*/
                string userName = new GetBestUsername(User.Identity.Name).UserName;  //Set to Oracle ID if possible

                var createDt = DateTime.Now;
                string appendCompanyName = "";

                if (customer.Customer == null || customer.Waterrights == null)
                {
                    return BadRequest("Mandatory information for customer was not provided.");
                }

                var existingCustomers = WaterRightFacilityCustomer.GetList(l => l.WaterRightFacilityId == wrf);

                if (existingCustomers == null)
                {
                    if (WaterRightFacility.Get(l => l.Id == wrf) == null)
                    {
                        return BadRequest("The water right facility ID submitted is incorrect or doesn't exist.");
                    }
                }
                using (var context = new OracleContext())
                {
                    int? customerID = (int?)customer.Customer.CustomerId ?? -1;

                    customer.Waterrights.Select(w =>
                    {
                        w.WaterRightFacilityId = wrf;
                        return w;
                    }).ToList();

                    //Customer is new, ensure required data is provided
                    if (!customer.IsValid() && customerID < 1) //if the customer doesn't exist, ensure that the appropriate data is being submitted
                    {
                        return BadRequest(String.Format("THE CUSTOMER WAS NOT CREATED BECAUSE THE FOLLOWING INFORMAION IS MISSING: {0}", customer.IsValidMsg()));
                    }
                    //Customer exists, verify the CCT_CODE is provided
                    else if (customerID > 0 && customer.Waterrights.Count() != customer.Waterrights.Count(w => w.CustomerTypeCode != null))
                    {
                        return BadRequest("COULD NOT CREATE AN ASSOCIATION BECAUSE THE CCT CODE MUST BE PROVIDED");
                    }

                    //Customer is new, create customer
                    if (customerID < 1)
                    {
                        if (customer.Customer.CompanyLongName != null && customer.Customer.CompanyLongName.Length > 60)
                        {
                            appendCompanyName = customer.Customer.CompanyLongName.Substring(60);
                            customer.Customer.CompanyLongName = customer.Customer.CompanyLongName.Substring(0, 60);
                        }

                        customer.Customer.BadAddressFlag = customer.Customer.BadAddressFlag == "false" ? "N" : "Y";
                        var rgrCustomer = new Customer(customer.Customer, userName)
                        {
                            CreateBy = userName,
                            CreateDt = createDt
                        };
                        context.CUSTOMER.Add(rgrCustomer);
                        context.SaveChanges();//need to save and get rgr.customer ID back from DB sequence to use in wrf_cust

                        customerID = rgrCustomer.Id;

                        if (customerID < 1)
                        {
                            return BadRequest("ERROR CREATING CUSTOMER");
                        }

                        if (appendCompanyName != "" && customerID > 0) //insert long name value into 
                        {
                            var customerLongName = new AwCustLongName()
                            {
                                CustId = customerID ?? -1,
                                CompanyLongName = appendCompanyName,
                                CreateBy = userName,
                                CreateDt = createDt
                            };
                            context.AW_CUST_LONG_NAME.Add(customerLongName);
                            context.SaveChanges();
                        }
                    }  //customer exists              
                    else
                    {
                        var findCust = Customer.Get(c => c.Id == customer.Customer.CustomerId, context);
                        if (findCust == null)
                        {
                            BadRequest("COULD NOT FIND A MATCHING CUSTOMER FOR THE ID ENTERED");
                        }
                        else
                        {
                            customer.Customer = VAwsCustomerLongName.Get(c => c.CustomerId == customer.Customer.CustomerId, context);
                        }
                    }

                    var waterRights = customer.Waterrights.Select(w =>
                    {
                        w.CustomerId = customerID ?? -1;
                        w.IsActive = w.IsActive ?? "Y";
                        w.LineNum = ((from f in existingCustomers where f.CustomerTypeCode == w.CustomerTypeCode && f.WaterRightFacilityId == w.WaterRightFacilityId select f).Max(l => (int?)l.LineNum) ?? 0) + 1; // Set line num 
                        w.CreateBy = userName;
                        w.CreateDt = createDt;
                        return w;
                    }).ToList();

                    if (waterRights == null)
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
            }
        }

        [HttpPut, Route("aws/customer/{custId}"), Authorize]
        public IHttpActionResult UpdateCustomer(int custId, Aws_customer_wrf_ViewModel customer)
        {
            try
            {
                string userName = new GetBestUsername(User.Identity.Name).UserName;
                var currentDt = DateTime.Now;
                var requestMethod = ActionContext.Request.Method.ToString().ToUpper(); //POST, DELETE ETC.

                using (var context = new OracleContext())
                {
                    var foundUser = VAwsCustomerLongName.Get(x => x.CustomerId == custId, context);
                    var allWaterRights = WaterRightFacilityCustomer.GetList(x => x.CustomerId == custId, context);
                    var foundWaterRights = allWaterRights.FirstOrDefault();

                    //if (requestMethod == "DELETE")
                    //{
                    var deleteMsg = "";

                    //Ensure IS_ACTIVE is "N". Set the CUST_ID value
                    var wrfCust = customer.Waterrights.Where(w => w.IsActive == "N" && w.CustomerId == custId);

                    if (wrfCust != null && wrfCust.Count() > 0)
                    {
                        var deleteWrfCust = (from w in allWaterRights
                                             join c in wrfCust on new { w.WaterRightFacilityId, w.CustomerId, w.CustomerTypeCode, w.LineNum }
                                             equals new { c.WaterRightFacilityId, c.CustomerId, c.CustomerTypeCode, c.LineNum }
                                             select w).ToList();

                        if (deleteWrfCust == null || deleteWrfCust.Count() == 0)
                        {
                            return BadRequest("CUSTOMER RECORDS COULD NOT BE FOUND");
                        }

                        context.WRF_CUST.RemoveRange(deleteWrfCust); //Delete a customer association from the wrf_cust table
                        context.SaveChanges();
                        deleteMsg = string.Format("DELETED {0} WRF_CUST RECORD(S)", deleteWrfCust.Count());

                        var custCount = WaterRightFacilityCustomer.GetList(x => x.CustomerId == custId, context).Count();

                        //When all the records have been deleted in WRF_CUST, remove corresponding customer records
                        if (custCount == 0)
                        {
                            var deleteCust = Customer.Get(c => c.Id == custId, context);
                            context.CUSTOMER.Remove(deleteCust);
                            context.SaveChanges();
                            deleteMsg = "WRF_CUST AND CUSTOMER RECORDS DELETED";

                            var deleteCustLongName = AwCustLongName.Get(c => c.CustId == custId, context);
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

                    var rightsCount = allWaterRights != null ? allWaterRights.Select(w => w.WaterRightFacilityId).Distinct().Count() : 0;

                    if (rightsCount > 1)
                    {
                        return BadRequest(string.Format("THIS CUSTOMER IS ASSOCIATED WITH {0} RIGHTS AND CANNOT BE UPDATED", rightsCount));
                    }

                    //get customer properties
                    var custPropList = customer.Customer.GetType().GetProperties().ToList();

                    foreach (var prop in custPropList)
                    {
                        var tempVal = prop.GetValue(customer.Customer);
                        var hasTrim = prop.PropertyType.GetMethods().Where(i => i.Name == "Trim"); //use the trim method if one exists                          

                        if (hasTrim != null && tempVal != null)
                        {
                            tempVal.ToString().Trim(new Char[] { ' ', '\n', '\r' });
                        }

                        var originalValue = prop.GetValue(foundUser);

                        if (!Object.Equals(tempVal, (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : prop.GetValue(foundUser))))
                        {
                            if (prop.Name != "CustomerId" && prop.Name != "CustomerTypeCode")//can't update the DB Key
                                prop.SetValue(foundUser, tempVal);
                        }
                    }

                    var rightProps = typeof(WaterRightFacilityCustomer).GetProperties().ToList();
                    foreach (var waterRight in customer.Waterrights)
                    {
                        var wrf_cust = WaterRightFacilityCustomer.Get(x => x.CustomerId == waterRight.CustomerId && x.WaterRightFacilityId == waterRight.WaterRightFacilityId && x.CustomerTypeCode == waterRight.CustomerTypeCode, context);
                        bool changesOccurred = false;
                        foreach (var prop in rightProps)
                        {
                            var tempValue = prop.GetValue(waterRight);
                            var incomingValue = prop.GetValue(wrf_cust);
                            var useValue = !(tempValue == (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : incomingValue));
                            var newValue = Object.Equals(tempValue, (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : incomingValue));

                            //if (tempValue != (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : incomingValue))
                            if (!Object.Equals(tempValue, (prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : incomingValue)))
                            {
                                if (prop.Name != "WaterRightFacilityId" && prop.Name != "CustomerId" && prop.Name != "CustomerTypeCode" && prop.Name != "LineNum")
                                {
                                    //r st = String.Format("Old value {0} New Value {1} equal ? {2}", tempValue, incomingValue, newValue

                                    prop.SetValue(wrf_cust, tempValue);
                                    changesOccurred = true;
                                }
                            }
                        }
                        if (changesOccurred)
                        {
                            waterRight.UpdateBy = userName;
                            waterRight.UpdateDt = currentDt;
                        }
                    }

                    context.SaveChanges();
                    var rgrCustomer = context.CUSTOMER.Where(x => x.Id == foundUser.CustomerId).FirstOrDefault();
                    rgrCustomer.UpdateBy = userName;
                    rgrCustomer.UpdateDt = currentDt;
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
        [Authorize]
        public IHttpActionResult CreateWrfcust([FromBody] List<WaterRightFacilityCustomer> wrfcustList)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                using (var context = new OracleContext())
                {
                    foreach (var wrfcust in wrfcustList)
                    {
                        var customerExists = context.CUSTOMER.Where(x => x.Id == wrfcust.CustomerId).FirstOrDefault() != null;
                        var wrfExists = context.WRF_CUST.Where(x => x.WaterRightFacilityId == wrfcust.WaterRightFacilityId).FirstOrDefault() != null;
                        var count = WaterRightFacilityCustomer.GetList(x => x.WaterRightFacilityId == wrfcust.WaterRightFacilityId && x.CustomerId == wrfcust.CustomerId && x.CustomerTypeCode == wrfcust.CustomerTypeCode).Count();
                        if (!customerExists && !wrfExists)
                        {
                            return BadRequest("customer or wrf does not exist");
                        }
                        if (count > -1)
                        {
                            wrfcust.CreateBy = userName;
                            wrfcust.CreateDt = DateTime.Now;
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
            var codes = CdCustType.GetList(x => custCodeList.Contains(x.Code));
            return Ok(codes);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPost, Route("aws/updatehydro/{wrf}")]
        public IHttpActionResult UpdateHydro(int wrf, [FromBody] AwsHydrologyViewModel wHydro)
        {
            
            try
            {
                var hydroVm = new AwsHydrologyViewModel();
                if (wHydro == null)
                {
                    return BadRequest("Hydrology updates error");
                }
                var userName = new GetBestUsername(User.Identity.Name).UserName;
                var hydro = wHydro.Hydrology ?? null;
                var wellServing = wHydro.WellServing ?? null;


                if (hydro != null)
                {
                    wrf = hydro.WaterRightFacilityId;
                    hydro.UserName = userName;
                    hydroVm.Hydrology = hydro;
                    VAwsHydro.Update(hydro);
                }

                if (wellServing != null && wellServing.Count() > 0)
                {
                    
                    var deletes = wellServing.Where(d => d.Id == -1);
                    var adds = wellServing.Where(d => d.Id != -1).ToList();
                    var wellList = new List<AwWellServing>();                   

                    if (deletes != null)
                    {

                        foreach (var d in deletes)
                        {
                            var item = AwWellServing.Get(i => i.WaterRightFacilityId == d.WaterRightFacilityId && i.WellRegistryId == d.WellRegistryId);
                            AwWellServing.Delete(item);
                        }

                    }

                    if (adds != null)
                    {
                        foreach (var a in adds)
                        {
                            AwWellServing.Add(new AwWellServing()
                            {
                                WaterRightFacilityId = a.WaterRightFacilityId,
                                CreateBy = userName,
                                CreateDt = DateTime.Now,
                                WellRegistryId = a.WellRegistryId

                            }); ;
                        }
                    }

                }
              
                return Ok(new AwsHydrologyViewModel(wrf));
            }

            catch(Exception exception)

            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
                //log error
                //return InternalServerError(exception);
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/hydrobypcc/{pcc}")]
        public IHttpActionResult GetHydroByPcc(string pcc)
        {
            Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
            pcc = regex.Replace(pcc, "$1-$2.$3");
            return Ok(VAwsHydro.Get(h => h.PCC == pcc));
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/hydrobyid/{id}")]
        public IHttpActionResult GetHydroByWrfId(int id)
        {
            var hydro = new AwsHydrologyViewModel(id);
            return Ok(hydro);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpGet, Route("aws/physavail/{id}")]
        public IHttpActionResult GetPhysicalAvailability(int id)
        {
            var phys = new AwsPhysicalAvailabilityViewModel(id);
            return Ok(phys);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPost, Route("aws/savephysavail/{id}")]
        public IHttpActionResult SavePhysicalAvailability(int? id, [FromBody] AwsPhysicalAvailabilityViewModel physical)
        {
            try
            {
                var userName = new GetBestUsername(User.Identity.Name).UserName;

                if (id == null)
                {
                    return BadRequest("Invalid WaterRightFacilityId");
                }

                var phys = new AwsPhysicalAvailabilityViewModel((id ?? -1), physical, userName);

                return Ok(phys);
            }

            catch (Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", BundleExceptions(exception)));
            }
        }

        [HttpGet, Route("aws/getwrfid/{pcc}/{type?}")]
        public IHttpActionResult GetWrfId(string pcc, string type=null)
        {
           
            int? waterRightFacilityId=null;
            if (pcc == null)
            {
                return BadRequest("Please provide a PCC");
            }

            try
            {
                Regex regex = type != null && type == "SW" ? new Regex(@"(\d{2})\D?(\d{5})\D?(\d{1,4})") : new Regex(@"(\d{2})\D?(\d{6})\D?(\d{4})");                

                pcc = regex.Replace(pcc, "$1-$2.$3");

                if (type != null &&  type == "SW")
                {

                    //regex = new Regex(@"(\d{2})\D?(\d{6})\D?(\d{1,4})");

                    // pcc= regex.Replace(pcc, "$1-$2.$3");


                    if (pcc.Length > 9)
                    {

                        waterRightFacilityId = QueryResult.RgrRptSurface(pcc);

                        if (waterRightFacilityId == null)
                            return Ok(-1);
                        else
                            return Ok(waterRightFacilityId);
                    }
                }
                else {
                    if (pcc.Length != 14)
                    {
                        return BadRequest("Invalid PCC format was submitted.");
                    }

                    waterRightFacilityId = QueryResult.RgrRptGet(pcc);
                }                

                if (waterRightFacilityId == null)
                    return BadRequest("PCC does not exist.");

                return Ok(waterRightFacilityId);
            }
            catch (Exception exception)
            {
                return BadRequest(QueryResult.BundleExceptions(exception));
            }
        }

        [HttpPost, Route("aws/legalAvail/remove")]
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        public IHttpActionResult DeleteLegalAvailability([FromBody] int[] deletionIds)
        {
            try
            {
                List<int> deleteSuccess = new List<int>();
                using (var context = new OracleContext())
                {
                    var found = AwLegalAvailability.GetList(x => deletionIds.Contains(x.Id), context);
                    var wrf = found.FirstOrDefault().WaterRightFacilityId;

                    foreach (var f in found)
                    {
                        
                        if (f != null)
                        {
                            context.AW_LEGAL_AVAILABILITY.Remove(f);
                            deleteSuccess.Add(f.Id);
                        }
                    }

                    context.SaveChanges();

                    var updates = AwLegalAvailability.GetList(l => l.WaterRightFacilityId == wrf);

                    if (updates != null)
                    {
                        return Ok(updates);
                    }

                    return BadRequest(String.Format("Unable to delete values {0}", deletionIds));
                }
            }
            catch (Exception exception)
            {
                //log
                return InternalServerError(exception);
            }
        }

     
        /* [HttpGet, Route("aws/getConsistManage/{id}")]
         public IHttpActionResult GetConsistManage(int id)
         {
             try
             {
                 return Ok(VAwsActiveManagementArea.GetCAGRDInfo(id));
             }
             catch (Exception exception)
             {
                 //log exception
                 return InternalServerError();
             }
         }*/


        //[HttpGet, Route("aws/anyquery")]
        //public IHttpActionResult TestAnyQuery()
        //{
        //    //var sql = "select * from wtr_right_facility where rownum < 10";
        //    //var result = QueryResult.RunAnyQuery(sql);

        //    //return Ok(result);

        //}
    }
    // SAVING COMMENTS

}