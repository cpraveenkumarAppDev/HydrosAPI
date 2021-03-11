using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_CUSTOMER")]
    public class V_AWS_CUSTOMER : Repository<V_AWS_CUSTOMER>
    {

        [Key, Column("CUST_ID", Order = 1)]
        public int CUST_ID { get; set; }

        [Key, Column("WRF_ID", Order = 0)]
        public int WRF_ID { get; set; }

     
    }
}