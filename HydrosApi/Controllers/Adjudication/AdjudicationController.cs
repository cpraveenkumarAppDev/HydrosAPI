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

namespace HydrosApi.Controllers.Adjudication
{

    public class AdjudicationController : ApiController
    {

        private SDEContext sdeDB = new SDEContext();
        private OracleContext db = new OracleContext();
        //IRR-29-A16011018CBB-01
        // [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
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

        [Route("adj/getpodpwrdwr/{id}")]
        [HttpGet]
        public IHttpActionResult PodListPwrDwr(string id)
        {
            var result=db.PWR_POD.Where(p => p.PROPOSED_WATER_RIGHT.POU_ID==id).ToList();

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        public IEnumerable<PWR_POD> PodList(int id)
        {
            return db.PWR_POD.Where(p => p.PWR_ID == id).ToList();
        }

        [Route("adj/getpod/{id}")]
        [HttpGet]
        public IHttpActionResult GetPod(string id) //USE A PLACE OF USE DWR_ID OR PWR_ID
        {           
            Regex rgx = new Regex(@"[^0-9]");
            var pwrpod = rgx.IsMatch(id) ? db.PWR_POD.Where(p => p.PROPOSED_WATER_RIGHT.POU_ID == id).ToList() :
                PodList(int.Parse(id));

            if(pwrpod == null)
            {
                return NotFound();
            }
            var idList = (from p in pwrpod
                          where p.POD_ID != null
                          select new
                          {

                              value = p.POD_ID.GetValueOrDefault(-1)
                          }).Distinct().Select(i => i.value).ToList();

            var result = sdeDB.POINT_OF_DIVERSION.Where(p => idList.Contains(p.OBJECTID)).ToList();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        /* [Route("adj/getpwrtopod/{id}")] //GET THE ASSOCIATED POD IDS
         [HttpGet]
         public IHttpActionResult GetPwrToPod(string id) //proposed water right ID
         {

             Regex rgx = new Regex(@"[^0-9]");
             var pwrpod = rgx.IsMatch(id) ? db.PWR_POD.Where(p => p.PROPOSED_WATER_RIGHT.POU_ID == id).ToList() :
                 PodList(int.Parse(id)).ToList();

             if (pwrpod == null)
             {
                 return NotFound();
             }
             return Ok(pwrpod);
         }
        */


        //[Route("adj/getpod/{id}")]
       // [HttpGet]

        public IHttpActionResult GetPodx(int id) //proposed water right ID
        {

            //var pwrpod= GetPwrToPod(id);

            var idList = (from p in PodList(id)
                          where p.POD_ID != null
                          select new
                          {

                              value = p.POD_ID.GetValueOrDefault(-1)
                          }).Distinct().Select(i => i.value).ToList();


            var result = sdeDB.POINT_OF_DIVERSION.Where(p => idList.Contains(p.OBJECTID)).ToList();



            if (idList == null)
            {
                return NotFound();
            }
            return Ok(idList);
        }


       // [Route("adj/getpod/{id}")]
       // [HttpGet]

        public IHttpActionResult GetAllPods(int id) //proposed water right ID
        {

            //var pwrpod= GetPwrToPod(id);

            var idList = (from p in PodList(id)
                          where p.POD_ID != null
                          select new
                          {

                              value = p.POD_ID.GetValueOrDefault(-1)
                          }).Distinct().Select(i => i.value).ToList();


            var result = sdeDB.POINT_OF_DIVERSION.Where(p => idList.Contains(p.OBJECTID)).ToList();

          

            if (idList== null)
            {
                return NotFound();
            }
            return Ok(idList);
        }

        [Route("adj/getonepod/{id}")] //GET PODS
        [HttpGet]
        public IHttpActionResult Pod(string id)
        {
            Regex rgx = new Regex(@"[^0-9]");
            List<POINT_OF_DIVERSION> pod = null;
            if (id != null)
            {
                if (rgx.IsMatch(id))
                {

                    pod = sdeDB.POINT_OF_DIVERSION.Where(p => p.DWR_ID == id).ToList();
                }

                else
                {
                    int pid = int.Parse(id);
                    pod = sdeDB.POINT_OF_DIVERSION.Where(p => p.OBJECTID==pid).ToList();
                    
                   

                }
            }
            if (pod == null)
            {
                return NotFound();
            }
            return Ok(pod);


        }

        [Route("adj/getplaceofuse")]
        [HttpGet]
        public IHttpActionResult GetPlaceOfUse()
        {

            var pou = sdeDB.PLACE_OF_USE_VIEW.ToList();

            if (pou == null)
            {
                return NotFound();
            }
            return Ok(pou);
        }

        [Route("adj/getplaceofusebyid/{id}")]
        [HttpGet]
        public IHttpActionResult GetPlaceOfUseById(string id)       
        {
           
            var pou = sdeDB.PLACE_OF_USE_VIEW.Where(p => p.DWR_ID == id).ToList();

            if (pou == null)
            {
                return NotFound();
            }
            return Ok(pou);
        }

        //public IEnumerable<POINT_OF_DIVERSION> GetPodById(int id)
        //{
        //return sdeDB.POINT_OF_DIVERSION.Where(p => p.OBJECTID == id).ToList();
        // }

    }
}


    
 