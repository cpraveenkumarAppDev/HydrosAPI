
namespace HydrosApi.Models.Permitting.AAWS
{
    using HydrosApi.Data;
    using HydrosApi.Models.ADWR;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   
    [Table("AWS.AW_COMMENTS")]
    public class AwsComments : Repository<AwsComments>//AWS_COMMENTS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }//ID

        [Column("WRF_ID")]
        public int WaterRightFacilityId { get; set; }//WRF_ID

        [Column("AUSR_ID")]
        public int AwsUserId { get; set; }//AUSR_ID

        [Column("CMT_DT_TIME")]
        public DateTime CommentDate { get; set; }//CMT_DT_TIME

        [Column("CACM_CODE")]
        public string CommentTypeCode { get; set; }//CACM_CODE

        [Column("CREATEBY")]
        public string CreateBy { get; set; }//CREATEBY

        [Column("COMMENTS")]
        public string Comments { get; set; }//COMMENTS

        [NotMapped]
        public AwUsers USER { get; set; }
        public static List<AwsComments> PopulateComments(int id)
        {
            var awsComments = AwsComments.GetList(p => p.WaterRightFacilityId == id);
            foreach (var comment in awsComments)
            {
                comment.USER = AwUsers.Get(p => p.Id == comment.AwsUserId);
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