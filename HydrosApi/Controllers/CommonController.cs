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
        [HttpGet, Route("common/user")]
        public async Task<IHttpActionResult> GetUserInformation()
        {
            var user = User.Identity.Name.Replace("AZWATER0\\", "").ToLower();
            var userInfo = await Task.FromResult(AW_USERS.Get(u => u.EMAIL.ToLower().Replace("@azwater.gov","") == user));
            
            if(userInfo==null)
            {
                return BadRequest("Unable to get user information");
            }

            userInfo.ADUser = user;
            return Ok(userInfo);             
        }
    }
}