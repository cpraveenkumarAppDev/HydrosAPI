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
    public class HYDROS_MANAGER : Repository<HYDROS_MANAGER>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("NAME")]
        [StringLength(50)]
        public string NAME { get; set; }

        [Column("GROUPS")]
        [StringLength(500)]
        public string GROUPS { get; set; }

        [Column("STATUS")]
        [StringLength(20)]
        public string STATUS { get; set; }

        [Column("STATUS_DT")]
        public DateTime? STATUS_DT { get; set; }

        [Column("USERNAME")]
        [StringLength(25)]
        public string USERNAME { get; set; }

    }
}