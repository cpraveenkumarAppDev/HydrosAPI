
namespace HydrosApi 
{
    using System.Web.Http;

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