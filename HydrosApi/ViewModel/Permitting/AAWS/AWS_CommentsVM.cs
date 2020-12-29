using HydrosApi.Models;
using HydrosApi.Models.ADWR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.ViewModel.Permitting.AAWS
{
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
            var awsComments = AWS_COMMENTS.GetList(p => p.WRF_ID == id).OrderByDescending(t => t.CMT_DT_TIME);
            foreach (var comments in awsComments)
            {
                var user = AW_USERS.Get(p => p.ID == comments.AUSR_ID);
                var commentTypes = CD_AW_COMMENT_TYPE.Get(p => p.CODE == comments.CACM_CODE);
                var commentObj = new AWS_CommentsVM();
                commentObj.WrfId = comments.WRF_ID;
                commentObj.Date = comments.CMT_DT_TIME.ToString("M/d/yyyy");
                commentObj.Code = commentTypes.CODE;
                commentObj.Description = commentTypes.DESCR;
                commentObj.FileManager = user.FIRST_NAME + " " + user.LAST_NAME;
                commentObj.Comment = comments.COMMENTS;
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
