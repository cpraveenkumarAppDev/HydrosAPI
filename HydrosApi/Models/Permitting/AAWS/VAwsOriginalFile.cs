namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System;

    [Table("AWS.V_AWS_ORIGINAL_FILE")]
    public class VAwsOriginalFile: Repository<VAwsOriginalFile>
    {
       
        [Key, Column("SEARCH_WRF_ID", Order = 0)]
        public int? SearchWaterRightFacilityId { get; set; }

        [Column("SEARCH_FILE_NO")]
        public string SearchFileNo { get; set; } //the primary pcc in the hydros app

        [Key, Column("ORIGINAL_WRF_ID", Order = 1)]
        public int? OriginalWaterRightFacilityId { get; set; }

        [Column("ORIGINAL_FILE_NO")]
        public string OriginalFileNo { get; set; } //the original pcc from which all subsequent conveyances are derived (if any)

        [Column("ORIGINAL_FILE_DATE")]
        public DateTime? OriginalFileDate { get; set; } //the date of issue for the original pcc 


    }
}