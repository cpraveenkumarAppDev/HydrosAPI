using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_AMA")]
    public class V_AWS_AMA
    {
        [Key, Column("WRFID", Order = 1)]
        public int? WRFID { get; set; }

        [Key, Column("PCC", Order = 2), StringLength(4000)]
        public string PCC { get; set; }

        [Column("PROVIDER_SIZE"), StringLength(50)]
        public string PROVIDER_SIZE { get; set; }

        [Column("GWRIGHTS"), StringLength(20)]
        public string GWRIGHTS { get; set; }

        [Column("SWRIGHTS"), StringLength(20)]
        public string SWRIGHTS { get; set; }

        [Column("EXTINGUISH"), StringLength(20)]
        public string EXTINGUISH { get; set; }

        [Column("DRYLOT"), StringLength(5)]
        public string DRYLOT { get; set; }

        [Column("CCN"), StringLength(5)]
        public string CCN { get; set; }

        [Column("PLAT"), StringLength(5)]
        public string PLAT { get; set; }

        [Column("EXEMPTCREDITS"), StringLength(5)]
        public string EXEMPTCREDITS { get; set; }

        [Column("POORQUALITY_EXEMPT"), StringLength(5)]
        public string POORQUALITY_EXEMPT { get; set; }

        [Column("NOIREFILE"), StringLength(5)]
        public string NOIREFILE { get; set; }

        [Column("PROVTARGET"), StringLength(50)]
        public string PROVTARGET { get; set; }

        [Column("PROVCOMPLY"), StringLength(5)]
        public string PROVCOMPLY { get; set; }

        [Column("ADMINREVIEW"), StringLength(5)]
        public string ADMINREVIEW { get; set; }

        [Column("BASICGWALLOW"), StringLength(50)]
        public string BASICGWALLOW { get; set; }

        [Column("EXTCREDITS"), StringLength(50)]
        public string EXTCREDITS { get; set; }

        [Column("STOREDCREDITS"), StringLength(50)]
        public string STOREDCREDITS { get; set; }

        [Column("TOTALCREDITS"), StringLength(50)]
        public string TOTALCREDITS { get; set; }

        [Column("TOTALRESDEMAND"), StringLength(50)]
        public string TOTALRESDEMAND { get; set; }

        [Column("RES_USE_GPCD")]
        public int? RES_USE_GPCD { get; set; }

        [Column("TOTALNONRES"), StringLength(50)]
        public string TOTALNONRES { get; set; }

        [Column("CONSTRUCDEMAND"), StringLength(50)]
        public string CONSTRUCDEMAND { get; set; }

        [Column("LUDEMAND"), StringLength(50)]
        public string LUDEMAND { get; set; }

        [Column("TOTAL_NON_RES_DEMAND")]
        public int? TOTAL_NON_RES_DEMAND { get; set; }

        [Column("TOTALDEMAND"), StringLength(50)]
        public string TOTALDEMAND { get; set; }

        [Column("TOTAL_DEMAND_GPCD")]
        public decimal? TOTAL_DEMAND_GPCD { get; set; }

        [Column("GWDEMAND"), StringLength(50)]
        public string GWDEMAND { get; set; }

        [Column("EFFLUENT"), StringLength(50)]
        public string EFFLUENT { get; set; }

        [Column("SURFACEWATER"), StringLength(50)]
        public string SURFACEWATER { get; set; }

        [Column("CAPWATER"), StringLength(50)]
        public string CAPWATER { get; set; }

        [Column("COLORADORIVERWATER"), StringLength(50)]
        public string COLORADORIVERWATER { get; set; }

        [Column("CENYRDEMAND"), StringLength(50)]
        public string CENYRDEMAND { get; set; }

        [Column("APPLICANT_ESTIMATE"), StringLength(50)]
        public string APPLICANT_ESTIMATE { get; set; }

        [Column("DISTRIBUTION_LOSS_TOTAL")]
        public int? DISTRIBUTION_LOSS_TOTAL { get; set; }

        [Column("DISTRIBUTION_LOSS_FACTOR")]
        public int? DISTRIBUTION_LOSS_FACTOR { get; set; }

        [Column("CONSTRUCTION_NUM_LOTS")]
        public int? CONSTRUCTION_NUM_LOTS { get; set; }

        [Column("CONSTRUCTION_DEMAND")]
        public int? CONSTRUCTION_DEMAND { get; set; }

        [Column("CONSTRUCTION_DEMAND_100YR")]
        public int? CONSTRUCTION_DEMAND_100YR { get; set; }

        [Column("CURRENT_DEMAND")]
        public int? CURRENT_DEMAND { get; set; }

        [Column("CURRENT_DEMAND_YEAR")]
        public int? CURRENT_DEMAND_YEAR { get; set; }

        [Column("COMMITTED_DEMAND")]
        public int? COMMITTED_DEMAND { get; set; }

        [Column("PROJECTED_DEMAND")]
        public int? PROJECTED_DEMAND { get; set; }

        [Column("PROJECTED_DEMAND_YEAR")]
        public int? PROJECTED_DEMAND_YEAR { get; set; }

        [Column("DEMAND_ASSIGNED_OR_RETAINED"), StringLength(1)]
        public string DEMAND_ASSIGNED_OR_RETAINED { get; set; }

        [Column("DEDICATION_LTSC"), StringLength(1)]
        public string DEDICATION_LTSC { get; set; }


    }
}