
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

        [Column("AMA")]
        public string AMA { get; set; }

        [Column("COUNTY")]
        public string COUNTY { get; set; }

        [Column("BASIN_ABBR")]
        public string BASIN_ABBR { get; set; }
        [Column("BASIN_NAME")]
        public string BASIN_NAME { get; set; }
        [Column("SUBBASIN_ABBR")]
        public string SUBBASIN_ABBR { get; set; }
        [Column("SUBBASIN_NAME")]
        public string SUBBASIN_NAME { get; set; }
       
       public static List<dynamic> GetAmaCountyBasinSubbasin()
        {
            dynamic formattedList = new List<dynamic>();
            
            //get only a few fields populated with data for top tabs
            var infoList = AW_AMA_COUNTY_BASIN_SUBBAS.GetAll();
            return infoList.GroupBy(g => new { g.AMA })
                .Select(a => new
                {
                    AMA = a.Key,
                    AMAInfo = a.GroupBy(g => new { g.COUNTY })
                .Select(c => new
                {
                    County = c.Key,
                    Basin = c.GroupBy(g => new { g.BASIN_ABBR, g.BASIN_NAME })
                .Select(i => new { Basin = i.Key, Subbasin = i.Select(s => new { s.SUBBASIN_ABBR, s.SUBBASIN_NAME }).Distinct() })
                }).Distinct()
                }).Distinct().ToList<dynamic>();
        }
    }
}