
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
    public class AwAmaCountyBasinSubbasin : Repository<AwAmaCountyBasinSubbasin>//AW_AMA_COUNTY_BASIN_SUBBAS
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }//ID

        [Column("AMA")]
        public string AMA { get; set; }//Could not update

        [Column("CAMA_CODE")]
        public string Cama_code { get; set; }//Could not update

        public string AMA_INA_TYPE { get; set; }//Could not update

        [Column("COUNTY")]
        public string County_Descr { get; set; }//Could not update

        [Column("COUNTY_CODE")]
        public string County_Code { get; set; }//Could not update

        [Column("BASIN_ABBR")]
        public string BasinCode { get; set; }

        [Column("BASIN_NAME")]
        public string BasinName { get; set; }
        
        [Column("SUBBASIN_ABBR")]
        public string SubbasinCode { get; set; }

        [Column("SUBBASIN_NAME")]
        public string SubbasinName { get; set; }       
      
    }
}