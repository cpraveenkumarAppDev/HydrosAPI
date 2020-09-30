
namespace HydrosApi.Models {  
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;
 
    //[Table("AWS.V_AWS_GENERAL_INFO")]
    public class AWS_COMMENTS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PCC")]
        public bool PCC { get; set; }

        [Column("CMT_DATE")]
        public DateTime CMT_DATE { get; set; }

        [Column("CMT_TYPE")]
        public string CMT_TYPE { get; set; }

        [Column("CMT_CREATOR")]
        public string CMT_CREATOR { get; set; }

        [Column("COMMENTS")]
        public string COMMENTS { get; set; }
    }
}