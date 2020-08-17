using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HydrosApi.Models;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Cors;

namespace AdwrApi.Controllers
{
    public class ADWRController : ApiController
    {

        [HttpGet]
        [Route("adwr/windows")]
        [System.Web.Http.Authorize]
        public IHttpActionResult WindowsAuthentication()
        {

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
    }
}