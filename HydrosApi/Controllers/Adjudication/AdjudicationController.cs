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
    using System.Net;
    using Newtonsoft.Json;
    using System.Net.Http;
    using System.Web.Http.Description;
    using HydrosApi.Data;
    using HydrosApi.ViewModel;
    
  

    //[Authorize] 
    //at minimum, ensure this is an authorized user, granular permissions will be added later
    public class AdjudicationController : ApiController
    {

        //remove this, it was all moved to the place of use model in the process of trying to make  
        //everything faster. Not sure if I succeeded.
        /*
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
        }  */      
      
        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- WEB SERVICE REQUESTS ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------       
        
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
        
        [Route("adj/getallpod")]
        [HttpGet]       
        public async Task<IHttpActionResult> GetAllPod()
        {
            return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion()));
        }

        [HttpGet, Route("adj/getpou/{id?}")]
        public async Task<IHttpActionResult> GetPlaceOfUse(string id = null)
        {             
            if (id != null)
            {
                var pou = Task.FromResult(PLACE_OF_USE_VIEW.PlaceOfUseView(id));                 
                return Ok(pou);
            }
            else
            {
                return Ok(await Task.FromResult(PLACE_OF_USE_VIEW.GetAll())); //return all places of use
            }         
        }

        [HttpGet, Route("adj/getwfr/{id?}")]
        public IHttpActionResult GetWfr(int id)
        {
   
                var wfr = Task.FromResult(WATERSHED_FILE_REPORT.WatershedFileReportByObjectId(id));
                return Ok(wfr);
          
        }

        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- ADD/ DELETE/U PDATE ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------       
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/addpod/{podobjectid}/{pwrId}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPod(int podobjectid, int pwrId)
        {            
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

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
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
        [HttpPost, Route("adj/addfileblob/")] //PWR_ID or an error message is returned       
        public async Task<IHttpActionResult> AddFileBlob() //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<HandleForm>(new HandleForm());
            var fileList = await Task.FromResult(TEST_FILE_UPLOAD.UploadFile(provider));

            if (fileList != null )
            {
                return Ok(fileList);
            }

            return BadRequest("Error Uploading File");
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addfile/")] //PWR_ID or an error message is returned       
        public async Task<IHttpActionResult> AddFile() //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            var provider = await Request.Content.ReadAsMultipartAsync<HandleForm>(new HandleForm());
            var fileList = await Task.FromResult(FILE.UploadFile(provider, User.Identity.Name.Replace("AZWATER0\\", "")));

            if(fileList != null && fileList.STATUS==null)
            {
                return Ok(fileList);
            }

            return BadRequest(fileList.STATUS);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deletefile/{id}")] //pwr id
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
        [HttpPost, Route("adj/addexp/")]
        public async Task<IHttpActionResult> AddExplanation([FromBody] EXPLANATIONS explanation) //Send all form values
        {
            if (!(explanation != null && (explanation.PWR_ID != null || explanation.WFR_ID != null)))
            {
                return BadRequest("An invalid proposed water right ID was entered.");
            }

            var newExplanation = await Task.FromResult(EXPLANATIONS.Add(new EXPLANATIONS()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                PWR_ID = explanation.PWR_ID != null ? explanation.PWR_ID:null,
                WFR_ID = explanation.WFR_ID != null ? explanation.WFR_ID : null,
                EXP_TYPE = explanation.EXP_TYPE,
                EXPLANATION = explanation.EXPLANATION
            }));            

            return Ok(newExplanation);
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deleteexp/{id}")]
        public async Task<IHttpActionResult> DeleteExplanation(int id) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {           
            EXPLANATIONS exp = await Task.FromResult(EXPLANATIONS.Get(p => p.ID == id));

            if (exp == null)
            {
                return BadRequest("An invalid id was entered");
            }

            EXPLANATIONS.Delete(exp);
            return Ok("Explanation deleted");
        }
    }

    internal class NameValueCollection
    {
    }
}
 