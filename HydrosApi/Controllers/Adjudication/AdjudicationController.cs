namespace HydrosApi
{

    using System;
    using System.Collections.Generic;

    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;
    using Models;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;



    //[Authorize]
    public class AdjudicationController : ApiController
    {
        private SDEContext sdeDB = new SDEContext();
        private ADWRContext db = new ADWRContext();

        ///private async Task<PROPOSED_WATER_RIGHT> AddProposedWaterRight(PLACE_OF_USE_VIEW pou)
        private async Task<PROPOSED_WATER_RIGHT> AddProposedWaterRight(PLACE_OF_USE_VIEW pou)
        {
            PROPOSED_WATER_RIGHT status = new PROPOSED_WATER_RIGHT();

            if (pou == null)
            {
                status.StatusMessage = "Invalid place of use was submitted";
                return status;
            }

            var newPwr = new PROPOSED_WATER_RIGHT()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POU_ID = pou.DWR_ID
            };

            db.Entry(newPwr).State = EntityState.Added;
            await db.SaveChangesAsync();
            return newPwr;
        }

        public async Task<IEnumerable<PLACE_OF_USE_VIEW>> PlaceOfUseForm(string id)
        {
            Regex rgx = new Regex(@"[^0-9]");
            List<PLACE_OF_USE_VIEW> placeOfUse = new List<PLACE_OF_USE_VIEW>();

            //var userInRole = RoleCheck.ThisUser("AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications");

            int? newPwr = null;

            if (!rgx.IsMatch(id))
            {
                return null;
            }

            var pou = await sdeDB.PLACE_OF_USE_VIEW.Where(p => p.DWR_ID == id).FirstOrDefaultAsync();

            if (pou == null)
            {
                return null;
            }

            var pwr = await db.PROPOSED_WATER_RIGHT.Where(p => p.POU_ID == id).FirstOrDefaultAsync();

            if (pwr == null) //User must be in role to Add
            {
                newPwr = 1;

                pwr = await AddProposedWaterRight(pou);
            }

            if (pwr != null)
            {
                pou.PWR_ID = pwr.ID;
                pou.ProposedWaterRight = pwr;
            }

            if (newPwr == null && pwr != null)
            {
                var pwrToPod = await PwrPodList(pwr.ID);

                if (pwrToPod != null)
                {
                    var matchList = pwrToPod.Select(i => i.POD_ID ?? -1).Distinct();

                    var pod = (from pd in (await sdeDB.POINT_OF_DIVERSION.Where(p => matchList.Contains(p.OBJECTID)).ToListAsync())
                               join pp in pwrToPod.ToList() on pd.OBJECTID equals pp.POD_ID ?? -1
                               select new
                               {
                                   pd,
                                   PWR_PID = pd.PWR_POD_ID = pp.ID
                               }).Distinct().Select(x => x.pd).ToList();

                    if (pod != null)
                    {
                        pou.PointOfDiversion = pod;
                    }
                }
            }

            //STATEMENTS OF CLAIM INFORMATION           

            var socField = pou.SOC;

            if (socField != null)
            {
                pou.StatementOfClaim = SOC_AIS_VIEW.StatementOfClaimView(socField);
                pou.Surfacewater = SW_AIS_VIEW.SurfaceWaterView(socField);
            }

            var bocField = pou.BAS_OF_CLM;

            if (bocField != null)
            {
                pou.Well = WELLS_VIEW.WellsView(bocField);
            }

            pou.Explanation = await db.EXPLANATIONS.Where(i => i.PWR_ID == pwr.ID).ToListAsync();

            placeOfUse.Add(pou);
            return placeOfUse;
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

        private async Task<POINT_OF_DIVERSION> PodByPodId(string id) //GET A SINGLE POINT_OF_DIVERSION RECORD
        {
            Regex rgx = new Regex(@"[^0-9]");

            if (rgx.IsMatch(id))
            {
                return await sdeDB.POINT_OF_DIVERSION.Where(p => p.DWR_ID == id).FirstOrDefaultAsync();
            }
            else
            {
                return await sdeDB.POINT_OF_DIVERSION.Where(p => p.OBJECTID == int.Parse(id)).FirstOrDefaultAsync();
            }
        }

        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- WEB SERVICE REQUESTS ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------

        //IRR-29-A16011018CBB-01
        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
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

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
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
        [Route("adj/addpod/{podobjectid}/{pwrId}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPod(int podobjectid, int pwrId)
        {
            //Ensure a relationship doesn't already exist
            var pwrPodList = await db.PWR_POD.Where(p => (p.POD_ID ?? -1) == podobjectid && (p.PWR_ID ?? -1) == pwrId).ToListAsync();

            if (pwrPodList.Count() > 0)
            {
                return BadRequest("A relationship already exists for this Place of Use and Point of Diversion");
            }

            var newPwrPod = new PWR_POD()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POD_ID = podobjectid,
                PWR_ID = pwrId
            };
            db.Entry(newPwrPod).State = EntityState.Added;
            await db.SaveChangesAsync();

            var pod = PodByPodId(podobjectid.ToString()).Result;
            pod.PWR_POD_ID = newPwrPod.ID;
            return Ok(pod);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deletepod/{id}")]
        public async Task<IHttpActionResult> DeletePod(int id) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            PWR_POD pod = await db.PWR_POD.Where(i => i.ID == id).FirstOrDefaultAsync();

            if (pod == null)
            {
                return BadRequest("An invalid id was entered");
            }

            db.Entry(pod).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Ok("Deleted");
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deleteexp/{id}")]
        public async Task<IHttpActionResult> DeleteExplanation(int id) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            EXPLANATIONS exp = await db.EXPLANATIONS.Where(i => i.ID == id).FirstOrDefaultAsync();

            if (exp == null)
            {
                return BadRequest("An invalid id was entered");
            }

            db.Entry(exp).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addexp/{id}")]
        public async Task<IHttpActionResult> AddExplanation([FromBody] EXPLANATIONS explanation) //SEND ALL VALUES, USE DATABASE/MODEL COLUMN NAMES
        {
            if (!(explanation != null && explanation.PWR_ID != null))
            {
                return BadRequest("An invalid proposed water right ID was entered.");
            }

            var newExplanation = new EXPLANATIONS()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                PWR_ID = explanation.PWR_ID,
                EXP_TYPE = explanation.EXP_TYPE,
                EXPLANATION = explanation.EXPLANATION
            };
            db.Entry(newExplanation).State = EntityState.Added;
            await db.SaveChangesAsync();

            return Ok(newExplanation);
        }

        [HttpGet, Route("adj/getpou/{id?}")]
        public async Task<IHttpActionResult> GetPlaceOfUse(string id = null)
        {
            List<PLACE_OF_USE_VIEW> pou = null;
            if (id != null)
            {
                var pouForm = await PlaceOfUseForm(id);

                if (pouForm == null)
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