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

namespace AdwrApi.Controllers.Adjudications
{
    public class AISController : ApiController
    {

        private SDEContext sdeDB = new SDEContext();
        private OracleContext db = new OracleContext();

        //IRR-29-A16011018CBB-01
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/ais/getproposedwaterright/{id}")]
        [HttpGet]
        public IHttpActionResult GetProposedWaterRight(string id)
        {
            Regex rgx = new Regex(@"[^0-9]");
            List<PROPOSED_WATER_RIGHT> pwr = null;

            if (id != null)
            {

                if (rgx.IsMatch(id))
                {
                    pwr = db.PROPOSED_WATER_RIGHT.Where(p => p.POU_ID == id).ToList();
                }
                else
                {
                    pwr = db.PROPOSED_WATER_RIGHT.Where(p => p.ID == int.Parse(id)).ToList();
                }
            }

            if (pwr == null)
            {
                return NotFound();
            }
            return Ok(pwr.ToList());
        }
    }
}