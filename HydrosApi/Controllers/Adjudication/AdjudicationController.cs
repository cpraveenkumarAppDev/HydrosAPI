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
    using HydrosApi.Models.Adjudication;



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
        [Route("adj/getmapurls")]
        [HttpGet]
        public IHttpActionResult GetMapUrls()
        {
  
            string[] repositoryUrls = ConfigurationManager.AppSettings.AllKeys
                 .Where(key => key.StartsWith("gisAdjUrl"))
                 .Select(key => ConfigurationManager.AppSettings[key])
                 .ToArray();
            return Ok(repositoryUrls);

        }
        [Route("adj/getpwr/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProposedWaterRight(string id)
        {
            if (id == null)
            {
                return BadRequest("Please enter a valid ID or DwrId");
            }

            var pwr = await Task.FromResult(PROPOSED_WATER_RIGHT.ProposedWaterRight(id));

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

        [Route("adj/managepod/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetPod(int id)
        {
            try
            {

                return Ok(await Task.FromResult(POINT_OF_DIVERSION_VIEW.PointOfDivsersionView(id)));
            }
            catch (Exception exception)
            {
                return Ok(exception);
            }
        }
        [HttpGet, Route("adj/getwfr/{id?}")]
        public IHttpActionResult GetWfr(int? id = null)
        {
            if (id != null)
            {
                var wfr = Task.FromResult(WATERSHED_FILE_REPORT.WatershedFileReportByObjectId(id));
                return Ok(wfr);
            }
            else
            {
                return Ok(WATERSHED_FILE_REPORT_SDE.GetAll());
            }
        }       

        [HttpGet, Route("adj/crop/{code?}")]
        public IHttpActionResult GetCrop(string code = null)
        {
            if (code != null)
            {
                return Ok(Task.FromResult(CropCode.Get(c=>c.Code==code)));
               
            }
            else
            {
                return Ok(Task.FromResult(CropCode.GetAll()));
            }
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/updatenoa/")]        
        public IHttpActionResult UpdateNoticeOfAppropriation([FromBody] NoticeOfAppropriation noa) //like pcc but program/file_no/file_ext
        {
            int? claimantId = 0;
            var user = User.Identity.Name.Replace("AZWATER0\\", "");

            var noaContainer = new List<NoticeOfAppropriation>();
            if(noa==null)
            {
                return BadRequest("No data was updated because no data was submitted");
            }

           if(noa.Program == null || noa.FileNo==null || noa.FileExt==null)
            {
                return BadRequest("The Program, File Number and File Extension must have a value");
            }

            //restore option             
            noa.RestoreRecord = false;

            if (noa.Id != null && noa.RestoreRecord == true)
            {
                //make sure restorerecord doesn't exist
                //var restoreRecord = noaContainer.Where(d => d.Id == noa.Id);

                    noa.UpdateBy = user;
                    noa.UpdateDt = DateTime.Now;

                var restore=NoticeOfAppropriation.Add(noa);
                    noaContainer = NoticeOfAppropriation.GetAll();
                    return Ok(noaContainer);                     
            }

            else if (noa.Id != null && noa.DeleteRecord==true)
            {

                /* noaContainer = NoticeOfAppropriation.GetAll();
                 var deleteRecord = noaContainer.Where(d => d.Id == noa.Id && noa.DeleteRecord==true).FirstOrDefault();
                 noaContainer.Where(d => d.Id == noa.Id).FirstOrDefault().RestoreRecord = true;*/


                var deleteRecord = NoticeOfAppropriation.Get(d => d.Id == noa.Id);
                NoticeOfAppropriation.Delete(deleteRecord);
                return Ok(NoticeOfAppropriation.GetAll());
            }
            else  
            {
                var existing = NoticeOfAppropriation.Get(e => e.Program == noa.Program && e.FileNo == noa.FileNo && e.FileExt == noa.FileExt);

                if ((existing != null && ((noa.Id != null && existing.Id != noa.Id) || noa.Id == null))) {
                    //make sure a duplicate program/fileno/fileext combination isn't being added/updated
                    return BadRequest(string.Format("Could not {0} the record. A record with the file number {1} already exists",noa.Id==null ? "update" : "add",existing.FileNumber));
                }

                if (noa.ClaimantNew != null && noa.ClaimantId==null)
                {
                    var newClaimant = new NoticeOfAppropriationClaimant() { Claimant = noa.ClaimantNew.ToUpper(), CreateBy = user };
                    var ncu = NoticeOfAppropriationClaimant.Add(newClaimant);
                        
                      
                    if(ncu != null)
                    {
                        claimantId = ncu.Id;
                        noa.ClaimantId = claimantId;
                    }                        
                }

                if(noa.Id != null)
                {
                    noa.UpdateBy = user;
                    noa.UpdateDt = DateTime.Now;
                    NoticeOfAppropriation.Update(noa);                    
                }
                else
                {
                    noa.CreateBy = user;
                    noa.CreateDt = DateTime.Now;
                    var newNoa=NoticeOfAppropriation.Add(noa);
                }

                return GetNoticeOfAppropriation();
            }

           // return Ok(noaContainer);
        }

        [HttpGet, Authorize, Route("adj/getnoa/{pcc?}")]
        public IHttpActionResult GetNoticeOfAppropriation(string pcc = null) //like pcc but program/file_no/file_ext
        {
            int? id = null;
            var user = User.Identity.Name.Replace("AZWATER0\\", "");

            if (pcc != null)
            {
                Regex regex = new Regex(@"([1-9][0-9])\D?([0-9]{6,7})\D?([0-9]{4})");

                
                pcc = regex.Replace(pcc, "$1-$2.$3");

                if (pcc.Length == 14 || pcc.Length == 15)
                {
                    var pgm = regex.Replace(pcc,"$1");
                    var fno = regex.Replace(pcc,"$2");
                    var fex = regex.Replace(pcc,"$3");

                    return Ok(NoticeOfAppropriationView.PopulateNoaView(pgm, fno, fex));
                }
                else
                {
                    id = int.Parse(pcc);
                }
            }

            var noaCode = id == null ? NoticeOfAppropriationView.PopulateNoaView() : NoticeOfAppropriationView.PopulateNoaView(id);
            return Ok(noaCode);
              
        }


        //--------------------------------------------------------------------------------------------------------
        //---------------------------------- ADD/ DELETE/UPDATE ------------------------------------------------
        //--------------------------------------------------------------------------------------------------------       
        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/addpod/{podobjectid}/{Objtype}/{id}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPod(int podobjectid, string Objtype, int id)
        {
            var adwrpod = POINT_OF_DIVERSION_VIEW.Get(p => p.OBJECTID == podobjectid);
            if (Objtype == "PWR")
            {
                var pwrPodList = await Task.FromResult(PWR_POD.GetList(p => (p.POD_ID ?? -1) == adwrpod.ID && (p.PWR_ID ?? -1) == id));
                if (pwrPodList != null && pwrPodList.Count() > 0)
                {
                    return BadRequest("A relationship already exists for this Place of Use and Point of Diversion");
                }
                if (adwrpod != null)
                {

                    var newPwrPod = PWR_POD.Add(new PWR_POD()
                    {
                        CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                        CREATEDT = DateTime.Now,
                        POD_ID = adwrpod.ID,
                        PWR_ID = id
                    });
                    return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversion(newPwrPod)));
                }
                else
                {
                    return Ok("No POD found in ADWR table");
                }

            }
            else
            {

                var wfrPodList = await Task.FromResult(WFR_POD.Get(p => p.POD_ID == adwrpod.ID && p.WFR_ID == id));

                if (wfrPodList != null)
                {
                    return BadRequest("A relationship already exists for this Place of Use and Point of Diversion");
                }
                var newWfrPod = WFR_POD.Add(new WFR_POD()
                {
                    CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                    CREATEDT = DateTime.Now,
                    POD_ID = adwrpod.ID,
                    WFR_ID = id
                });
                return Ok(await Task.FromResult(POINT_OF_DIVERSION.PointOfDiversionWfr(newWfrPod)));
            }

        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]       
        [HttpPost, Route("adj/updateirr/")]
        public async Task<IHttpActionResult> UpdateIrrigationData([FromBody] List<IrrigationData> irrigation)
        {
            try
            {
                int actions = 0;
                if (irrigation == null)
                {
                    return BadRequest("No data was submitted. Nothing Updated");
                }

                List<IrrigationData> notDelete = new List<IrrigationData>();


                var delete = irrigation.Where(i => i.DeleteRecord == true && i.Id != null).ToList();
                var add = irrigation.Where(i => i.Id == null && i.ProposedWaterRightId != null).ToList();
                var update = irrigation.Where(i => i.DeleteRecord != true && i.Id != null && i.ProposedWaterRightId != null).ToList();

                using (var context = new ADWRContext())
                {
                    if (delete != null && delete.Count() > 0)
                    {
                        var deleteList = delete.Select(d => d.Id).ToArray();
                        var items = context.IrrigationData.Where(i => deleteList.Contains(i.Id));
                        context.IrrigationData.RemoveRange(items);
                    }

                    if (add != null && add.Count() > 0)
                    {
                        context.IrrigationData.AddRange(add);
                        notDelete.AddRange(add);
                    }

                    if (update != null && update.Count() > 0)
                    {
                        foreach (var u in update)
                        {
                            context.IrrigationData.Attach(u);
                            context.Entry(u).State = System.Data.Entity.EntityState.Modified;
                        }

                        notDelete.AddRange(update);
                    }

                    actions = await context.SaveChangesAsync();
                }

                if (actions == 0)
                {
                    return BadRequest("No changes were made");
                }

                return Ok(await Task.FromResult(notDelete));
            }
            catch(Exception exception)
            {
                return BadRequest(string.Format("Error: {0}", QueryResult.BundleExceptions(exception)));
            }
        }

      

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [Route("adj/updateWfr/{useage}/{id}/{wfr_num}")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateWfr(string useage, int id, string wfr_num)
        {
            try
            {
                var user=User.Identity.Name.Replace("AZWATER0\\", "");
                var date = DateTime.Now;

                var wfrSde = await Task.FromResult(WATERSHED_FILE_REPORT_SDE.Get(p => p.WFR_NUM == wfr_num));
                var wfr = await Task.FromResult(WATERSHED_FILE_REPORT.Get(p => p.OBJECTID == wfrSde.OBJECTID));
                var adwrpod = POINT_OF_DIVERSION_VIEW.Get(p => p.OBJECTID == id);
                if (useage == "POD" && adwrpod != null)
                {
                    var pod = await Task.FromResult(WFR_POD.Get(p => p.ID == id));
                    pod.WFR_ID = wfr.ID;
                    pod.POD_ID = adwrpod.ID;
                    pod.UPDATEBY = user;
                    pod.UPDATEDT = date;
                    WFR_POD.Update(pod);
                }
                else if (useage == "POD" && adwrpod == null)
                {
                    return Ok("No POD found in ADWR table");
                }
                else
                {
                    var pwr = await Task.FromResult(PROPOSED_WATER_RIGHT.Get(p => p.ID == id));
                    pwr.WFR_ID = wfr.ID;
                    pwr.UPDATEBY = user;
                    pwr.UPDATEDT = date;
                    PROPOSED_WATER_RIGHT.Update(pwr);
                }
                wfr.WFR_NUM = wfr_num;                
                WATERSHED_FILE_REPORT.Update(wfr);
                return Ok(wfr);

            }
            catch
            {
                return BadRequest("Could not find WFR to assign");
            }



        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpDelete, Route("adj/deletepod/{id}/{fromFeature}")]
        public async Task<IHttpActionResult> DeletePod(int id, string fromFeature) //<== ID IS THE ID FROM THE PWR_POD TABLE
        {
            if (fromFeature == "PWR")
            {

                PWR_POD pod = await Task.FromResult(PWR_POD.Get(p => p.ID == id));

                if (pod == null)
                {
                    return BadRequest("An invalid id was entered");
                }
                PWR_POD.Delete(pod);
                return Ok("Point of Diversion Deleted");
            }
            else
            {
                WFR_POD pod = await Task.FromResult(WFR_POD.Get(p => p.ID == id));
                if (pod == null)
                {
                    return BadRequest("An invalid id was entered");
                }
                WFR_POD.Delete(pod);
                return Ok("Point of Diversion Deleted");
            }
        }


        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/testfileblob/")] //PWR_ID or an error message is returned       
        public async Task<IHttpActionResult> TestFileBlob() //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<HandleForm>(new HandleForm());
            var fileList = await Task.FromResult(TEST_FILE_UPLOAD.UploadFile(provider));

            if (fileList != null)
            {
                return Ok(fileList);
            }

            return BadRequest("Error Uploading File");
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

            if (fileList != null)
            {
                return Ok(TEST_FILE_UPLOAD.FindFile(fileList.ID));
            }

            return BadRequest("Error Uploading File");
        }

        [HttpGet, Route("adj/addfileblob/{id}")] //PWR_ID or an error message is returned       
        public async Task<IHttpActionResult> FindFileBlob(int? id) //<== ID IS THE ID FROM THE EXPLANATION TABLE
        {
            return Ok(await Task.FromResult(TEST_FILE_UPLOAD.FindFile(id)));
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

            if (fileList != null && fileList.STATUS == null)
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

            if (fileRecord == null)
            {
                return BadRequest("The file was not found.");
            }
            if (fileRecord.LOCATION != null)
            {
                if (fileRecord.LOCATION != null && fileRecord.LOCATION.IndexOf(ConfigurationManager.AppSettings["FileUploadLocation"]) == -1)
                {
                    return BadRequest("This file could not be deleted.");
                }

                var fileExists = File.Exists(fileRecord.LOCATION);
                if (!fileExists)
                {
                    return Ok("The file record was deleted but the physical file was not found");
                }

                File.Delete(fileRecord.LOCATION);
            }
            FILE.Delete(fileRecord);
            return Ok("File successfully deleted");
        }

        [Authorize(Roles = "AZWATER0\\PG-APPDEV,AZWATER0\\PG-Adjudications")]
        [HttpPost, Route("adj/addexp/")]
        public async Task<IHttpActionResult> AddExplanation([FromBody] EXPLANATIONS explanation) //Send all form values
        {
            if (!(explanation != null && (explanation.PWR_ID != null || explanation.WFR_ID != null || explanation.POD_ID != null)))
            {
                return BadRequest("An invalid proposed water right ID was entered.");
            }

            var newExplanation = await Task.FromResult(EXPLANATIONS.Add(new EXPLANATIONS()
            {
                CREATEBY = User.Identity.Name.Replace("AZWATER0\\", ""),
                CREATEDT = DateTime.Now,
                PWR_ID = explanation.PWR_ID != null ? explanation.PWR_ID : null,
                WFR_ID = explanation.WFR_ID != null ? explanation.WFR_ID : null,
                POD_ID = explanation.POD_ID != null ? explanation.POD_ID : null,
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
