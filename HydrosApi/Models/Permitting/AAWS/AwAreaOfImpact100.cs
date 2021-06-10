namespace HydrosApi.Models.Permitting.AAWS
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data;
    using System;
    


    [Table("AWS.AW_AREA_OF_IMPACT_100")]
public class AwAreaOfImpact100 : Repository<AwAreaOfImpact100>//AW_WRF_WRF_DEMAND
{

        [Key, Column("WRF_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? WaterRightFacilityId { get; set; }

        [Column("GROUNDWATER"),StringLength(30)]

    public string GroundwaterAreaOfImpact { get; set; }

        [Column("EFFLUENT"), StringLength(30)]

        public string EffluentAreaOfImpact { get; set; }

        [Column("SURFACEWATER"), StringLength(30)]

        public string SurfaceWaterAreaOfImpact { get; set; }

        [Column("CAP"), StringLength(30)]

        public string CAPAreaOfImpact { get; set; }


        [Column("COLORADORIVER"), StringLength(30)]

        public string ColoradoRiverAreaOfImpact { get; set; }

        [Column("CREATEBY"), StringLength(30)]
    public string CreateBy { get; set; }//CREATEBY

    [Column("CREATEDT")]
    public DateTime? CreateDt { get; set; }//CREATEDT

    [Column("UPDATEBY"), StringLength(30)]
    public string UpdateBy { get; set; }//UPDATEBY

    [Column("UPDATEDT")]
    public DateTime? UpdateDt { get; set; }//UPDATEDT
 
}
}