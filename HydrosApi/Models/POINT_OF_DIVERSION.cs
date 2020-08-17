namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    //[Table("ADJ.POINT_OF_DIVERSION_VIEW")]
    [Table("ADJ.LLC_PODS_ALL")]
    public partial class POINT_OF_DIVERSION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]

        public POINT_OF_DIVERSION()
        {
            //LAYERS = new HashSet<LAYER>();

        }


        [Key]

        public int OBJECTID { get; set; } //ObjectId

        
        public string DWR_ID { get; set; } //DwrId

        
public string ACTIVE_INACTIVE { get; set; }
        public string BASIN_NAME { get; set; }
        public string BOC { get; set; }
         
        public decimal? CASING_DEP { get; set; }
        public decimal? CASING_DIA { get; set; }
        public string CASING_TYP { get; set; }
        public string CLAIMANT { get; set; }
        public string COMPLETION { get; set; }
        public string COUNTY { get; set; }
        public string DIVERSIO_2 { get; set; }
        public string DIV_COMPLE { get; set; }
        public string LAND_OWNER { get; set; }
        public string INSTALLED { get; set; }
        
        public string POD_NAME { get; set; }

        public string LOC_CAD { get; set; }
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
