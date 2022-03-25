using HydrosApi.Services;
using HydrosApi.Services.docushareClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using HydrosApi.Data;

namespace HydrosApi.Models
{ 

       //ID, ART_PROGRAM, ART_APPLI_NO, ART_CONVY_NO, PCC, OWNER_NAME

    [Table("ADWR.SW_AIS_VIEW")]

    public partial class SW_AIS_VIEW:AdwrRepository<SW_AIS_VIEW>
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [StringLength(6)]
        public string ART_PROGRAM { get; set; }

        public int? ART_APPLI_NO { get; set; }

        public int? ART_CONVY_NO { get; set; }

        [NotMapped]
        public List<SWDOC> SurfaceWaterDocument { get; set; }


        [NotMapped]         
        public List<string> FILE_LINK
        {
            get; set;
           
           /* get
            {
                if (ART_APPLI_NO != null)
                {
                    DocushareService doc = new DocushareService();
                    var docItem = doc.getSurfaceWaterDocs(this.ART_PROGRAM + "-" + this.ART_APPLI_NO);
                    var docStatus = docItem?.Where(d => d.Status != null)?.Select(d => d.Status)?.FirstOrDefault();

                    StatusMsg = docStatus;

                    return docItem?.Select(f => f.FileUrl).Distinct().ToList();
                }
                return null;
            }

            set
            {
                this.FILE_LINK = value;
            }*/
        }

        [Column("OWNER_NAME"), StringLength(40)]
        public string NAME { get; set; }


        //[StringLength(4000)]
        //public string USE { get; set; }
        [Column("PCC"), StringLength(88)]
        public string PCC { get; set; }
       
        [NotMapped]
        public string StatusMsg { get; set; }

        public static List<SW_AIS_VIEW> SurfaceWaterView(List<FileFromStringList> fileInfo) //a comma-delimited list
        {
            var surface = new List<SW_AIS_VIEW>();
            try
            {
                if (fileInfo == null)
                    return null;

                
                foreach (var f in fileInfo)
                {                                  
                    var sw = f.Program != "CW" ? Get(s => s.ART_APPLI_NO == f.NumericFileNo && s.ART_PROGRAM == f.Program) : null;

                    if (sw != null)
                    {
                        DocushareService doc = new DocushareService();
                        var docItem = doc.getSurfaceWaterDocs(sw.ART_PROGRAM + "-" + sw.ART_APPLI_NO)?.OrderByDescending(x=>x.Handle).ToList();
                        var docStatus = docItem?.Where(d => d.Status != null)?.Select(d => d.Status)?.FirstOrDefault();
                        sw.SurfaceWaterDocument = docItem;
                        sw.StatusMsg = docStatus;
                        sw.FILE_LINK=docItem?.Select(d => d.FileUrl).Distinct().ToList();
                    }

                    else // (sw == null || sw.ART_PROGRAM != f.Program)
                    {
                        sw = new SW_AIS_VIEW()
                        {
                            ART_PROGRAM = f.Program,
                            ART_APPLI_NO = f.NumericFileNo,
                            StatusMsg = string.Format("Could not find a record for Surfacewater {0}", f.UserValue),
                            PCC = string.Format("Error: {0}", f.UserValue)
                        };
                    }
                 
                    surface.Add(sw);
                }
                return surface.Distinct().ToList();
            }
            catch (Exception exception)
            {
                var errorSw = new SW_AIS_VIEW();
                errorSw.StatusMsg = string.Format("Error{0}", exception.Message);
                surface.Add(errorSw);
                return surface;
            }

        }
    }
}
