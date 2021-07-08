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
    [Table("ADWR_ADMIN.HYDROS_MANAGER")]
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
    }
}
