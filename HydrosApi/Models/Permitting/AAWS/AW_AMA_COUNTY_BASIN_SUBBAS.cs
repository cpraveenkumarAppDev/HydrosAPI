
namespace HydrosApi.Models.Permitting.AAWS
{
    using HydrosApi.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("AWS.AW_AMA_COUNTY_BASIN_SUBBAS")]
    public class AW_AMA_COUNTY_BASIN_SUBBAS : Repository<AW_AMA_COUNTY_BASIN_SUBBAS>
    {
        [Key]
        [Column("ID")]
        public int ID { get; set; }
       
        public string AMA { get; set; }
        
        public string CAMA_CODE { get; set; }

        public string AMA_INA_TYPE { get; set; }

        [Column("COUNTY")]
        public string County_Descr { get; set; }

        [Column("COUNTY_CODE")]
        public string County_Code { get; set; }


        public string BASIN_ABBR { get; set; }
         
        public string BASIN_NAME { get; set; }
      
        
        [Column("SUBBASIN_ABBR")]
        public string SubbasinCode { get; set; }

        public string SUBBASIN_NAME { get; set; }       
      
    }
}