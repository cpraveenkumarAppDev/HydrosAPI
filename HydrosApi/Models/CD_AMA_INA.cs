using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models
{
    [Table("RGR.CD_AMA_INA")]
    public class CD_AMA_INA : AdwrRepository<CD_AMA_INA>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CD_AMA_INA()
        {
            //LAYERS = new HashSet<LAYER>();
        }

        [Key]
        [StringLength(3)]
        public string CODE { get; set; }

        [StringLength(50)]
        public string DESCR { get; set; }

        [StringLength(3)]
        public string AMA_INA_TYPE { get; set; }
    }
}