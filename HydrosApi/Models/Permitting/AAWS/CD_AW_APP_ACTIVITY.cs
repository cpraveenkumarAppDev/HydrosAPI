using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.CD_AW_APP_ACTIVITY")]
    public class CD_AW_APP_ACTIVITY : Repository<CD_AW_APP_ACTIVITY>
    {
        [Key]
        [Column("CODE")]
        public string CODE { get; set; }
        [Column("DESCR")]
        public string DESCR { get; set; }
        [StringLength(1)]
        [Column("ALSO_FILE_STATUS")]
        public string ALSO_FILE_STATUS { get; set; }
        [StringLength(1)]
        [Column("STOPS_LTF_CLOCK")]
        public string STOPS_LTF_CLOCK { get; set; }
        [StringLength(1)]
        [Column("IS_LETTER_TO_APPLICANT")]
        public string IS_LETTER_TO_APPLICANT { get; set; }
        [StringLength(1)]
        [Column("IS_RESPONSE_BY_APPLICANT")]
        public string IS_RESPONSE_BY_APPLICANT { get; set; }
    }
}