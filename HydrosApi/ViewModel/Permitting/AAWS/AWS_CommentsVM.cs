namespace HydrosApi.ViewModel.Permitting.AAWS
{
    using Models;
    using Models.ADWR;
    using Models.Permitting.AAWS;  
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class AWS_CommentsVM
    {
        public int WrfId { get; set; }
        public string FileManager { get; set; }
        public string Date { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public static List<AWS_CommentsVM> GetComments(int id)
        {
            List<AWS_CommentsVM> AWS_CommentsVM = new List<AWS_CommentsVM>();
            var awsComments = AwsComments.GetList(p => p.WaterRightFacilityId == id).OrderByDescending(t => t.CommentDate);
            foreach (var comments in awsComments)
            {
                var user = AwUsers.Get(p => p.Id == comments.AwsUserId);
                var commentTypes = CdAwCommentType.Get(p => p.Code == comments.CommentTypeCode);
                var commentObj = new AWS_CommentsVM();
                commentObj.WrfId = comments.WaterRightFacilityId;
                commentObj.Date = comments.CommentDate.ToString("g",DateTimeFormatInfo.InvariantInfo);
                commentObj.Code = commentTypes.Code;
                commentObj.Description = commentTypes.Description;
                commentObj.FileManager = user.FirstName + " " + user.LastName;
                commentObj.Comment = comments.Comments;
                AWS_CommentsVM.Add(commentObj);
            };

            return AWS_CommentsVM;
        }

        public static string TypeFormatter(string type)
        {
            var commentType = type == "HYDR" ? "hydrology" :
                type == "LGL" || type == "LGLI" ? "legalAvail" :
            type == "WQ" ? "waterQuality" : null;
            return commentType;
        }
    }

}
