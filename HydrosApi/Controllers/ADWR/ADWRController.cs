using System.Web.Http;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System;
using System.Configuration;
using HydrosApi.Services;

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
            var environment = ConfigurationManager.ConnectionStrings["ADWRContext"].ToString().Split(';')[0].Split('=')[1] != "ADWR";
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

        [HttpGet, Route("adwr/error")]
        public IHttpActionResult SubmitError(string content)
        {
            try
            {
                var sentOkay = EmailService.Message("appdev@azwater.gov", $"{Environment.MachineName} - HydrosAPI", content);
                return Ok($"Message sent: {sentOkay}");
            }
            catch (Exception exception)
            {
                return Ok("Failed to send noticiation");
            }

        }
    }

public class WindowsUser
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
}

}