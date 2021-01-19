using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Permitting.AAWS
{
    [Table("AWS.V_AWS_CUSTOMER_LONG_NAME")]
    public class V_AWS_CUSTOMER_LONG_NAME : Repository<V_AWS_CUSTOMER_LONG_NAME>
    {
        [Column("USER_NAME")]
        [StringLength(30)]
        public string USER_NAME { get; set; }

        [Column("CUST_ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CUST_ID { get; set; }

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string COMMENTS { get; set; }

        [Column("BAD_ADDRESS_FLAG")]
        [StringLength(1)]
        public string BAD_ADDRESS_FLAG { get; set; }

        [Column("EMAIL")]
        [StringLength(120)]
        public string EMAIL { get; set; }

        [Column("FAX")]
        [StringLength(14)]
        public string FAX { get; set; }

        [Column("PHONE")]
        [StringLength(20)]
        public string PHONE { get; set; }

        [Column("ZIP4")]
        [StringLength(4)]
        public string ZIP4 { get; set; }

        [Column("ZIP")]
        [StringLength(6)]
        public string ZIP { get; set; }

        [Column("COUNTRY")]
        [StringLength(20)]
        public string COUNTRY { get; set; }

        [Column("STATE")]
        [StringLength(2)]
        public string STATE { get; set; }

        [Column("CITY")]
        [StringLength(20)]
        public string CITY { get; set; }

        [Column("ADDRESS2")]
        [StringLength(100)]
        public string ADDRESS2 { get; set; }

        [Column("ADDRESS1"), Required]
        [StringLength(100)]
        public string ADDRESS1 { get; set; }

        [Column("COMPANY_LONG_NAME")]
        [StringLength(2090)]
        public string COMPANY_LONG_NAME { get; set; }

        [Column("PERSON_TITLE")]
        [StringLength(80)]
        public string PERSON_TITLE { get; set; }

        [Column("FIRST_NAME")]
        [StringLength(30)]
        public string FIRST_NAME { get; set; }

        [Column("LAST_NAME")]
        [StringLength(30)]
        public string LAST_NAME { get; set; }

        public bool IsValid()
        {
            var isValid = true;
            if (this.ADDRESS1 == null)
            {
                isValid = false;
            }
            if (this.CITY == null)
            {
                isValid = false;
            }
            if (this.STATE == null)
            {
                isValid = false;
            }
            if(this.COMPANY_LONG_NAME == null && (this.FIRST_NAME == null || this.LAST_NAME == null))
            {
                isValid = false;
            }
            return isValid;
        }
    }
}