using System.Web.Http;
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
                var page1 = new ActivePage() { Page = "adjudications", Online=true};
                var page2 = new ActivePage() { Page = "aaws", Online = true };
                var page3 = new ActivePage() { Page = "logs", Online = false };
                pages.Add(page1);
                pages.Add(page2);
                pages.Add(page3);
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "AZWATER0"))
                {
                    UserPrincipal foundUsername = UserPrincipal.FindByIdentity(ctx, User.Identity.Name);
                    GroupPrincipal appDevGroup = GroupPrincipal.FindByIdentity(ctx, "PG-APPDEV");
                    bool foundUserInAppDevGroup = foundUsername.IsMemberOf(appDevGroup);
                    var validUser = new { appEnv = environment, user = user, admin = foundUserInAppDevGroup, activeApps = pages };
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
            catch (Exception exception)
            {
                return Ok("Failed to send noticiation");
            }

        }

        [HttpGet, Route("adwr/pcc/{wrf}")]
        public IHttpActionResult GetPcc(int wrf)
        {
            WTR_RIGHT_FACILITY found;
            try
            {
                found = WTR_RIGHT_FACILITY.Get(x => x.ID == wrf);
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.PCC);
        }

        [HttpGet, Route("adwr/wrf/{inputPcc}")]
        public IHttpActionResult GetPcc(string inputPcc)
        {
            WTR_RIGHT_FACILITY found;
            try
            {
                PCC validPCC = new PCC(inputPcc);
                found = WTR_RIGHT_FACILITY.Get(x => x.Program == validPCC.Program && x.Certificate == validPCC.Certificate && x.Conveyance == validPCC.Conveyance);
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.PCC);
        }
        /// <summary>
        /// Returns object for Counties, AMA & INA, and Basins
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
            catch (Exception exception)
            {
                //log error
                return InternalServerError();
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