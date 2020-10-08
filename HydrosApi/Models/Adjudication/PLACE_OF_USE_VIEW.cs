namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;    
    using System.Linq;    
    using System.Web.UI.WebControls;
    using System.Web;    
   
    [Table("ADJ.PLACE_OF_USE_VIEW")]
    public partial class PLACE_OF_USE_VIEW :SdeRepository<PLACE_OF_USE_VIEW>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PLACE_OF_USE_VIEW()
        {             
        }

        [Key]
        
        public string DWR_ID { get; set; } //Formerly DwrId
        
        public string POU_NAME { get; set; } //formerly Name         
        public string POU_CODE { get; set; } //formerly Code
       
        public int? SW { get; set; } //formerly SubWatershed
        
        public string WS { get; set; } //formerly Watershed
        
        public string LCR_REGION { get; set; } //formerly Region
         
        public string POU_USE { get; set; } //formerly Use
        
        public decimal? ACRES { get; set; }
        public string ACTIVE { get; set; }
        public string APN { get; set; }
        public decimal? AREA_AC { get; set; }
        public string BAS_OF_CLM { get; set; }
        public decimal? CC_1 { get; set; }
        public decimal? CC_2 { get; set; }
        public decimal? CC_3 { get; set; }
        public string CLAIMANT { get; set; }
        public string CONTACT_ADDRESS { get; set; }
        public string CONTACT_CITY { get; set; }
        public string CONTACT_EMAIL { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string CONTACT_STATE { get; set; }
        public string CONTACT_ZIP { get; set; }
        public decimal? CONVEY_EFFICIENCY { get; set; }
        public string COUNTY { get; set; }
        public string CROP_TYPE1 { get; set; }
        public string CROP_TYPE2 { get; set; }
        public string CROP_TYPE3 { get; set; }
        public decimal? CU_1 { get; set; }
        public decimal? CU_2 { get; set; }
        public decimal? CU_3 { get; set; }
        public decimal? DWELLINGS { get; set; }
        public decimal? EST_VOL { get; set; }
        public decimal? FIELD_ACRE { get; set; }
        public decimal? FIELD_CAP { get; set; }
        public string FIELD_CHK { get; set; }
        public string FINDINGS { get; set; }
        public string FIRST_USE { get; set; }
        public string GLOBALID { get; set; }
        public string INSIDE_CB { get; set; }
        public string INSIDE_ND { get; set; }
        public string IRRIGATION_SYSTEM { get; set; }
        public decimal? IR_EFFICIENCY { get; set; }
        public string LAND_OWNER { get; set; }        
        public string LESSEE { get; set; }
        public string LOC_CAD { get; set; }
        public string LOC_LEGAL { get; set; }
        //public int? OBJECTID { get; set; }
        public decimal? PEOPLE { get; set; }
        public string PHOTO_DT { get; set; }
        public string PODS { get; set; }
       
        public decimal? POU_SEQ { get; set; }
        public string POU_STATUS { get; set; }
        public string PRIORTY_DT { get; set; }
        public string QUANTITY { get; set; }
        public string REMARKS { get; set; }
        public string RGHT_OWNER { get; set; }
        public string SHARED_POD { get; set; }
        public string SOC { get; set; }
        public string VERIFIED { get; set; }
        public decimal? WATER_DUTY { get; set; }
        public string WILDLIFE { get; set; }

        public string WTR_SOURCE { get; set; }
        public string XPLANATION { get; set; }

        [NotMapped]
        public int? PWR_ID { get; set; }

        [NotMapped]
        public string PWR_COMMENTS { get; set; }

      
        [NotMapped]
        public PROPOSED_WATER_RIGHT ProposedWaterRight { get; set; }

        [NotMapped]
        public List<POINT_OF_DIVERSION> PointOfDiversion { get; set;}

        [NotMapped]
        public List<SOC_AIS_VIEW> StatementOfClaim{get; set;}        

        [NotMapped]
        public List<WELLS_VIEW> Well { get; set; }

        [NotMapped]
        public List<SW_AIS_VIEW> Surfacewater { get; set; }

        [NotMapped]
        public List<EXPLANATIONS> Explanation { get; set; }

        [NotMapped]
        public List<FILE> FileList { get; set; }

        //Returns EVERYTHING needed to populate the form
        //removed from controller
        public static List<PLACE_OF_USE_VIEW> PlaceOfUseView(string id)
        {
            var pouList = new List<PLACE_OF_USE_VIEW>();
            var pou= PLACE_OF_USE_VIEW.Get(p => p.DWR_ID == id);

            pou.StatementOfClaim = pou.SOC == null ? null :
            (from s in pou.SOC.Split(',')
             select new
             {
                 program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                 file_no = int.Parse((s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", ""))
             }).Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.file_no)).Distinct().ToList();

            if(pou.BAS_OF_CLM != null)
            {
                var bocList = (from p in (from s in pou.BAS_OF_CLM.Split(',')
                                         select new
                                         {
                                             program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                                             file_no = (s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", "")
                                         })
                              select new
                              {
                                  p.program,
                                  p.file_no,
                                  numeric_file_no=int.Parse(p.file_no==null ? "0" : p.file_no.ToString()),
                                  registry_id = p.program + "-" + p.file_no
                              }).Distinct();

                var wellList = bocList.Where(p => p.program == "55" || p.program=="35");                
                var swList = bocList.Where(p => p.program != "55" && p.program != "35");

                pou.Well = wellList == null ? null :
                    wellList.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.file_no && s.PROGRAM==f.program)).ToList();

                pou.Surfacewater = swList==null ? null : swList.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.numeric_file_no)).ToList();
            }

            pou.ProposedWaterRight = PROPOSED_WATER_RIGHT.Get(p => p.POU_ID == pou.DWR_ID);

            var pwr = pou.ProposedWaterRight;

            if(pwr==null)
            {
                var user = HttpContext.Current.User.Identity.Name;
                pwr=PROPOSED_WATER_RIGHT.Add(new PROPOSED_WATER_RIGHT()
                {
                    CREATEBY = user.Replace("AZWATER0\\", ""),
                    CREATEDT = DateTime.Now,
                    POU_ID = pou.DWR_ID
                });

                pou.PWR_ID = pwr.ID;
                pou.ProposedWaterRight = pwr;

            }
            else
            {
                pou.PWR_ID = pwr.ID;
                   
                pou.ProposedWaterRight = pwr;
                pou.PointOfDiversion = PWR_POD.GetList(p => p.PWR_ID == pwr.ID).Select(p => p.PointOfDiversion).Distinct().ToList();
                pou.FileList = FILE.GetList(f => f.PWR_ID == pwr.ID);
                pou.Explanation = EXPLANATIONS.GetList(i => i.PWR_ID == pwr.ID);
            }

            pouList.Add(pou);

            return pouList;
        }        
    }
}
