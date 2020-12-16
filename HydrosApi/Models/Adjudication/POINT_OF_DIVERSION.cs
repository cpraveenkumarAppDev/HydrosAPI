namespace HydrosApi.Models
{
    using HydrosApi.Models.Adjudication;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;

    //[Table("ADJ.POINT_OF_DIVERSION_VIEW")]
    [Table("ADJ.LLC_PODS_ALL")]
    public partial class POINT_OF_DIVERSION : SdeRepository<POINT_OF_DIVERSION>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
         
               
        public int OBJECTID { get; set; } //ObjectId

        [Key]
        [StringLength(50)]
        
        public string DWR_ID { get; set; } //DwrId

        [StringLength(500)]
        public string APN { get; set; } //Parcel

        [StringLength(100)]
       
        public string BASIN_NAME { get; set; } //Basin

        [StringLength(50)]
        [Column("BOC")]
        public string BOC { get; set; } //Well
        [StringLength(50)]
        [Column("SOC")]
        public string SOC { get; set; } //SOC

        [StringLength(300)]
        public string CLAIMANT{ get; set; }

        [StringLength(100)]
        public string COUNTY { get; set; }

        public DateTime? INSTALLED { get; set; }

        [StringLength(100)]       
        public string LAND_OWNER { get; set; }

        [StringLength(100)]
        public string LOC_CAD { get; set; } //Cadastral Location

        [StringLength(100)]
        [Column("POD_NAME")]
        public string Name { get; set; } //Name of Facility

        [StringLength(100)]
        [Column("POD_OWNER")]
        public string Owner { get; set; }

        [StringLength(700)]
        public string POD_REMARK { get; set; }

        [StringLength(20)]
        public string POD_SEQ { get; set; }

        [StringLength(100)]
        [Column("POD_TYPE")]
        public string Type { get; set; }

        [StringLength(20)]
        public string LCR_REGION { get; set; }

        [StringLength(100)]
        public string LOC_LEGAL { get; set; } //Legal Location

        [StringLength(300)]
        public string PUMPED_VOL { get; set; } //Pumped Vol Current Year - 5

        [StringLength(300)]
        public string PUMPED_V_1 { get; set; } //Pumped Vol Current Year - 4

        [StringLength(300)]
        public string PUMPED_V_2 { get; set; } //Pumped Vol Current Year - 3

        [StringLength(300)]
        public string PUMPED_V_3 { get; set; } //Pumped Vol Current Year - 2

        [StringLength(300)]
        public string PUMPED_V_4 { get; set; } //Pumped Vol Current Year - 1       

        [StringLength(100)]
        public string SHARED_POD { get; set; } //Shared Well/Diversion

        [StringLength(100)]
        [Column("SUBBASIN_N")]
        public string Subbasin { get; set; }

        public Decimal? UTM_X { get; set; }

        public Decimal? UTM_Y { get; set; }

        [StringLength(100)]
        [Column("WATERSHED")]
        public string Watershed { get; set; }

        [StringLength(300)]
        public string WTR_SOURCE { get; set; }

        [StringLength(700)]
        public string XPLANATION { get; set; }

        [NotMapped]
        public int? PWR_POD_ID { get; set; } //This is populated at runtime

        [NotMapped]
        public int? PWR_ID { get; set; } //This is populated at runtime

        [NotMapped]
        private string podTypeDescription;       
     
        [NotMapped]
        public string PodTypeDescription
        {
            get {
                if (Type == null)
                {
                    podTypeDescription = "Unknown Type";
                }
                else
                {
                    switch (Type)
                    {
                        case "D":
                            podTypeDescription = "Surfacewater Diversion";
                            break;
                        case "Instream Pump":
                        case "P":
                            podTypeDescription = "Instream Pump";
                            break;
                        case "S":
                        case "Spring":
                            podTypeDescription = "Spring";
                            break;
                        case "W":
                        case "Well":
                            podTypeDescription = "Well";
                            break;
                        default:
                            podTypeDescription = "Other-"+Type;
                            break;
                    }
                }
                    return podTypeDescription;                
            }
            set
            {
                podTypeDescription = value;
            }             
        }

        ///get the point of diversion with a list of Proposed Water Right/and Point of Diversion pairs (populates the pwr_pod_id)
        public static List<POINT_OF_DIVERSION> PointOfDiversion(List<PWR_POD> pwrPod) 
        {    
            var matchList = pwrPod.Select(i => i.POD_ID ?? -1).Distinct();
            var podList = POINT_OF_DIVERSION.PointOfDiversion(matchList);

            var pod = (from pd in podList
                        join pp in pwrPod on pd.OBJECTID equals pp.POD_ID ?? -1
                        select new
                        {
                            pd,
                            pwrPid = pd.PWR_POD_ID = pp.ID
                        }).Distinct().Select(x => x.pd).ToList();

            return pod;            
        }


        //This is populated at runtime
        ///get the point of diversion with a single Proposed Water Right/and Point of Diversion pair (populates the pwr_pod_id)
        public static POINT_OF_DIVERSION PointOfDiversion(PWR_POD pwrPod) 
        {
            var objectid = pwrPod.POD_ID ?? -1;

            if(objectid > -1)
            { 
                var pod = POINT_OF_DIVERSION.PointOfDiversion(objectid);
                pod.PWR_POD_ID = pwrPod.ID;
                return pod;
            }
            return null;
        }

        ///get the point of diversion using its dwr_id
        public static POINT_OF_DIVERSION PointOfDiversion(string dwrid)
        {
            return POINT_OF_DIVERSION.Get(p => p.DWR_ID == dwrid);             
        }

        ///get the point of diversion using its objectid
        public static POINT_OF_DIVERSION PointOfDiversion(int objectid)
        {           
            return POINT_OF_DIVERSION.Get(p => p.OBJECTID == objectid);             
        }

        ///get all points of diversion
        public static List<POINT_OF_DIVERSION> PointOfDiversion()
        {            
            return POINT_OF_DIVERSION.GetAll();            
        }

        ///get a list of points of diversion with the provided object ids
        public static List<POINT_OF_DIVERSION> PointOfDiversion(IEnumerable<int> objectids)
        {
            return POINT_OF_DIVERSION.GetList(p => objectids.Contains(p.OBJECTID));              
        }

        public static POINT_OF_DIVERSION PointOfDiversionByObjectId(int id)
        {

            try
            {

                var pod = POINT_OF_DIVERSION_VIEW.Get(p => p.OBJECTID == id);
                var podSde = POINT_OF_DIVERSION.Get(p => p.OBJECTID == id);
                pod.Explanations = EXPLANATIONS.GetList(p => p.POD_ID == pod.ID);
                pod.FileList = FILE.GetList(p => p.POD_ID == pod.ID);
                char[] delimiters = new[] { ',', ';' };
                return podSde;
            }
            catch (Exception exception)
            {
                // FileNotFoundExceptions are handled here.
                return POINT_OF_DIVERSION.Get(p => p.OBJECTID == id);
            }

            //wfr.SOC = wfrSde.SOC == null ? null :
            //     (from s in wfrSde.SOC.Split(delimiters)
            //      select new
            //      {
            //          program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
            //          file_no = int.Parse((s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", ""))
            //      }).Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.file_no)).Where(c => c != null).Distinct().ToList();
            //if (wfrSde.BOC != null)
            //{
            //    var bocList = (from p in (from s in wfrSde.BOC.Split(delimiters)
            //                              select new
            //                              {
            //                                  program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
            //                                  file_no = (s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", "")
            //                              })
            //                   select new
            //                   {
            //                       p.program,
            //                       p.file_no,
            //                       numeric_file_no = int.Parse(p.file_no == null ? "0" : p.file_no.ToString()),
            //                       registry_id = p.program + "-" + p.file_no
            //                   }).Distinct();
            //    var wellList = bocList.Where(p => p.program == "55" || p.program == "35");
            //    var swList = bocList.Where(p => p.program != "55" && p.program != "35");

            //    wfr.Well = wellList == null ? null :
            //        wellList.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.file_no && s.PROGRAM == f.program)).Where(c => c != null).ToList();
            //    wfr.Surfacewater = swList == null ? null : swList.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.numeric_file_no)).Where(c => c != null).ToList();
            //};
            //wfr.ProposedWaterRights = PROPOSED_WATER_RIGHT.GetList(p => p.WFR_ID == wfr.ID);
        }
        /* 
        
APN	NVARCHAR2(254)
 
 
CI_INVEST	NVARCHAR2(254)
 
 
CREATED_DATE	TIMESTAMP(6)
CREATED_USER	NVARCHAR2(255)
 
 
DM_INVEST	NVARCHAR2(254)
DWR_ID	NVARCHAR2(25)
FIELD_VISI	NVARCHAR2(254)
 
FINDINGS	NVARCHAR2(254)
GLOBALID	CHAR(38)
 
IR_INVEST	NVARCHAR2(254)
LAND_OWNER	NVARCHAR2(150)
LAST_EDITED_DATE	TIMESTAMP(6)
LAST_EDITED_USER	NVARCHAR2(255)
LCR_REGION	NVARCHAR2(5)
LOCTN_CORR	NVARCHAR2(254)
LOC_CAD	NVARCHAR2(15)
LOC_LEGAL	NVARCHAR2(20)
MU_INVEST	NVARCHAR2(254)
OBJECTID	INTEGER
OT_INVEST	NVARCHAR2(254)
POD_CODE	NVARCHAR2(2)

POD_OWNER	NVARCHAR2(254)
POD_PWRS	NVARCHAR2(254)
POD_REMARK	NVARCHAR2(254)
POD_SEQ	NVARCHAR2(2)
POD_TYPE	NVARCHAR2(254)
PUMPED_VOL	NVARCHAR2(254)
PUMPED_V_1	NVARCHAR2(254)
PUMPED_V_2	NVARCHAR2(254)
PUMPED_V_3	NVARCHAR2(254)
PUMPED_V_4	NVARCHAR2(254)
PUMPRATE	NUMBER(38,8)
PUMP_CAPAC	NVARCHAR2(254)
PV_2015_SO	NVARCHAR2(254)
PV_2016_SO	NVARCHAR2(254)
PV_2017_SO	NVARCHAR2(254)
PV_2018_SO	NVARCHAR2(254)
PV_2019_SO	NVARCHAR2(254)
SHAPE	SDE.ST_GEOMETRY
SHARED_POD	NVARCHAR2(254)
SP_INVEST	NVARCHAR2(255)
SUBBASIN_N	NVARCHAR2(254)
SW_INVEST	NVARCHAR2(254)
UTM_X	NUMBER(38,8)
UTM_Y	NUMBER(38,8)
WATERSHED	NVARCHAR2(254)
WATER_LEVE	NUMBER(38,8)
WELL_CANCE	NVARCHAR2(254)
WELL_DEPTH	NUMBER(38,8)
WTR_SOURCE	NVARCHAR2(254)
XPLANATION	NVARCHAR2(500)*/

    }
}
