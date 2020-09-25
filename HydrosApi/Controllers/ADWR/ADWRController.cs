using System.Web.Http;
using System.DirectoryServices.AccountManagement;
using System.Collections.Generic;
using System;

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
            if (user.Equals(""))
            {
                return Unauthorized();
            }
            else
            {
                return Ok(user);
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
    }

    public class WindowsUser
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }

}