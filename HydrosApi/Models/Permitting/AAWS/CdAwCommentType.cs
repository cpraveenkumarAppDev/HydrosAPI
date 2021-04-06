namespace HydrosApi.Models
{

    using System;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;

    [Table("AWS.CD_AW_COMMENT_TYPE")]
    public class CdAwCommentType : Repository<CdAwCommentType>//CD_AW_COMMENT_TYPE
    {
        [Key]
        [Column("CODE")]
        public string Code { get; set; }//CODE

        [Column("DESCR")]
        public string Description { get; set; }//DESCR
    }
}