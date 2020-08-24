using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity; 
using System.Linq; 
using System.Web.Http;
using HydrosApi.Models;
using System.Text.RegularExpressions; 
using System.Web; 
using System.Threading.Tasks;
using System.Diagnostics;
using HydrosApi.Data;
using System.Runtime.Remoting.Messaging;

namespace HydrosApi.Controllers.Adjudication
{
    public class AdjudicationController : ApiController
    {
        private SDEContext sdeDB = new SDEContext();
        private ADWRContext db = new ADWRContext();


        //CREATE A PROPOSED_WATER_RIGHT RECORD IF IT DOESN'T
        private async Task<IEnumerable<PROPOSED_WATER_RIGHT>> CreateProposedWaterRight(List<PLACE_OF_USE_VIEW> pou)
        {
            if (pou == null)
            {
                return null;
            }

            var newPwr = new PROPOSED_WATER_RIGHT()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POU_ID = pou.Select(p => p.DwrId).FirstOrDefault()
            };

            db.Entry(newPwr).State = EntityState.Added;
            await db.SaveChangesAsync();

            List<PROPOSED_WATER_RIGHT> pwr = new List<PROPOSED_WATER_RIGHT>();
            pwr.Add(newPwr);
            return pwr;
        }

        private async Task<PROPOSED_WATER_RIGHT> AddProposedWaterRight(PLACE_OF_USE_VIEW pou)
        {
            if (pou == null)
            {
                return null;
            }

            var newPwr = new PROPOSED_WATER_RIGHT()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POU_ID = pou.DwrId
            };

            db.Entry(newPwr).State = EntityState.Added;
            await db.SaveChangesAsync();

