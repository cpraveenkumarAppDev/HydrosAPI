using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace HydrosApi.Models.Adjudication
{
    [Table("AISPRD.CROPCODE")]
    public partial class CropCode : AdwrRepository<CropCode>
    {

        [Key, Column("CODE")]
        public string Code { get; set; }

        [Column("CROPNAME")]
        public string CropName { get; set; }
        
    }
}