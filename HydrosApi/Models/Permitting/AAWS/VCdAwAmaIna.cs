namespace HydrosApi.Models
{
    using System;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;

    [Table("AWS.V_CD_AW_AMA_INA")]
    public class VCdAwAmaIna : Repository<VCdAwAmaIna>//V_CD_AW_AMA_INA
    {
        [Key]
        [Column("CODE")]
        public string Code { get; set; }//CODE

        [Column("DESCR")]
        public string Description { get; set; }//DESCR

        [Column("AMA_INA_TYPE")]
        public string AmaInaType { get; set; }//AMA_INA_TYPE

        [Column("IS_ASSURED")]
        public string IsAssured { get; set; }//IS_ASSURED

        [NotMapped] 
        public string DescriptionTitleCase
        {
            
            get
            {

                TextInfo textSetting = new CultureInfo("en-US", false).TextInfo;
                return textSetting.ToTitleCase(Description);

            }

            set { this.DescriptionTitleCase = value; }
             
        }
    }
}