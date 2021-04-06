using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_CUSTOMER")]
    public class VAwsCustomer : Repository<VAwsCustomer>//V_AWS_CUSTOMER
    {

        [Key, Column("CUST_ID", Order = 1)]
        public int CustomerId { get; set; }//CUST_ID

        [Key, Column("WRF_ID", Order = 0)]
        public int WaterRightFacilityId { get; set; }//WRF_ID

    }
}