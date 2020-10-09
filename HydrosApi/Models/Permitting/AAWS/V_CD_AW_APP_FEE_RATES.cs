﻿namespace HydrosApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;



    [Table("AWS.V_CD_AW_APP_FEE_RATES")]
    public class V_CD_AW_APP_FEE_RATES : Repository<V_CD_AW_APP_FEE_RATES>
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROGRAM_CODE", Order =0)]
        public string PROGRAM_CODE { get; set; }

        [Column("PROGRAM_DESCR")]
        public string PROGRAM_DESCR { get; set; }
        [Key]
        [Column("ASSURED_OR_ADEQUATE_CODE", Order =1)]
        public string ASSURED_OR_ADEQUATE_CODE { get; set; }
        [Key]
        [Column("BASIC_FEE", Order = 2)]
        public int BASIC_FEE { get; set; }

        [NotMapped]
        public string ASSURED_OR_ADEQUATE_DESCR { get {
                string fulldescription = "";
                switch (this.ASSURED_OR_ADEQUATE_CODE.ToUpper())
                {
                    case "S": fulldescription = "Assured"; break;
                    case "Q": fulldescription = "Adequate"; break;
                    case "B": fulldescription = "Assured and Adequate"; break;
                    default: fulldescription = "error"; break;
                }
                return fulldescription;
            } set { this.ASSURED_OR_ADEQUATE_DESCR = value; } 
        }
        //public int ADD_FEE_RATE { get; set; }       
        //public int SUBSTRACTOR { get; set; }      
        //public int MAX_FEE { get; set; }        
        //public string LOT_AF { get; set; }         
        //public string NO_FEE_AFTER_0906 { get; set; }

        //public string PUB_NOTICE_FEE { get; set; }

    }
}