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
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Data.Entity.Core.Common.CommandTrees;

namespace HydrosApi.Controllers.Adjudication
{

    public class AdjudicationController : ApiController
    {
        private SDEContext sdeDB = new SDEContext();
        private OracleContext db = new OracleContext();
        //IRR-29-A16011018CBB-01
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/getproposedwaterright/{id}")]
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

        public IEnumerable<PWR_POD> PodList(int id)
        {
            return db.PWR_POD.Where(p => p.PWR_ID == id).ToList();
        }

        [Route("adj/getpod/{id?}")]
        [HttpGet]
        //USE A PROPOSED_WATER_RIGHT.ID OR PROPOSED_WATER_RIGHT.POU_ID/PLACE_OF_USE.DWR_ID
        //*or*
        //adj/getpod (NO PARAMETER) RETURNS ALL PODS
        public IHttpActionResult GetPod(string id=null) 
        {           
            Regex rgx = new Regex(@"[^0-9]");

            List<POINT_OF_DIVERSION> result = null;
            if (id != null)
            {

                var pwrpod = rgx.IsMatch(id) ? db.PWR_POD.Where(p => p.PROPOSED_WATER_RIGHT.POU_ID == id).ToList() :
                    PodList(int.Parse(id));

                if (pwrpod == null)
                {
                    return NotFound();
                }
               
                //SAVE THE PWR_POD_ID
                var idList = (from p in pwrpod
                              where p.POD_ID != null
                              select new
                              {
                                    p.ID,
                                  POD_ID = p.POD_ID.GetValueOrDefault(-1)
                              }).Distinct().Select(i => i).ToList();

                var matchIdList = idList.Select(i => i.POD_ID).ToList();

                result = sdeDB.POINT_OF_DIVERSION.Where(p => matchIdList.Contains(p.OBJECTID)).ToList();

                result = (from p in result                                 
                               join i in idList on p.OBJECTID equals i.POD_ID  
                               select new {                                   
                                   PWR_PID=p.PWR_POD_ID=i.ID, //assign pwr_pod.id this should be submitted for deletes and updates to pod
                                   p //get everything from result
                               }).Select(p=>p.p).Distinct().ToList();
            }
            else // SHOW ALL PODS WHEN NO VALUE IS SUBMITTED
            {
                result = sdeDB.POINT_OF_DIVERSION.ToList();
            }
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete,Route("adj/deletepod/{id}")]        
        public IHttpActionResult DeletePod(int id) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            PWR_POD pod = db.PWR_POD.Where(i => i.ID == id).FirstOrDefault();

            if(pod==null)
            {
                return BadRequest("An invalid id was entered");
            }           

            db.Entry(pod).State = EntityState.Deleted;
            db.SaveChanges();           
            return Ok();
        }

        [Route("adj/getplaceofuse/{id?}")]
        [HttpGet]
        public IHttpActionResult GetPlaceOfUse(string id=null)
        {
            List<PLACE_OF_USE_VIEW> pou = null;

            if(id !=null)
            {
                pou = sdeDB.PLACE_OF_USE_VIEW.Where(p=>p.DwrId==id).ToList();
            }
            else
            {
                pou = sdeDB.PLACE_OF_USE_VIEW.ToList();
            }             

            if (pou == null)
            {
                return NotFound();
            }
            return Ok(pou);
        }
    }
}


    
 