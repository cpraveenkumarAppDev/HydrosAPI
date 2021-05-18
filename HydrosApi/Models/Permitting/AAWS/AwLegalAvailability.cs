﻿using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_LEGAL_AVAILABILITY")]
    public class AwLegalAvailability : Repository<AwLegalAvailability>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }

        [Column("EFF_TYPE")]
        [StringLength(20)]
        public string EffluentType { get; set; }

        [Column("CONTRACT_NAME")]
        [StringLength(50)]
        public string ContractName { get; set; }

        [Column("CONTRACT_NUMBER")]
        [StringLength(25)]
        public string ContractNumber { get; set; }

        [Column("AMOUNT")]
        public decimal? Amount { get; set; }

        [Column("GROUNDWATER_USE_TYPE")]
        [StringLength(20)]
        public string GroundwaterUseType { get; set; }

        [Column("WATER_TYPE_CODE")]
        [StringLength(4)]
        public string WaterTypeCode { get; set; }

        [Column("PROVIDER_RECEIVER_ID")]
        public int? ProviderReceiverId { get; set; }

        [Column("AREA_OF_IMPACT")]
        [StringLength(25)]
        public string AreaOfImpact { get; set; }

        [Column("SURFACE_WATER_TYPE")]
        [StringLength(25)]
        public string SurfaceWaterType { get; set; }

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CreateBy { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

        [NotMapped]
        public string PCC
        {
            get
            {
                if (ProviderReceiverId == null)
                    return null;
                else
                    return WaterRightFacility.Get(f => f.Id == ProviderReceiverId).PCC;
            }
        }

    }
}