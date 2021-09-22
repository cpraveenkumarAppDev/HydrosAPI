using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HydrosApi.Models.Adjudication
{
    [Table("ADJ_INV.WATERSHED_VIEW")]
    public class WatershedView : AdwrRepository<WatershedView>
    {

        [Key, Column("WS_CODE", Order = 0)]
        public string WatershedCode { get; set; }

        [Key, Column("SWS_CODE", Order = 1)]
        public int? SubWatershedCode { get; set; }

        [Column("SWS_NAME")]
        public string SubWatershedName { get; set; }

        [Column("WS_NAME")]
        public string WatershedName { get; set; }

        [Column("NOSWS")]
        public bool NoSubWatershed { get; set; }
    }
}
