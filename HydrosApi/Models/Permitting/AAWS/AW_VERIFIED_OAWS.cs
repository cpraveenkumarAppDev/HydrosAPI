using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdwrApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_VERIFIED_OAWS")]
    public class AW_VERIFIED_OAWS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRF_ID")]
        public int WRF_ID { get; set; }

        [Column("APP_COMPLETE")]
        public string APP_COMPLETE { get; set; }

        [Column("APP_ACCEPTED")]
        public string APP_ACCEPTED { get; set; }
    }
}