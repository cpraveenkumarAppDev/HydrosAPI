

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
    public class V_AWS_COUNTY_BASIN : Repository<V_AWS_COUNTY_BASIN>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("COUNTY_NAME")]
        public string COUNTY_NAME { get; set; }
        [Column("BASIN_ABBR")]
        public string BASIN_ABBR { get; set; }
        [Column("BASIN_NAME")]
        public string BASIN_NAME { get; set; }
        [Column("SUBBASIN_ABBR")]
        public string SUBBASIN_ABBR { get; set; }
        [Column("SUBBASIN_NAME")]
        public string SUBBASIN_NAME { get; set; }
    }
}
