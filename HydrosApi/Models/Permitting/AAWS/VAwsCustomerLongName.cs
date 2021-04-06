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
    public class VAwsCustomerLongName : Repository<VAwsCustomerLongName>//V_AWS_CUSTOMER_LONG_NAME
    {
        [Column("USER_NAME")]
        [StringLength(30)]
        public string UserName { get; set; }//USER_NAME

        [Column("CUST_ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }//CUST_ID

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string Comments { get; set; }//COMMENTS

        [Column("BAD_ADDRESS_FLAG")]
        [StringLength(1)]
        public string BadAddressFlag { get; set; }//BAD_ADDRESS_FLAG

        [Column("EMAIL")]
        [StringLength(120)]
        public string Email { get; set; }//EMAIL

        [Column("FAX")]
        [StringLength(14)]
        public string Fax { get; set; }//FAX

        [Column("PHONE")]
        [StringLength(20)]
        public string Phone { get; set; }//PHONE

        [Column("ZIP4")]
        [StringLength(4)]
        public string Zip4 { get; set; }//ZIP4

        [Column("ZIP")]
        [StringLength(6)]
        public string Zip { get; set; }//ZIP

        [Column("COUNTRY")]
        [StringLength(20)]
        public string Country { get; set; }//COUNTRY

        [Column("STATE")]
        [StringLength(2)]
        public string State { get; set; }//STATE

        [Column("CITY")]
        [StringLength(20)]
        public string City { get; set; }//CITY

        [Column("ADDRESS2")]
        [StringLength(100)]
        public string Address2 { get; set; }//ADDRESS2

        [Column("ADDRESS1")]
        [StringLength(100)]
        public string Address1 { get; set; }//ADDRESS1

        [Column("COMPANY_LONG_NAME")]
        [StringLength(2090)]
        public string CompanyLongName { get; set; }//COMPANY_LONG_NAME

        [Column("PERSON_TITLE")]
        [StringLength(80)]
        public string PersonTitle { get; set; }//PERSON_TITLE

        [Column("FIRST_NAME")]
        [StringLength(30)]
        public string FirstName { get; set; }//FIRST_NAME

        [Column("LAST_NAME")]
        [StringLength(30)]
        public string LastName { get; set; }//LAST_NAME
    }
}