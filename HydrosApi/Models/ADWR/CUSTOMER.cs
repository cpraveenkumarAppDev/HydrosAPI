using HydrosApi.Data;
using HydrosApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.ADWR
{
    [Table("RGR.CUSTOMER")]
    public class CUSTOMER : Repository<CUSTOMER>
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column("LAST_NAME")]
        [StringLength(30)]
        public string LAST_NAME { get; set; }

        [Column("FIRST_NAME")]
        [StringLength(30)]
        public string FIRST_NAME { get; set; }

        [Column("EMAIL")]
        [StringLength(120)]
        public string EMAIL { get; set; }

        [Column("COMPANY")]
        [StringLength(100)]
        public string COMPANY { get; set; }

        [Column("ADDRESS1")]
        [StringLength(100)]
        public string ADDRESS1 { get; set; }

        [Column("ADDRESS2")]
        [StringLength(100)]
        public string ADDRESS2 { get; set; }

        [Column("CITY")]
        [StringLength(20)]
        public string CITY { get; set; }

        [Column("STATE")]
        [StringLength(2)]
        public string STATE { get; set; }

        [Column("COUNTRY")]
        [StringLength(20)]
        public string COUNTRY { get; set; }

        [Column("ZIP")]
        [StringLength(6)]
        public string ZIP { get; set; }

        [Column("ZIP4")]
        [StringLength(4)]
        public string ZIP4 { get; set; }

        [Column("PHONE")]
        [StringLength(20)]
        public string PHONE { get; set; }

        [Column("FAX")]
        [StringLength(14)]
        public string FAX { get; set; }

        [Column("BAD_ADDRESS_FLAG")]
        [StringLength(1)]
        public string BAD_ADDRESS_FLAG { get; set; }

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string COMMENTS { get; set; }

        [Column("PERSON_TITLE")]
        [StringLength(80)]
        public string PERSON_TITLE { get; set; }

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CREATEBY { get; set; }

        [Column("CREATEDT")]
        public DateTime? CREATEDT { get; set; }

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UPDATEBY { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UPDATEDT { get; set; }

        public CUSTOMER()
        {
            //default constructor
        }

        public CUSTOMER(V_AWS_CUSTOMER_LONG_NAME awsCustomer, string userName)
        {
            this.ADDRESS1 = awsCustomer.ADDRESS1;
            this.ADDRESS2 = awsCustomer.ADDRESS2;
            this.BAD_ADDRESS_FLAG = awsCustomer.BAD_ADDRESS_FLAG;
            this.CITY = awsCustomer.CITY;
            this.COMMENTS = awsCustomer.COMMENTS;
            this.COMPANY = awsCustomer.COMPANY_LONG_NAME;
            this.COUNTRY = awsCustomer.COUNTRY;
            this.CREATEBY = userName;
            this.CREATEDT = DateTime.Now; //this is changed by the rgr.customer insert trigger
            this.EMAIL = awsCustomer.EMAIL;
            this.FAX = awsCustomer.FAX;
            this.FIRST_NAME = awsCustomer.FIRST_NAME;
            this.ID = awsCustomer.CUST_ID;
            this.LAST_NAME = awsCustomer.LAST_NAME;
            this.PERSON_TITLE = awsCustomer.PERSON_TITLE;
            this.PHONE = awsCustomer.PHONE;
            this.STATE = awsCustomer.STATE;
            this.ZIP = awsCustomer.ZIP;
            this.ZIP4 = awsCustomer.ZIP4;
        }
    }
}