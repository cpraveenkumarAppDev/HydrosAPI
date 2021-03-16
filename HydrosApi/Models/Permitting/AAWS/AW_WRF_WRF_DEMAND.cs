namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System.Collections.Generic;
    using System.Linq;
    using Models.ADWR;


    [Table("AWS.AW_WRF_WRF_DEMAND")]
    public class AW_WRF_WRF_DEMAND : Repository<AW_WRF_WRF_DEMAND>
    {       

         
        [Key,Column("WRF_ID_TO")]
        public int WRF_ID_TO { get; set; }
        
        [Column("WRF_ID_FROM")]
        public int WRF_ID_FROM { get; set; }

        [Column("WTR_DEMAND")]
        public decimal? WTR_DEMAND { get; set; }

        [NotMapped, StringLength(14)]
        public string ASSOCIATED_PCC { get; set; }

        [NotMapped, StringLength(20)]
        public string AVAILABILITY_TYPE { get; set; }


    }
}