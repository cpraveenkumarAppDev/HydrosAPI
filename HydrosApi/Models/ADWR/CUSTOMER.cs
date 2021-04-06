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
    public class Customer : Repository<Customer>//CUSTOMER
    {
        [Column("ID")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }//ID

        [Column("LAST_NAME")]
        [StringLength(30)]
        public string LastName { get; set; }//LAST_NAME

        [Column("FIRST_NAME")]
        [StringLength(30)]
        public string FirstName { get; set; }//FIRST_NAME

        [Column("EMAIL")]
        [StringLength(120)]
        public string Email { get; set; }//EMAIL

        [Column("COMPANY")]
        [StringLength(100)]
        public string Company { get; set; }//COMPANY

        [Column("ADDRESS1")]
        [StringLength(100)]
        public string Address1 { get; set; }//ADDRESS1

        [Column("ADDRESS2")]
        [StringLength(100)]
        public string Address2 { get; set; }//ADDRESS2

        [Column("CITY")]
        [StringLength(20)]
        public string City { get; set; }//CITY

        [Column("STATE")]
        [StringLength(2)]
        public string State { get; set; }//STATE

        [Column("COUNTRY")]
        [StringLength(20)]
        public string Country { get; set; }//COUNTRY

        [Column("ZIP")]
        [StringLength(6)]
        public string ZIP { get; set; }

        [Column("ZIP4")]
        [StringLength(4)]
        public string Zip4 { get; set; }//ZIP4

        [Column("PHONE")]
        [StringLength(20)]
        public string Phone { get; set; }//PHONE

        [Column("FAX")]
        [StringLength(14)]
        public string Fax { get; set; }//FAX

        [Column("BAD_ADDRESS_FLAG")]
        [StringLength(1)]
        public string BadAddressFlag { get; set; }//BAD_ADDRESS_FLAG

        [Column("COMMENTS")]
        [StringLength(1990)]
        public string Comments { get; set; }//COMMENTS

        [Column("PERSON_TITLE")]
        [StringLength(80)]
        public string PersonTitle { get; set; }//PERSON_TITLE

        [Column("CREATEBY")]
        [StringLength(30)]
        public string CreateBy { get; set; }//CREATEBY

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }//CREATEDT

        [Column("UPDATEBY")]
        [StringLength(30)]
        public string UpdateBy { get; set; }//UPDATEBY

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }//UPDATEDT

        public Customer()
        {
            //default constructor
        }

        public Customer(VAwsCustomerLongName awsCustomer, string userName)
        {
            this.Address1 = awsCustomer.Address1;
            this.Address2 = awsCustomer.Address2;
            this.BadAddressFlag = awsCustomer.BadAddressFlag;
            this.City = awsCustomer.City;
            this.Comments = awsCustomer.Comments;
            this.Company = awsCustomer.CompanyLongName;
            this.Country = awsCustomer.Country;
            this.CreateBy = userName;
            this.CreateDt = DateTime.Now; //this is changed by the rgr.customer insert trigger
            this.Email = awsCustomer.Email;
            this.Fax = awsCustomer.Fax;
            this.FirstName = awsCustomer.FirstName;
            this.Id = awsCustomer.CustomerId;
            this.LastName = awsCustomer.LastName;
            this.PersonTitle = awsCustomer.PersonTitle;
            this.Phone = awsCustomer.Phone;
            this.State = awsCustomer.State;
            this.ZIP = awsCustomer.Zip;
            this.Zip4 = awsCustomer.Zip4;
        }
    }
}