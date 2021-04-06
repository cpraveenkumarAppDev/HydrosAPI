using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using HydrosApi.Models.ADWR;

namespace HydrosApi.Controllers
{
    [Authorize]
    public class CommonController:ApiController
    {       
        [HttpGet, Route("common/user/{id?}")]
        public async Task<IHttpActionResult> GetUserInformation(string id = null)
        {
            var user = id != null ? id.ToLower() : User.Identity.Name.Replace("AZWATER0\\", "").ToLower();
            var userInfo = await Task.FromResult(AwUsers.Get(u => u.Email.ToLower().Replace("@azwater.gov","") == user && u.Active=="Y"));
            
            if(userInfo==null)
            {
                if(user != null)
                {
                    return BadRequest("Unable to get user information for "+user);
                }
                return BadRequest("Unable to get user information");
            }

            userInfo.ActiveDirectoryUser = user;
            return Ok(userInfo);             
        }
    }
}