            return newPwr;
        }

        public async Task<IEnumerable<PLACE_OF_USE_VIEW>> PlaceOfUseForm(string id)
        {
            Regex rgx = new Regex(@"[^0-9]");
            List<PLACE_OF_USE_VIEW> pou = null;

            int? newPwr = null;

            if (!rgx.IsMatch(id))
            {
                return null;
            }

            pou = await sdeDB.PLACE_OF_USE_VIEW.Where(p => p.DwrId == id).ToListAsync();

            if (pou == null)
            {
                return null;
            }

            var pwr = await db.PROPOSED_WATER_RIGHT.Where(p => p.POU_ID == id).FirstOrDefaultAsync();

            if (pwr == null)
            {
                newPwr = 1;
                pwr = await AddProposedWaterRight(pou.FirstOrDefault()); //create a pwr record if not exists
            }

            if (pwr != null)
            {                
               pou.FirstOrDefault().ProposedWaterRight = pwr;
            }

            if (newPwr == null && pwr != null)
            {
                var pwrToPod= await PwrPodList(pwr.ID);
                
                if(pwrToPod != null)
                {                 
                    var matchList = pwrToPod.Select(i => i.POD_ID ?? -1).Distinct();                    

                    var pod = (from pd in (await sdeDB.POINT_OF_DIVERSION.Where(p => matchList.Contains(p.OBJECTID)).ToListAsync())
                    join pp in pwrToPod.ToList() on pd.OBJECTID equals pp.POD_ID ?? -1
                    select new
                    {
                        pd,
                        PWR_PID = pd.PWR_POD_ID = pp.ID
                    }).Distinct().Select(x => x.pd).ToList();

                    
                    if(pod != null)
                    {
                        pou.FirstOrDefault().PointOfDiversion = pod;
                    }
                }               
            }

            var socField = pou.Select(i => i.SOC).FirstOrDefault();

            if (socField !=null)
            {                 
                var socList = (from f in socField.Split(',').ToList<string>()
                               select new
                               {
                                   fileId = int.Parse(rgx.Replace(f.Replace("35-", ""),""))

                               }).Select(f => f.fileId).Distinct();
                
                var soc = await db.SOC_AIS_VIEW.Where(s => socList.Contains(s.FILE_NO)).ToListAsync();
                if (soc != null)
                {
                    pou.FirstOrDefault().StatementOfClaim = soc;
                }
            }            
            return pou;
        }

        //GET PROPOSED_WATER_RIGHT RECORD USING A PROPOSED_WATER_RIGHT.ID 
        //*OR* POU_ID (A DWR_ID)

        private async Task<IEnumerable<PROPOSED_WATER_RIGHT>> ProposedWaterRight(string id)

        {
            Regex rgx = new Regex(@"[^0-9]");


            if (rgx.IsMatch(id))
            {
                return await db.PROPOSED_WATER_RIGHT.Where(p => p.POU_ID == id).ToListAsync();
            }
            else
            {

                var numericId = int.Parse(id);

                return await db.PROPOSED_WATER_RIGHT.Where(p => p.ID == numericId).ToListAsync();
            }
        }

        private async Task<IEnumerable<PWR_POD>> PwrPodList(int id) //GET PROPOSED_WATER_RIGHT RECORD USING THE PROPOSED_WATER_RIGHT.ID
        {
            return await db.PWR_POD.Where(p => p.PWR_ID == id).ToListAsync();
        }

        private async Task<IEnumerable<POINT_OF_DIVERSION>> PodObjectList(string id) //GET A POINT_OF_DIVERSION RECORD
        {
            Regex rgx = new Regex(@"[^0-9]");

            if (rgx.IsMatch(id))
            {
                return await sdeDB.POINT_OF_DIVERSION.Where(p => p.DwrId == id).ToListAsync();
            }
            else
            {
                return await sdeDB.POINT_OF_DIVERSION.Where(p => p.OBJECTID == int.Parse(id)).ToListAsync();
            }
        }

        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- WEB SERVICE REQUESTS ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------

        //IRR-29-A16011018CBB-01
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/getpwr/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProposedWaterRight(string id)
        {
            if (id == null)
            {
                return BadRequest("Please enter a valid ID or DwrId");
            }

            var pwr = await ProposedWaterRight(id);

            if (pwr == null)
            {
                return NotFound();
            }
            return Ok(pwr);
        }


        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/getallpod")]
        [HttpGet]
        //USE A PROPOSED_WATER_RIGHT.ID OR PROPOSED_WATER_RIGHT.POU_ID/PLACE_OF_USE.DWR_ID
        //*or*
        //adj/getpod (NO PARAMETER) RETURNS ALL PODS
        public async Task<IHttpActionResult> GetAllPod() 
        {
            return Ok(await sdeDB.POINT_OF_DIVERSION.ToListAsync());
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addpod/{podobjectid}/{pwrId}")]
        //add a relationship between the Place Of Use and Point of Diversion
        //the pwrId should be stored in the Place of Use model
        public async Task<IHttpActionResult> AddPod(int podobjectid,int pwrId)  
        {
            //FIND THE PROPOSED WATER RIGHT
            var pwrPodList = await db.PWR_POD.Where(p => (p.POD_ID ?? -1) == podobjectid && (p.PWR_ID ?? -1) == pwrId).ToListAsync();            

            if(pwrPodList.Count() > 0)
            {   
                return BadRequest("A relationship already exists for this Place of Use and Point of Diversion");
            }

            var newPwrPod = new PWR_POD()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POD_ID = podobjectid,
                PWR_ID=pwrId
            };
            db.Entry(newPwrPod).State = EntityState.Added;
            await db.SaveChangesAsync();
            return Ok(newPwrPod);         
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete,Route("adj/deletepod/{id}")]
        public async Task<IHttpActionResult> DeletePod(int id) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            PWR_POD pod = await db.PWR_POD.Where(i => i.ID == id).FirstOrDefaultAsync();
            PROPOSED_WATER_RIGHT pwr = await db.PROPOSED_WATER_RIGHT.Where(p => p.ID == pod.PWR_ID.GetValueOrDefault(-1)).FirstOrDefaultAsync();

            if(pod==null || pwr==null)
            {
                return BadRequest("An invalid id was entered");
            }           

            db.Entry(pod).State = EntityState.Deleted;
            await db.SaveChangesAsync();           
            return Ok();
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpGet, Route("adj/getpou/{id?}")]
        public async Task<IHttpActionResult> GetPlaceOfUse(string id = null)
        {
            List<PLACE_OF_USE_VIEW> pou = null;
            if (id != null)
            {
                var pouForm = await PlaceOfUseForm(id);

                if(pouForm==null)
                {
                   return NotFound();
                }

                return Ok(pouForm.ToList());
            }

            else
            {
                pou = await sdeDB.PLACE_OF_USE_VIEW.ToListAsync();
            }

            if (pou == null)
            {
                return NotFound();
            }
            return Ok(pou);
        }
    }
}


    
 