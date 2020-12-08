using HydrosApi.Data;
 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace HydrosApi.Models.ADWR
{
    [Table("AWS.AW_USERS")]
    public class AW_USERS : Repository<AW_USERS>
    {
        [Key]
        public int ID { get; set; }

        public string USER_ID { get; set; }

        public string CADV_CODE { get; set; }

        public string EMAIL { get; set; }

        public string LAST_NAME { get; set; }

        public string FIRST_NAME { get; set; }

        [NotMapped]
        public string ADUser { get; set; } //the active directory user
    }
}