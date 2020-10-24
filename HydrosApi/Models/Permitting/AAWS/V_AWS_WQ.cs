namespace HydrosApi.Models
{
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;


    [Table("AWS.V_AWS_WQ")]
    public class V_AWS_WQ : Repository<V_AWS_WQ>
    {
        [Key]
        public int WRFID { get; set; }
        public string PCC { get; set; }
        public string CAPW_PWSID { get; set; }
        public string NEW_PROVIDER { get; set; }
        public string SAFE_WTR { get; set; }
        public string CONTAMINATION { get; set; }
        public string WQ_CHANGES { get; set; }
        public string WQ_NOT_PROVEN { get; set; }
        public string MEET_DRINK_WTR_STD { get; set; }
        public string MTGN_MGRT_ANALYSIS { get; set; }
       

    }
}