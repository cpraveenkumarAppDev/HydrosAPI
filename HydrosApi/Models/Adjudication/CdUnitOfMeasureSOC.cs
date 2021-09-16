using HydrosApi.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HydrosApi.Models.Adjudication
{
    [Table("SOC.CD_SOC_UNIT_OF_MEASURE")]
    public class CdUnitOfMeasureSOC : AdwrRepository<CdUnitOfMeasureSOC>
    {

        [Key, Column("CODE")]
        public string Code { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        public static List<CdUnitOfMeasureSOC> NoticeOfAppropriationUnitOfMeasure()
        {
            var uomList = new List<string> { "MI", "AF", "SF", "GAL", "CFS" };
            return GetList(m => uomList.Contains(m.Code));
        }
    }
}