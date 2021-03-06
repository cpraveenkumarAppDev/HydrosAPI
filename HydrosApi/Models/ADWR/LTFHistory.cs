using HydrosApi.Data;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("ADWR_ADMIN.LTF_HISTORY")]
    public class LTFHistory : Repository<LTFHistory>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("APP_ID")]
        public int? AppId { get; set; }

        [Column("ACT_ID")]
        public int? ActId { get; set; }

        [Column("USER_ID")]
        [StringLength(30)]
        public string UserId { get; set; }

        [Column("APPLIED_DATE")]
        public DateTime? AppliedDate { get; set; }

        [Column("CLOCK")]
        public int? Clock { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("CREATE_BY")]
        [StringLength(30)]
        public string CreateBy { get; set; }

        [Column("COMMENTS")]
        [StringLength(30)]
        public string Comments { get; set; }

        [NotMapped]
        public string ActionDescription
        {
            get
            {
                using (var ctx = new OracleContext())
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    ctx.Database.Connection.Open();
                    cmd.CommandText = string.Format("select t.description " +
                                                    "  from ADWR_ADMIN.LTF_CD_ACTION t where t.id = {0}", ActId);
                    var ActDescription = cmd.ExecuteScalar();
                    if (ActDescription != null)
                        return ActDescription.ToString();
                    else
                        return null;//send error
                }
            }
        }

        [NotMapped]
        public string PeriodDescription
        {
            get
            {
                using (var ctx = new OracleContext())
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    ctx.Database.Connection.Open();
                    cmd.CommandText = string.Format("select p.description from ltf_cd_action a, adwr_admin.ltf_period p " +
                                                    "  where a.ltf_period_id = p.id and a.id = {0}", ActId);
                    var PeriodDescr = cmd.ExecuteScalar();
                    if (PeriodDescr != null)
                        return PeriodDescr.ToString();
                    else
                        return null;//send error
                }
            }
        }
    }
}
