namespace HydrosApi.ViewModel.Permitting.AAWS
{
    using HydrosApi.Data;
    using Models;
    using Models.ADWR;
    using Models.Permitting.AAWS;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using System.Linq;

    public class CommentsViewModel
    {
        public int WaterRightFacilityId { get; set; }
        public string FileManager { get; set; }
        public string CommentDate { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }

        public static List<CommentsViewModel> AwsGetComments(int id)
        {
            List<CommentsViewModel> commentsList = new List<CommentsViewModel>();
            var awsComments = AwsComments.GetList(p => p.WaterRightFacilityId == id).OrderByDescending(t => t.CommentDate);
            foreach (var comments in awsComments)
            {
                var user = AwUsers.Get(p => p.Id == comments.AwsUserId);
                var commentTypes = CdAwCommentType.Get(p => p.Code == comments.CommentTypeCode);
                var commentObj = new CommentsViewModel();
                commentObj.WaterRightFacilityId = comments.WaterRightFacilityId;
                commentObj.CommentDate = comments.CommentDate.ToString("g", DateTimeFormatInfo.InvariantInfo);
                commentObj.Code = commentTypes.Code;
                commentObj.Description = commentTypes.Description;
                commentObj.FileManager = user.FirstName + " " + user.LastName;
                commentObj.Comment = comments.Comments;
                commentsList.Add(commentObj);
            };

            return commentsList;
        }
        public static CommentsViewModel AddAWSComment(CommentsViewModel comment, string userName)
        {

            using (var context = new OracleContext())
            {
                var awsComment = new AwsComments();
                //var selfList = new CommentsViewModel().GetType().GetProperties().ToList();
                //foreach (var prop in selfList)
                //{
                //    var tempVal = prop.GetValue(comment);

                //}
                var user = new GetBestUsername(userName);
                awsComment.AwsUserId = user.Id;
                awsComment.WaterRightFacilityId = comment.WaterRightFacilityId;
                awsComment.CommentDate = DateTime.Now;
                awsComment.Comments = comment.Comment;
                awsComment.CommentTypeCode = comment.Code;
                awsComment.CreateBy = user.UserName;
                context.AWS_COMMENTS.Add(awsComment);
                context.SaveChanges();
                return comment;
            }
        }

    }

}
