using HydrosApi.Data;
 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace HydrosApi.Models.ADWR
{
    [Table("AWS.AW_USERS")]
    public class AwUsers : Repository<AwUsers>//AW_USERS
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }//ID

        [Column("USER_ID")]
        public string UserId { get; set; }//USER_ID

        [Column("CADV_CODE")]
        public string DivisionCode { get; set; }//CADV_CODE

        [Column("EMAIL")]
        public string Email { get; set; }//EMAIL

        [Column("LAST_NAME")]
        public string LastName { get; set; }//LAST_NAME

        [Column("FIRST_NAME")]
        public string FirstName { get; set; }//FIRST_NAME

        [Column("ACTIVE")]
        public string Active { get; set; } = "Y";//ACTIVE

        [NotMapped]
        public string ActiveDirectoryUser { get; set; } //the active directory user
    }
}