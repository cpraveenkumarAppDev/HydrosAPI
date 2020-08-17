namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LOGS.LAYERS")]
    public partial class LAYER
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        public int TOP_DBLS { get; set; }

        public int BOT_DBLS { get; set; }

        [StringLength(15)]
        public string LS_CODE { get; set; }

        [StringLength(15)]
        public string PAQ_CODE { get; set; }

        [StringLength(255)]
        public string DESCRIPTION { get; set; }

        public long? LOG_EVENT_ID { get; set; }

        [StringLength(5)]
        public string AT_CODE { get; set; }

        [StringLength(15)]
        public string SAQ_CODE { get; set; }

        [StringLength(15)]
        public string LAQ_CODE { get; set; }

        [StringLength(5)]
        public string HU_CODE { get; set; }

        public decimal? FINES { get; set; }

        public decimal? SAND { get; set; }

        public decimal? GRAVEL { get; set; }

        public decimal? MED_SIZE { get; set; }

        public decimal? MAX_SIZE { get; set; }

        [StringLength(10)]
        public string DRL_CODE { get; set; }

        [StringLength(5)]
        public string USCS_CODE { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(100)]
        public string CREATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(100)]
        public string UPDATEBY { get; set; }

        [StringLength(255)]
        public string COMMENTS { get; set; }

        [StringLength(10)]
        public string TAQ_CODE { get; set; }

        [StringLength(10)]
        public string LITHO_STRAT_TYPE { get; set; }

        [StringLength(1)]
        public string WATER_ENCOUNTERED { get; set; }

        public virtual CD_AQUIFER_TYPE CD_AQUIFER_TYPE { get; set; }

        public virtual CD_DRILLERS_LOG CD_DRILLERS_LOG { get; set; }

        public virtual CD_HYDROGEOLOGIC_UNIT CD_HYDROGEOLOGIC_UNIT { get; set; }

        public virtual CD_LITHO_STRAT CD_LITHO_STRAT { get; set; }

        public virtual CD_LOCAL_AQUIFER CD_LOCAL_AQUIFER { get; set; }

        public virtual CD_PRINCIPAL_AQUIFER CD_PRINCIPAL_AQUIFER { get; set; }

        public virtual CD_SECONDARY_AQUIFER CD_SECONDARY_AQUIFER { get; set; }

        public virtual CD_TERTIARY_AQUIFER CD_TERTIARY_AQUIFER { get; set; }

        public virtual CD_USCS CD_USCS { get; set; }

        public virtual LOG_EVENTS LOG_EVENTS { get; set; }
    }
}
