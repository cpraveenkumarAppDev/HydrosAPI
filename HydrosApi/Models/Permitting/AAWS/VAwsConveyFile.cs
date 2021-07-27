namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System;


    [Table("AWS.V_AWS_CONVEY_FILE")]
    public class VAwsConveyFile : Repository<VAwsConveyFile>
    {

        [Column("FAKE_ID")]
        public int? FakeId { get; set; }

        [Key, Column("SEARCH_WRF_ID", Order = 0)]
        public int SearchWaterRightFacilityId { get; set; }

        [Column("SEARCH_FILE_NO")]
        public string SearchFileNo { get; set; } //the primary pcc in the hydros app

        [Key, Column("CONVEYING_WRF_ID", Order = 1)]
        public int ConveyingWaterRightFacilityId { get; set; }

        [Column("CONVEYING_FILE_NO")]
        public string ConveyingFileNo { get; set; } 

        [NotMapped]
        public int? DeleteItem { get; set; }
 
    }
}