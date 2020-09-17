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
    using System.Web.Http.Cors;

    public class AdjudicationController : ApiController
    {
        private SDEContext sdeDB = new SDEContext();
        private ADWRContext db = new ADWRContext();

        
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

        //PlaceOfUseForm returns EVERYTHING needed to populate the form
        public async Task<IEnumerable<PLACE_OF_USE_VIEW>> PlaceOfUseForm(string id)
        {
            Regex rgx = new Regex(@"[^0-9]");
            List<PLACE_OF_USE_VIEW> placeOfUse = new List<PLACE_OF_USE_VIEW>();
            //var userInRole = RoleCheck.ThisUser("AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications");

            int? newPwr = null;

            //The id should be the DWR_ID, at least make sure it has letters in it
            if (!rgx.IsMatch(id))
            {
                return null;
            }

            //Get the place of use
            var pou = await sdeDB.PLACE_OF_USE_VIEW.Where(p => p.DWR_ID == id).FirstOrDefaultAsync();                 

            if (pou == null)
            {
                return null;                
            }

            //if there is no proposed water right, then create one
            var pwr = await db.PROPOSED_WATER_RIGHT.Where(p => p.POU_ID == id).FirstOrDefaultAsync();

            if (pwr == null) 
            {
                newPwr = 1;
                pwr = await AddProposedWaterRight(pou);
            }

            //now there should be a proposed water right but check anyway
            //and populate the pwr.ID in for the place of use, and the ProposedWaterRight List  

            if (pwr != null)
            {
                pou.PWR_ID = pwr.ID;
                pou.ProposedWaterRight = pwr;
            }

            //if a proposed water right already exists (and wasn't just added) get the associated pods
            if (newPwr == null && pwr != null)
            {
                var pwrToPod = await Task.FromResult(PWR_POD.ProposedWaterRightAllPoint(pwr.ID));

                if (pwrToPod != null)
                {
                    //pass the PWR_POD relationship list in.  This will add the PWR_POD's ID 
                    //to the PointOfDiversion so that it can be passed if the pod is deleted
                    var pod = await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion(pwrToPod));
                    if(pod!=null)
                    {
                        pou.PointOfDiversion = pod;
                    }
                }
            }

            //Statements of Claim, Wells and Surfacewater
            //Place of Use table has comma delimited list for these
            var socField = pou.SOC;

            if (socField != null)
            {
                pou.StatementOfClaim = await Task.FromResult(SOC_AIS_VIEW.StatementOfClaimView(socField));               
            }

            var bocField = pou.BAS_OF_CLM;

            if (bocField != null)
            {
                pou.Well = await Task.FromResult(WELLS_VIEW.WellsView(bocField));
                pou.Surfacewater = await Task.FromResult(SW_AIS_VIEW.SurfaceWaterView(bocField));
            }

            pou.Explanation = await db.EXPLANATIONS.Where(i => i.PWR_ID == pwr.ID).ToListAsync();

            placeOfUse.Add(pou);
            return placeOfUse;
        }
      
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

        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- WEB SERVICE REQUESTS ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------
       
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
        public async Task<IHttpActionResult> GetAllPod()
        {
            return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion()));
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/addpod/{podobjectid}/{pwrId}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPod(int podobjectid, int pwrId)
        {            
            var pwrPodList = await Task.FromResult(PWR_POD.ProposedWaterRightToPoint(podobjectid, pwrId));

            if (pwrPodList != null && pwrPodList.Count() > 0)
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

            return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion(newPwrPod)));
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deletepod/{id}")]
        public async Task<IHttpActionResult> DeletePod(int id) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            PWR_POD pod = await Task.FromResult(PWR_POD.ProposedWaterRightToPoint(id));

            if (pod == null)
            {
                return BadRequest("An invalid id was entered");
            }

            db.Entry(pod).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Ok("Point of Diversion Deleted");
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
            return Ok("Explanation deleted");
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