using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HydrosApi.Models.Adjudication
{
    [Table("AIS.SUBWATERSHEDS")]
    public class SubWatershedAIS : AdwrRepository<SubWatershedAIS>
    {

        [Key, Column("SWS_WS_CODE", Order = 0)]
        public string WatershedCode { get; set; }

        [Key, Column("SWS_SUBWATERSHED_CODE", Order = 1)]
        public int? SubWatershedCode { get; set; }

        [Column("SWS_NAME")]
        public string SubWatershedName { get; set; }

        [NotMapped]
        public string WatershedName
        {
            get {
                var watershed = WatershedAIS.Get(w => w.WatershedCode == WatershedCode);                 
                return watershed != null ? watershed.WatershedName : null;
            }

            set => WatershedName = value;             
        }


        
    }
}