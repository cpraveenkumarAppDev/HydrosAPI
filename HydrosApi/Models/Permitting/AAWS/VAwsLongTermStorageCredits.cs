using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_LTSC")]
    public class VAwsLongTermStorageCredits : Repository<VAwsLongTermStorageCredits>
    {
        [Key, Column("WRF_ID", Order = 0)]
        public int WaterRightFacilityId { get; set; }

        [Key, Column("LTSA", Order = 1)]
        [StringLength(20)]
        public string LongTermStorageAccount { get; set; }

        [Column("AMOUNT")]
        public int? Amount { get; set; }
    }
}