namespace HydrosApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;



    [Table("AWS.V_CD_AW_APP_FEE_RATES")]
    public class VCdAwAppFeeRates : Repository<VCdAwAppFeeRates>//V_CD_AW_APP_FEE_RATES
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PROGRAM_CODE", Order = 0)]
        public string ProgramCode { get; set; }//PROGRAM_CODE

        [Column("PROGRAM_DESCR")]
        public string ProgramDescription { get; set; }//PROGRAM_DESCR

        [Key]
        [Column("ASSURED_OR_ADEQUATE_CODE", Order = 1)]
        public string AssuredOrAdequateCode { get; set; }//ASSURED_OR_ADEQUATE_CODE

        [Key]
        [Column("BASIC_FEE", Order = 2)]
        public int BasicFee { get; set; }//BASIC_FEE

        //public int ADD_FEE_RATE { get; set; }       
        //public int SUBSTRACTOR { get; set; }      
        //public int MAX_FEE { get; set; }        
        //public string LOT_AF { get; set; }         
        //public string NO_FEE_AFTER_0906 { get; set; }

        //public string PUB_NOTICE_FEE { get; set; }

    }
}