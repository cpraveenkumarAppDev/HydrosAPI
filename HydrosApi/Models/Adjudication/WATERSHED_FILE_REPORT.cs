namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using HydrosApi.Data;

    [Table("ADJ_INV.WATERSHED_FILE_REPORT")]
    public partial class WATERSHED_FILE_REPORT: AdwrRepository<WATERSHED_FILE_REPORT>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? OBJECTID { get; set; }

        [StringLength(100)]
        public string WFR_NUM { get; set; }        
    }
}
