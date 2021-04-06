

namespace HydrosApi.Models.Permitting.AAWS
{
    using HydrosApi.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("AWS.V_AWS_COUNTY_BASIN")]
    public class VAwsCountyBasin : Repository<VAwsCountyBasin>//V_AWS_COUNTY_BASIN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COUNTY")]
        public string CountyName { get; set; }//COUNTY_NAME

        [Column("AMA_CODE")]
        public string AmaCode { get; set; }//AMA_CODE

        [Column("BASIN_ABBR")]
        public string BasinAbbr { get; set; }//BASIN_ABBR

        [Column("BASIN_NAME")]
        public string BasinName { get; set; }//BASIN_NAME

        [Column("SUBBASIN_ABBR")]
        public string SubbasinAbbr { get; set; }//SUBBASIN_ABBR

        [Column("SUBBASIN_NAME")]
        public string SubbasinName { get; set; }//SUBBASIN_NAME
    }
}
