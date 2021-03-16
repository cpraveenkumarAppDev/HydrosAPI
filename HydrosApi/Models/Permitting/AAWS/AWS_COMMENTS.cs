
namespace HydrosApi.Models.Permitting.AAWS
{
    using HydrosApi.Data;
    using HydrosApi.Models.ADWR;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    [Table("AWS.AW_COMMENTS")]
    public class AWS_COMMENTS : Repository<AWS_COMMENTS>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        [Column("WRF_ID")]
        public int WRF_ID { get; set; }
        [Column("AUSR_ID")]
        public int AUSR_ID { get; set; }
        [Column("CMT_DT_TIME")]
        public DateTime CMT_DT_TIME { get; set; }

        [Column("CACM_CODE")]
        public string CACM_CODE { get; set; }

        [Column("CREATEBY")]
        public string CREATEBY { get; set; }

        [Column("COMMENTS")]
        public string COMMENTS { get; set; }
        [NotMapped]
        public AW_USERS USER { get; set; }
        public static List<AWS_COMMENTS> PopulateComments(int id)
        {
            var awsComments = AWS_COMMENTS.GetList(p => p.WRF_ID == id);
            foreach (var comment in awsComments)
            {
                comment.USER = AW_USERS.Get(p => p.ID == comment.AUSR_ID);
            }
            return awsComments;
        }
    }
    internal class CommentObj
    {
        public int WrfId { get; set; }
        public string FileManager { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
    }
}