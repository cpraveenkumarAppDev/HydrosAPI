using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.WTR_RIGHT_FACILITY")]
    public class WTR_RIGHT_FACILITY : Repository<WTR_RIGHT_FACILITY>
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [Column("CAMA_CODE")]
        public string AmaCode { get; set; }
        [Column("CFST_CODE")]
        public string FileStatusCode { get; set; }
        [Column("PROGRAM_CODE")]
        public string Program { get; set; }
        [Column("CERT_NO")]
        public string Certificate { get; set; }
        [Column("CONV_NO")]
        public string Conveyance { get; set; }

        public string PCC { get => $"{this.Program}-{this.Certificate}.{this.Conveyance}"; }
    }
}