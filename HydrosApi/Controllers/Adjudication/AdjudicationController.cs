namespace HydrosApi
{
    using System;
    using System.Collections.Generic;   
    using System.Linq;
    using System.Web.Http;  
    using Models;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;    
    using System.Configuration;
    using System.IO;

    //[Authorize] //at minimum, ensure this is an authorized user, granular permissions will be added later
    public class AdjudicationController : ApiController
    {
        private async Task<PROPOSED_WATER_RIGHT> AddProposedWaterRight(PLACE_OF_USE_VIEW pou)
        {
            PROPOSED_WATER_RIGHT status = new PROPOSED_WATER_RIGHT();

            if (pou == null)
            {
                status.StatusMessage = "Invalid place of use was submitted";
                return status;
            }
                     
            return await Task.FromResult(PROPOSED_WATER_RIGHT.Add(new PROPOSED_WATER_RIGHT()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POU_ID = pou.DWR_ID
            })); 
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
            var pou =  await Task.FromResult(PLACE_OF_USE_VIEW.Get(p=>p.DWR_ID==id));

            if (pou == null)
            {
                return null;                
            }

            //if there is no proposed water right, then create one
           
            var pwr = await Task.FromResult(PROPOSED_WATER_RIGHT.ProposedWaterRight(id));

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
                pou.FileList = await Task.FromResult(FILE.GetList(f => f.PWR_ID == pwr.ID));
            }

            //if a proposed water right already exists (and wasn't just added) get the associated pods
            if (newPwr == null && pwr != null)
            {
                var pwrToPod = await Task.FromResult(PWR_POD.GetList(p => (p.PWR_ID ?? -1) ==pwr.ID));                  

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

            pou.Explanation = await Task.FromResult(EXPLANATIONS.GetList(i => i.PWR_ID == pwr.ID));

            placeOfUse.Add(pou);
            return placeOfUse;
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

           var pwr= await Task.FromResult(PROPOSED_WATER_RIGHT.ProposedWaterRight(id));
 
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
            //var pwrPodList = await Task.FromResult(PWR_POD.ProposedWaterRightToPoint(podobjectid, pwrId));
            var pwrPodList = await Task.FromResult(PWR_POD.GetList(p => (p.POD_ID ?? -1) == podobjectid && (p.PWR_ID ?? -1) == pwrId));

            if (pwrPodList != null && pwrPodList.Count() > 0)
            {
                return BadRequest("A relationship already exists for this Place of Use and Point of Diversion");
            }

            var newPwrPod = PWR_POD.Add(new PWR_POD()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                POD_ID = podobjectid,
                PWR_ID = pwrId
            });
            
            return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion(newPwrPod)));
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deletepod/{id}")]
        public async Task<IHttpActionResult> DeletePod(int id) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            PWR_POD pod = await Task.FromResult(PWR_POD.Get(p=>p.ID==id));

            if (pod == null)
            {
                return BadRequest("An invalid id was entered");
            }
            PWR_POD.Delete(pod);
            return Ok("Point of Diversion Deleted");
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deleteexp/{id}")]
        public async Task<IHttpActionResult> DeleteExplanation(int id) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {            
            // EXPLANATIONS exp = await db.EXPLANATIONS.Where(i => i.ID == id).FirstOrDefaultAsync();
            EXPLANATIONS exp = await Task.FromResult(EXPLANATIONS.Get(p => p.ID == id));

            if (exp == null)
            {
                return BadRequest("An invalid id was entered");
            }

            EXPLANATIONS.Delete(exp);
            return Ok("Explanation deleted");             
        }

        //[Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addfile")] //pwr id
        public async Task<IHttpActionResult> AddFile([FromBody] FILE info) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {           
            Guid guid = Guid.NewGuid();           
            var uploadFilePath = @"" + ConfigurationManager.AppSettings["FileUploadLocation"];
            var fileType= Path.GetExtension(info.ORIGINAL_FILE_NAME).ToLower();
            var uploadFileName = guid + fileType;           
                              
            File.Copy(info.ORIGINAL_FILE_NAME, Path.Combine(uploadFilePath,uploadFileName));

            var newFile = await Task.FromResult(FILE.Add(new FILE()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                LOCATION = Path.Combine(uploadFilePath, uploadFileName),
                TYPE = fileType,
                ORIGINAL_FILE_NAME = Path.GetFileName(info.ORIGINAL_FILE_NAME),               
                PWR_ID = info.PWR_ID,
                DESCRIPTION=info.DESCRIPTION
            }));
                        
            return Ok(newFile);
        }

        [HttpPost, Route("adj/deletefile/{id}")] //pwr id
        public async Task<IHttpActionResult> DeleteFile(int id) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            Regex rgx = new Regex(@"file:/{1,}");
            var fileRecord = await Task.FromResult(FILE.Get(f => f.ID == id));

            if(fileRecord == null)
            {
                return BadRequest("The file was not found.");
            }

            if (fileRecord.LOCATION.IndexOf(ConfigurationManager.AppSettings["FileUploadLocation"]) == -1)
            {
                return BadRequest("This file could not be deleted.");
            }

            var fileExists = File.Exists(fileRecord.LOCATION);          
            if (!fileExists)
            {
                return Ok("The file record was deleted but the physical file was not found");
            }

            FILE.Delete(fileRecord);
            File.Delete(fileRecord.LOCATION);          
            return Ok("File successfully deleted");
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addexp/{data}")]
        public async Task<IHttpActionResult> AddExplanation([FromBody] EXPLANATIONS explanation) //SEND ALL VALUES, USE DATABASE/MODEL COLUMN NAMES
        {
            if (!(explanation != null && explanation.PWR_ID != null))
            {
                return BadRequest("An invalid proposed water right ID was entered.");
            }

            var newExplanation = await Task.FromResult(EXPLANATIONS.Add(new EXPLANATIONS()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                PWR_ID = explanation.PWR_ID,
                EXP_TYPE = explanation.EXP_TYPE,
                EXPLANATION = explanation.EXPLANATION
            }));            

            return Ok(newExplanation);
        }

        [HttpGet, Route("adj/getpou/{id?}")]
        public async Task<IHttpActionResult> GetPlaceOfUse(string id = null)
        {
            List<PLACE_OF_USE_VIEW> pou;
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
                pou=await Task.FromResult(PLACE_OF_USE_VIEW.GetAll()); //return all places of use
            }

            if (pou == null)
            {
                return NotFound();
            }
            return Ok(pou);
        }
    }
}
 