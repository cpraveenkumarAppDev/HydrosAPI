using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Globalization;
//using System.Text.RegularExpressions;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_LEGAL_AVAILABILITY")]
    public class AwLegalAvailability : Repository<AwLegalAvailability>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("SECTION")]
        [StringLength(2)]
        public string Section { get; set; }

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
        [StringLength(25)]
        public string WaterTypeCode { get; set; }

        [Column("PROVIDER_RECEIVER_ID")]
        public int? ProviderReceiverId { get; set; }

        [Column("AREA_OF_IMPACT")]
        [StringLength(25)]
        public string AreaOfImpact { get; set; }

        [Column("SURFACE_WATER_TYPE")]
        [StringLength(25)]
        public string SurfaceWaterType { get; set; }

        [Column("STORAGE_FACILITY_NAME")]
        public string StorageFacilityName { get; set; }

        [Column("PLEDGED_AMOUNT")]
        public decimal? PledgedAmount { get; set; }

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
                if (ProviderReceiverId == null || Section== null)
                    return null;
                else if (Section == "SW")
                {
                    using (var ctx = new OracleContext())
                    using (var cmd = ctx.Database.Connection.CreateCommand())
                    {
                        ctx.Database.Connection.Open();
                        cmd.CommandText = string.Format("select t.art_program||'-'||t.art_appli_no||'.'||t.art_convy_no pcc " +
                                                        "  from ADWR.SW_APPL_REGRY t where t.art_idno = {0}", ProviderReceiverId);
                        var pcc = cmd.ExecuteScalar();
                        if (pcc != null)
                            return pcc.ToString();
                        else
                            return null;//send error
                    }
                }
                else
                {
                    var wrf = WaterRightFacility.Get(f => f.Id == ProviderReceiverId);
                    if (Section.ToUpper() == "ST" && wrf != null)
                    {
                        StorageFacilityName = wrf.WaterRightFacilityName;
                    }

                    if (wrf == null)
                    {
                        return null;
                    }

                    return wrf.PCC;
                }
            }
            set
            {
                if (Section == "SW")
                {
                    using (var ctx = new OracleContext())
                    using (var cmd = ctx.Database.Connection.CreateCommand())
                    {
                        ctx.Database.Connection.Open();
                        cmd.CommandText = string.Format("select t.art_idno id from ADWR.SW_APPL_REGRY t " +
                                                        " where t.art_program || '-' || t.art_appli_no || '.' || t.art_convy_no = '{0}'", value);
                        var id = cmd.ExecuteScalar();
                        if (id != null)
                            this.ProviderReceiverId = Convert.ToInt32(id);
                        else
                            this.ProviderReceiverId = null;

                    }
                }
                else
                {
                    
                    this.ProviderReceiverId = QueryResult.RgrRptGet(value);

                    if (Section == "ST" && ProviderReceiverId != null)
                    {
                        var wrf = WaterRightFacility.Get(f => f.Id == ProviderReceiverId);
                        if(wrf != null)
                        {
                            StorageFacilityName = wrf.WaterRightFacilityName;
                        }
                    }

                }                    
            }
        }
    }        
      
}