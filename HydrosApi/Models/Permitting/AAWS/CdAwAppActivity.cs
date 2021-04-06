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
    public class CdAwAppActivity : Repository<CdAwAppActivity>//CD_AW_APP_ACTIVITY
    {
        [Key]
        [Column("CODE")]
        public string Code { get; set; }//CD_AW_APP_ACTIVITY

        [Column("DESCR")]
        public string Description { get; set; }//DESCR

        [StringLength(1)]
        [Column("ALSO_FILE_STATUS")]
        public string AlsoFileStatus { get; set; }//ALSO_FILE_STATUS

        [StringLength(1)]
        [Column("STOPS_LTF_CLOCK")]
        public string StopsLtfClock { get; set; }//STOPS_LTF_CLOCK

        [StringLength(1)]
        [Column("IS_LETTER_TO_APPLICANT")]
        public string IsLetterToApplicant { get; set; }//IS_LETTER_TO_APPLICANT

        [StringLength(1)]
        [Column("IS_RESPONSE_BY_APPLICANT")]
        public string IsResponseByApplicant { get; set; }//IS_RESPONSE_BY_APPLICANT
    }
}