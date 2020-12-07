namespace HydrosApi.Models
{

    using System;
    using System.Globalization;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Data;

    [Table("AWS.CD_AW_COMMENT_TYPE")]
    public class CD_AW_COMMENT_TYPE : Repository<CD_AW_COMMENT_TYPE>
    {
        [Key]
        public string CODE { get; set; }
        public string DESCR { get; set; }
    }
}