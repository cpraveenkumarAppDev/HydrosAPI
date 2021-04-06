using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_WELL_SERVING")]
    public class AwWellServing : Repository<AwWellServing>//AW_WELL_SERVING
    {
        [Key, Column("WRF_ID", Order = 0)]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Key, Column("WELL_REGISTRY_ID", Order = 1)]
        [StringLength(6)]
        public string WellRegistryId { get; set; }//WELL_REGISTRY_ID

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }//UPDATEBY

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT
    }
}