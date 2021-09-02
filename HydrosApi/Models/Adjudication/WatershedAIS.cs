using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace HydrosApi.Models.Adjudication
{
    [Table("AIS.WATERSHEDS")]
    public class WatershedAIS : AdwrRepository<WatershedAIS>
    {

        [Key, Column("WS_CODE")]
        public string WatershedCode { get; set; }

        [Column("WS_NAME")]
        public string WatershedName { get; set; }


    }
}