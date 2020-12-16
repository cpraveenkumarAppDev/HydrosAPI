using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Adjudication
{
    [Table("ADJ.POINT_OF_DIVERSION_VIEW")]
    public class POINT_OF_DIVERSION_VIEW : SdeRepository<POINT_OF_DIVERSION_VIEW>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POINT_OF_DIVERSION_VIEW()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("OBJECTID")]
        public int OBJECTID { get; set; }
        [Column("DWR_ID")]
        public string DWR_ID { get; set; }
        [Column("CLAIMANT")]
        public string CLAIMANT { get; set; }
        [Column("BOC")]
        public string BOC { get; set; }
        [Column("FILE_NO")]
        public string FILE_NO { get; set; }
  
    }
}


//OBJECTID
//CLAIMANT
//OWNER_NAME
//FILE_NO
//BOC
//POD_TYPE
//POD_SOURCE
//POD_NAME
//POD_PWRS
//POD_SHARED
//POD_REMARK
//POD_PARCEL
//INSTALLED
//WELL_DEPTH
//WATER_LEVE
//CASING_DEP
//CASING_DIA
//CASING_TYP
//PUMPRATE
//COMPLETION
//WELL_CANCE
//CADASTRAL
//COUNTY
//WATERSHED
//BASIN_NAME
//SUBBASIN_N
//PUMP_CAPAC
//DIV_COMPLE
//DIVERSIO_2
//PUMPED_VOL
//PV_2015_SO
//PUMPED_V_1
//PV_2016_SO
//PUMPED_V_2
//PV_2017_SO
//PUMPED_V_3
//PV_2018_SO
//PUMPED_V_4
//PV_2019_SO
//DWR_ID
//ACTIVE_INACTIVE
