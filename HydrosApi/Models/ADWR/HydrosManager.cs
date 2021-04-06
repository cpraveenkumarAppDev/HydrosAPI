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
    public class HydrosManager : Repository<HydrosManager>//HYDROS_MANAGER
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//ID

        [Column("NAME")]
        [StringLength(50)]
        public string Name { get; set; }//NAME

        [Column("GROUPS")]
        [StringLength(500)]
        public string Groups { get; set; }//GROUPS

        [Column("STATUS")]
        [StringLength(20)]
        public string Status { get; set; }//STATUS

        [Column("STATUS_DT")]
        public DateTime? StatusDt { get; set; }//STATUS_DT

        [Column("USERNAME")]
        [StringLength(25)]
        public string UserName { get; set; }//USERNAME

    }
}