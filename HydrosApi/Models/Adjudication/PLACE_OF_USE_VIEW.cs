namespace HydrosApi 
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Spatial;
    using System.Web.UI.WebControls;
 
    using Microsoft.SqlServer.Types;
    using Models;


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
        public List<POINT_OF_DIVERSION> PointOfDiversion { get; set; }

        [NotMapped]
        public List<SOC_AIS_VIEW> StatementOfClaim { get; set; }

        [NotMapped]
        public List<WELLS_VIEW> Well { get; set; }

        [NotMapped]
        public List<SW_AIS_VIEW> Surfacewater { get; set; }

        [NotMapped]
        public List<EXPLANATIONS> Explanation { get; set; }

        [NotMapped]
        public List<FILE> FileList { get; set; }


        /*public static PLACE_OF_USE_VIEW PlaceOfUseView(string id)
        {
            using (var db = new SDEContext())
            {
                return db.PLACE_OF_USE_VIEW.Where(p => p.DWR_ID == id).FirstOrDefault();
            }
        }

        public static List<PLACE_OF_USE_VIEW> PlaceOfUseView()
        {
            using (var db = new SDEContext())
            {
                return db.PLACE_OF_USE_VIEW.ToList();
            }
        }*/

        /* [NotMapped]
         public PROPOSED_WATER_RIGHT ProposedWaterRight { get; set; }
         [NotMapped]
         public List<PWR_POD> PwrPod { get; set; }
         [NotMapped]
         public POINT_OF_DIVERSION PointOfDiversion { get; set; }*/
        /*

        [NotMapped]
        private IEnumerable<string> socIdList;

        [NotMapped]
        public IEnumerable<string> SocIdList 
        {

            get
            {
                socIdList= (from s in SOC.Split(',')
                        select s);

                return socIdList;
            }

            set
            {
                socIdList = value;

            }            
        }

        [NotMapped]
        public List<SOC_AIS_VIEW> SocAisView { get; set; }
        */

    }
}
