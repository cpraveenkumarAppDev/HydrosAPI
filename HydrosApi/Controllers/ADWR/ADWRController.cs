﻿using System.Web.Http;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System;
using System.Configuration;
using HydrosApi.Services;
using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System.Linq;
using System.Text.RegularExpressions;
using HydrosApi.ViewModel.ADWR;
using System.Threading.Tasks;

namespace HydrosApi
{

    public class ADWRController : ApiController
    {

        [HttpGet]
        [Route("adwr/windows")]
        [System.Web.Http.Authorize]
        public IHttpActionResult WindowsAuthentication()
        {
            //comment test ron
            //To autmoatically login -> http://www.scip.be/index.php?Page=ArticlesNET38&Lang=EN
            var user = User.Identity.Name;
            var environment = ConfigurationManager.ConnectionStrings["ADWRContext"].ToString().Split(';')[0].Split('=')[1];
           

            if (user.Equals(""))
            {
                return Unauthorized();
            }
            else
            {
                var pages = new List<ActivePage>();
                var page1 = new ActivePage() { Page = "adjudications", Online = true };
                var page2 = new ActivePage() { Page = "aaws", Online = true };
                var page3 = new ActivePage() { Page = "logs", Online = false };
                pages.Add(page1);
                pages.Add(page2);
                pages.Add(page3);
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "AZWATER0"))
                {
                    List<string> roles = new List<string>();
                    UserPrincipal foundUsername = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
                    if (foundUsername != null)
                    {
                        PrincipalSearchResult<Principal> groups = foundUsername.GetAuthorizationGroups();

                        // iterate over all groups
                        foreach (Principal p in groups)
                        {
                            // make sure to add only group principals
                            if (p is GroupPrincipal)
                            {
                                roles.Add(p.Name);
                            }
                        }
                    }
                    GroupPrincipal appDevGroup = GroupPrincipal.FindByIdentity(ctx, "PG-APPDEV");
                    bool foundUserInAppDevGroup = foundUsername.IsMemberOf(appDevGroup);
                    var validUser = new { appEnv = environment, user, roles, activeApps = pages };
                    return Ok(validUser);
                }
            }
        }

        [HttpGet]
        [Route("adwr/windowsgroup/{groupId}")]
        [Authorize]
        public IHttpActionResult WindowsGroup(string groupId)
        {
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "AZWATER0"))
            {
                // find a user
                UserPrincipal foundUsername = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
                var unformattedGroupId = groupId.Replace("~AND~", " & "); //cannot pass & sign through url query
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, unformattedGroupId);
                GroupPrincipal appDevGroup = GroupPrincipal.FindByIdentity(ctx, "PG-APPDEV");
                bool foundUserInGroup = foundUsername.IsMemberOf(group);
                bool foundUserInAppDevGroup = foundUsername.IsMemberOf(appDevGroup);
                // if found....
                if (group != null && foundUserInGroup || foundUserInAppDevGroup)
                {

                    var groupList = new List<object>();
                    foreach (UserPrincipal p in group.GetMembers())
                    {

                        var user = new WindowsUser
                        {
                            UserName = p.DisplayName,
                            UserEmail = p.EmailAddress
                        };
                        groupList.Add(user);
                    }
                    return Ok(groupList);
                }
                else
                {
                    return BadRequest("Invalid User");
                }
            }
        }

        [HttpPost, Route("adwr/error")]
        [Authorize]
        public IHttpActionResult SubmitError()
        {
            try
            {
                string content = Request.Content.ReadAsStringAsync().Result;
                if (content != null)
                {
                    var sentOkay = EmailService.Message("appdev@azwater.gov", $"{Environment.MachineName}: {User.Identity.Name} - HydrosAPI", content);
                    return Ok($"Message sent: {sentOkay}");
                }
                else
                {
                    return BadRequest("no content in the body of the request");
                }
            }
            catch
            {
                return Ok("Failed to send noticiation");
            }

        }

        [HttpGet, Route("adwr/GetAppAvailability")]
        public IHttpActionResult GetAppAvailability()
        {
            try
            {
                var applicationList = HydrosManager.GetAll();
                return Ok(applicationList);
            }
            catch (Exception exception)
            {
                //log error
                return InternalServerError(exception);
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV")]
        [HttpPut, Route("adwr/SetAppAvailability/{id}")]
        public async Task<IHttpActionResult> SetAppAvailability([FromBody] HydrosManager man, int id)
        {
            var user = User.Identity.Name.Replace("AZWATER0\\", "");
            HydrosManager hydrosManager;
            using (var context = new OracleContext())
            {
                hydrosManager = context.HYDROS_MANAGER.Where(x => x.Id == id).FirstOrDefault();
                if (hydrosManager != null && hydrosManager.Status != man.Status)
                {
                    hydrosManager.Status = man.Status;
                    hydrosManager.UserName = user;
                    hydrosManager.StatusDt = DateTime.Now;
                    await context.SaveChangesAsync();
                }
                return Ok(hydrosManager);
            }
        }

        [HttpGet, Route("adwr/pcc/{wrf}")]
        public IHttpActionResult GetPcc(int wrf)
        {
            WaterRightFacility found;
            try
            {
                found = WaterRightFacility.Get(x => x.Id == wrf);
            }
            catch
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.PCC);
        }

        [HttpGet, Route("adwr/wrf/{inputPcc}")]
        public IHttpActionResult GetPcc(string inputPcc)
        {
            WaterRightFacility found;
            try
            {
                PCC validPCC = new PCC(inputPcc);
                found = WaterRightFacility.Get(x => x.Program == validPCC.Program && x.Certificate == validPCC.Certificate && x.Conveyance == validPCC.Conveyance);
            }
            catch
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.PCC);
        }
        [HttpGet, Route("adwr/wrfbypcc/{inputPcc}")]
        public IHttpActionResult GetWrfById(string inputPcc)
        {
            WaterRightFacility found;
            try
            {
                PCC validPCC = new PCC(inputPcc);
                found = WaterRightFacility.Get(x => x.Program == validPCC.Program && x.Certificate == validPCC.Certificate && x.Conveyance == validPCC.Conveyance);
            }
            catch
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.Id);
        }

        [HttpGet, Route("adwr/getWrfById/{id}")]
        public IHttpActionResult GetWrfById(int id)
        {
            WaterRightFacility wrf;
            try
            {
                wrf = WaterRightFacility.Get(x => x.Id == id);
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError(exception);
            }
            return Ok(wrf);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-AAWS")]
        [HttpPut, Route("adwr/UpdateWrf/{id}")]
        public async Task<IHttpActionResult> UpdateWrf([FromBody] WaterRightFacility data, int id)
        {
            data.UpdateBy = User.Identity.Name.Replace("AZWATER0\\", "");
            WaterRightFacility wrf;

            using (var context = new OracleContext())
            {
                wrf = context.WTR_RIGHT_FACILITY.Where(x => x.Id == id).FirstOrDefault();
                if (wrf != null)
                {
                    var props = wrf.GetType().GetProperties().ToList();
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(data);
                        if ((value != null) && (prop.Name != "PCC"))
                        {
                            prop.SetValue(wrf, value);
                        }
                    }
                    await context.SaveChangesAsync();
                }
                return Ok(wrf);
            }
        }

        /// <summary>
        /// Returns object for Counties, AMA and INA, and Basins
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("adwr/geoBoundaries")]
        public IHttpActionResult GetCountyBasinAmaInfo()
        {
            try
            {
                var info = new GeoBoundaryViewModel();
                return Ok(info);
            }
            catch
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpGet, Route("adwr/locationByWrf/{wrf}")]
        public IHttpActionResult LocationByWrf(int wrf)
        {
            try
            {
                var locationList = Location.GetList(x => x.WaterRightFacilityId == wrf);
                ////List<Aws_customer_wrf_ViewModel> customerList = new List<Aws_customer_wrf_ViewModel>();
                //foreach (var custId in custIdList)
                //{
                //    customerList.Add(new Aws_customer_wrf_ViewModel(custId, wrf, custType));
                //}
                return Ok(locationList);
            }
            catch
            {
                //log error
                return InternalServerError();
            }
        }

        [HttpPost, Route("adwr/addCadastralByWrf/{wrf}")]
        public IHttpActionResult AddCadastralByWrf([FromBody] List<Location> LocationList, int wrf)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                using (var context = new OracleContext())
                {
                    foreach (var location in LocationList)
                    {
                        //TO DO check for dups
                        var locationExists = context.LOCATION.Where(x => x.Id == location.Id).FirstOrDefault() != null ? true : false;
                        //var wrfExists = context.WRF_CUST.Where(x => x.WRF_ID == wrfcust.WRF_ID).FirstOrDefault() != null ? true : false;
                        //var count = LOCATION.GetList(x => x.WRF_ID == wrfcust.WRF_ID && x.CUST_ID == wrfcust.CUST_ID && x.CCT_CODE == wrfcust.CCT_CODE).Count();
                        //if (!customerExists || !wrfExists)
                        //{
                        //    return BadRequest("location wrf does not exist");
                        //}
                        if (!locationExists)
                        {
                            location.CreateBy = userName;
                            location.CreateDt = DateTime.Now;
                            context.LOCATION.Add(location);
                        }
                    }
                    context.SaveChanges();
                    var locationList = Location.GetList(x => x.WaterRightFacilityId == wrf);

                    return Ok(locationList);
                }

            }
            catch
            {
                return InternalServerError();
            }
        }
        [HttpPost, Route("adwr/deleteCadastralByWrf/{wrf}")]
        public IHttpActionResult DeleteCadastralByWrf([FromBody] List<Location> LocationList, int wrf)
        {
            try
            {
                string userName = User.Identity.Name.Replace("AZWATER0\\", "");
                using (var context = new OracleContext())
                {
                    foreach (var location in LocationList)
                    {
                        //TO DO check for dups
                        var locationExists = context.LOCATION.Where(x => x.Id == location.Id).FirstOrDefault() != null ? true : false;
                        //var wrfExists = context.WRF_CUST.Where(x => x.WRF_ID == wrfcust.WRF_ID).FirstOrDefault() != null ? true : false;
                        //var count = LOCATION.GetList(x => x.WRF_ID == wrfcust.WRF_ID && x.CUST_ID == wrfcust.CUST_ID && x.CCT_CODE == wrfcust.CCT_CODE).Count();
                        //if (!customerExists || !wrfExists)
                        //{
                        //    return BadRequest("location wrf does not exist");
                        //}
                        if (locationExists)
                        {
                            Location.Delete(location);
                        }
                    }
                    context.SaveChanges();
                    var locationList = Location.GetList(x => x.WaterRightFacilityId == wrf);

                    return Ok(locationList);
                }

            }
            catch
            {
                return InternalServerError();
            }
        }


        [HttpGet, Route("adwr/getplaces/{cads}")]
        public IHttpActionResult GetPlaces(string cads)
        {
          
            try
            {
                using (var context = new SDEContext())
                {
                    var qryString = "select OBJECTID, CITY_NAME, COUNTY_NAME,BASIN_NAME, SUB_NAME from" +
                                      " (" +
                                      " select distinct cnty.OBJECTID, null as CITY_NAME, NAME as COUNTY_NAME,null as BASIN_NAME, null as SUB_NAME from cad.CADASTRAL c, lib.county cnty" +
                                      " where c.cadastral_hook in " + cads + " and sde.st_intersects(c.shape, cnty.shape) = 1" +
                                      " union" +
                                      " select distinct cnty.OBJECTID, null as CITY_NAME, NAME as COUNTY_NAME,null as BASIN_NAME, null as SUB_NAME from lib.PLSTOWNSHIP t, lib.county cnty" +
                                      " where t.township_hook in " + cads + " and sde.st_intersects(t.shape, cnty.shape) = 1" +
                                      " union" +
                                      " select distinct city.OBJECTID, NAME as CITY_NAME, null as COUNTY_NAME,null as BASIN_NAME, null as SUB_NAME from cad.CADASTRAL c, lib.cityincorp city" +
                                      " where c.cadastral_hook in " + cads + " and sde.st_intersects(c.shape, city.shape) = 1" +
                                      " union" +
                                      " select distinct city.OBJECTID, NAME as CITY_NAME, null as COUNTY_NAME,null as BASIN_NAME, null as SUB_NAME from lib.PLSTOWNSHIP t, lib.cityincorp city" +
                                      " where t.township_hook in " + cads + " and sde.st_intersects(t.shape, city.shape) = 1" +
                                      " union" +
                                      " select distinct basin.OBJECTID, null as CITY_NAME, null as COUNTY_NAME, basin.BASIN_NAME as BASIN_NAME, null as SUB_NAME from cad.CADASTRAL c, lib.groundwaterbasinadwr basin"+
                                      " where c.cadastral_hook in " + cads + " and sde.st_intersects(c.shape, basin.shape) = 1" +
                                      " union" +
                                      " select distinct basin.OBJECTID, null as CITY_NAME, null as COUNTY_NAME, basin.BASIN_NAME as BASIN_NAME, null as SUB_NAME from lib.PLSTOWNSHIP t, lib.groundwaterbasinadwr basin" +
                                      " where t.township_hook in " + cads + " and sde.st_intersects(t.shape, basin.shape) = 1" +
                                      " union" +
                                      " select distinct sbasin.OBJECTID, null as CITY_NAME, null as COUNTY_NAME,null as BASIN_NAME, SUBBASIN_NAME as SUB_NAME from cad.CADASTRAL c, lib.groundwatersubbasinadwr sbasin" +
                                      " where c.cadastral_hook in " + cads + " and sde.st_intersects(c.shape, sbasin.shape) = 1" +
                                      " union" +
                                      " select distinct sbasin.OBJECTID, null as CITY_NAME, null as COUNTY_NAME,null as BASIN_NAME, SUBBASIN_NAME as SUB_NAME from lib.PLSTOWNSHIP t, lib.groundwatersubbasinadwr sbasin" +
                                      " where t.township_hook in " + cads + " and sde.st_intersects(t.shape, sbasin.shape) = 1" +
                                      " )";
   
                    var data = QueryResult.RunAnyQuery(qryString, context);
                    return Ok(data);
                }
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError(exception);
            }
        }


    }


    public class ActivePage
    {
        public string Page { get; set; }
        public bool Online { get; set; }
    }
    public class WindowsUser
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

}