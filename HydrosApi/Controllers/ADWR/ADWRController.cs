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
                var validUser = new { appEnv = environment, user = user };
                return Ok(validUser);
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
                if(content != null)
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

        [HttpGet, Route("adwr/wrf/{pcc}")]
        public IHttpActionResult GetPcc(string pcc)
        {
            WTR_RIGHT_FACILITY found;
            try
            {
                pcc = pcc.Replace(".", "").Replace("-", "");
                string program = pcc.Substring(0, 2);
                string cert = pcc.Substring(2, 6);
                string conv = pcc.Substring(8);
                found = WTR_RIGHT_FACILITY.Get(x => x.Program == program && x.Certificate == cert && x.Conveyance == conv);
            }
            catch (Exception exception)
            {
                //log exception
                return InternalServerError();
            }
            return Ok(found.PCC);
        }
    }

public class WindowsUser
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
}

}