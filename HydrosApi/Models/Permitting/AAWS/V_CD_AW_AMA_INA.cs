namespace HydrosApi.Models
{

    using System;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;



    [Table("AWS.V_CD_AW_AMA_INA")]
    public class V_CD_AW_AMA_INA : Repository<V_CD_AW_AMA_INA>
    {
        [Key]
        public string CODE { get; set; }
        public string DESCR { get; set; }
        public string AMA_INA_TYPE { get; set; }

        public string IS_ASSURED { get; set; }

        [NotMapped] 
        public string DescriptionTitleCase
        {
            
            get
            {

                TextInfo textSetting = new CultureInfo("en-US", false).TextInfo;
                return textSetting.ToTitleCase(DESCR);

            }

            set { this.DescriptionTitleCase = value; }
             
        }
    }
}