using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi.Models 
{
    [Table("SOC.SOC_AIS_VIEW")]
    public partial class SOC_AIS_VIEW : AdwrRepository<SOC_AIS_VIEW>
    {
        [NotMapped]
        [StringLength(4000)]
        public string FILE_LINK
        {
            get; set;
          /* get
            {
                if (FILE_NO != null)
                {
                    DocushareService doc = new DocushareService();
                    var docItem = doc.GetSocDocs("39-" + this.FILE_NO).FirstOrDefault();
                    if (docItem.Status != null)
                    {
                        StatusMsg = docItem.Status;
                    }
                    return docItem.FileUrl;
                }
                return null;
            }

            set
            {
                this.FILE_LINK = value;
            }*/
        }
        [StringLength(45)]
        public string PCC { get; set; }
        public string FILE_STATUS { get; set; }
        [StringLength(50)]
        public string NAME { get; set; }
        [StringLength(50)]
        public string USE { get; set; }
        public DateTime? FILE_DATE { get; set; }

        public DateTime? AMEND_DATE { get; set; }
        public decimal? ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? FILE_NO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string WS { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? MAIN_ID { get; set; }

        [NotMapped]
        public List<SOCDOC> StatementOfClaimDocument { 
            get
            {
                DocushareService doc = new DocushareService();
                return doc.GetSocDocs("39-" + this.FILE_NO);
            }
            set
            {
                StatementOfClaimDocument = value;
            }
        
        }

        [NotMapped]
        public string StatusMsg { get; set; }
    
       
        public static List<SOC_AIS_VIEW> StatementOfClaimView(List<int> fileNumberList) //a comma-delimited list
        {
            try
            { 
                if(!(fileNumberList!=null && fileNumberList.Count() > 0))
                {
                    return null;
                }

                var socList = GetList(s => fileNumberList.Contains(s.FILE_NO ?? -1));

                var statement = (from fileNumber in fileNumberList
                                 join soc in socList on fileNumber equals soc.FILE_NO ?? -1 into socJoin
                                 from sj in socJoin.DefaultIfEmpty()
                                 select new
                                 {
                                     fileNumber,
                                     statementJoin = sj != null ? sj : new SOC_AIS_VIEW()
                                     {
                                         FILE_NO = fileNumber,
                                         PCC= string.Format("Error: 39-{0}", fileNumber),
                                         StatusMsg = string.Format("Could not find a record for FILE_NO 39-{0}", fileNumber)
                                     }

                                 }).Select(s => s.statementJoin).Distinct().ToList();


                return statement;

            }
            catch(Exception exception)
            {
                var errorSOCList= new List<SOC_AIS_VIEW>();
                var errorSOC = new SOC_AIS_VIEW();
                errorSOC.StatusMsg = string.Format("Error{0}", exception.Message);
                errorSOCList.Add(errorSOC);
                return errorSOCList;
            }
          
        }
    }
}
