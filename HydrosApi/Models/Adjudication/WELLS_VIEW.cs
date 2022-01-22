using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi
{

    [Table("ADJ_INV.WELLS_VIEW")]
    public partial class WELLS_VIEW : AdwrRepository<WELLS_VIEW>
    {

        [NotMapped]
        [StringLength(4000)]
        public string FILE_LINK
        {
            get
            {
                if (this.REGISTRY_ID != null)
                {
                    DocushareService doc = new DocushareService();
                    var item = doc.getWellDocs(this.REGISTRY_ID);
                    StatusMsg = item == null ? "Could not find file" : item.Status != null ? item.Status : null;
                    return item?.FileUrl;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                this.FILE_LINK = value;
            }

        }
        [StringLength(11)]
        public string PCC { get; set; }
        [StringLength(61)]
        public string OWNER { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(80)]
        public string WELL_TYPE { get; set; }
        public DateTime? APPLICATION_DATE { get; set; }

        public decimal? ID { get; set; }

        [StringLength(2)]
        public string PROGRAM { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(6)]
        public string FILE_NO { get; set; }

        [StringLength(93)]
        public string CADASTRAL { get; set; }



        [StringLength(9)]
        public string REGISTRY_ID { get; set; }

        [StringLength(2)]
        public string WSHD_CODE { get; set; }

      

        

        public DateTime? INSTALL_DATE { get; set; }

        [NotMapped]
        public string StatusMsg { get; set; }

        
        public static List<WELLS_VIEW> WellsView(List<FileFromStringList> fileInfo) //a comma-delimited list
        {
            var well = new List<WELLS_VIEW>();
            try
            {
                if (fileInfo == null)
                    return null;

                foreach (var f in fileInfo)
                {
                    var w = Get(s => s.FILE_NO == f.FileNo6 && s.PROGRAM == f.Program);

                    if(w==null)
                    {
                        w = new WELLS_VIEW()
                        {
                            PROGRAM = f.Program,
                            FILE_NO = f.FileNo6,
                            StatusMsg = string.Format("Could not find a record for Well {0}", f.UserValue),
                            PCC= string.Format("Error: {0}", f.UserValue)
                        };
                    }

                    well.Add(w);
                }
                return well.Distinct().ToList();
            }
            catch (Exception exception)
            {
                var errorWell = new WELLS_VIEW();
                errorWell.StatusMsg = string.Format("Error{0}", exception.Message);
                well.Add(errorWell);
                return well;
            }

        }
    }
}
