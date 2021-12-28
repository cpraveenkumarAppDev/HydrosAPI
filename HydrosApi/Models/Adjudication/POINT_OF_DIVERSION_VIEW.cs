using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HydrosApi.Models.Adjudication
{
    [Table("ADJ_INV.POINT_OF_DIVERSION")]
    public class POINT_OF_DIVERSION_VIEW : AdwrRepository<POINT_OF_DIVERSION_VIEW>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public POINT_OF_DIVERSION_VIEW()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int? ID { get; set; }
        [Column("OBJECTID")]
        public int OBJECTID { get; set; }
        [NotMapped]
        public List<SOC_AIS_VIEW> StatementOfClaim { get; set; }
        [NotMapped]
        public List<WELLS_VIEW> Well { get; set; }
        [NotMapped]
        public List<SW_AIS_VIEW> Surfacewater { get; set; }
        [NotMapped]
        public List<EXPLANATIONS> Explanation { get; set; }

         [NotMapped]
        public List<ExplanationType> ExplanationTypeList { get; set; }
        [NotMapped]
        public List<FILE> FileList { get; set; }

        [NotMapped]
        public POINT_OF_DIVERSION PointOfDiversionSde { get; set; }

        public static POINT_OF_DIVERSION_VIEW PointOfDiversionView(int id)
        {
            var pod = new POINT_OF_DIVERSION_VIEW();

            var podData = Get(p => p.OBJECTID == id);

            if(podData != null)
            {              
                pod = podData;
            }
            else
            {
                pod.OBJECTID = id;
            }
                       
            var podSde = POINT_OF_DIVERSION.Get(p => p.OBJECTID == id);


            if (podData != null)
            {
                pod.Explanation = EXPLANATIONS.GetList(p => p.POD_ID == pod.ID);
                pod.FileList = FILE.GetList(p => p.POD_ID == pod.ID);
            }
            
            char[] delimiters = new[] { ',', ';' };

            if (podSde != null)
            {
                pod.PointOfDiversionSde = podSde;

                if (podSde.SOC != null)
                {
                    var soc = FileFromStringList.GetFileFromStringList(podSde.SOC, delimiters);
                    pod.StatementOfClaim = soc?.Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.NumericFileNo)).Distinct().ToList();
                }

                if (podSde.BOC != null)
                {
                    var bocList = FileFromStringList.GetFileFromStringList(podSde.BOC, delimiters);
                    var wellList = bocList?.Where(p => p.Program == "55" || p.Program == "35");
                    var swList = bocList?.Where(p => p.Program != "55" && p.Program != "35");
                    pod.Well = wellList?.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.FileNo && s.PROGRAM == f.Program)).ToList();
                    pod.Surfacewater = swList?.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.NumericFileNo)).ToList();
                }
            }

            if (pod != null)
            {
                pod.ExplanationTypeList = ExplanationType.GetAll();
            }


            /*

        pod.SOC = podSde.SOC == null ? null : Regex.Replace(podSde.SOC, @"\s+", "") == "" ? null :
                (from s in podSde.SOC.Split(delimiters)
                 select new
                 {
                     program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                     file_no = int.Parse((s.IndexOf("-") > -1 ? s.Split('-')[1].Replace(" ", "") : s).Replace(" ", ""))
                 }).Select(f => SOC_AIS_VIEW.Get(s => s.FILE_NO == f.file_no)).Where(c => c != null).Distinct().ToList();

        if (podSde.BOC != null && Regex.Replace(podSde.BOC, @"\s+", "") != "")
        {
            var bocList = (from p in (from s in podSde.BOC.Split(delimiters)
                                      select new
                                      {
                                          program = s.IndexOf("-") > -1 ? s.Split('-')[0].Replace(" ", "") : "",
                                          file_no = (s.IndexOf("-") > -1 ? s.Split('-')[1].Split('.')[0].Replace(" ", "") : s).Replace(" ", "")
                                      })
                           select new
                           {
                               p.program,
                               p.file_no,
                               numeric_file_no = int.Parse(p.file_no == null ? "0" : p.file_no.ToString()),
                               registry_id = p.program + "-" + p.file_no
                           }).Distinct();
            var wellList = bocList.Where(p => p.program == "55" || p.program == "35");
            var swList = bocList.Where(p => p.program != "55" && p.program != "35");

            pod.Well = wellList == null ? null :
                wellList.Select(f => WELLS_VIEW.Get(s => s.FILE_NO == f.file_no && s.PROGRAM == f.program)).Where(c => c != null).ToList();
            pod.Surfacewater = swList == null ? null : swList.Select(f => SW_AIS_VIEW.Get(s => s.ART_APPLI_NO == f.numeric_file_no)).Where(c => c != null).ToList();
        }*/


            return pod;
        }
    }
}


//OBJECTID
//CLAIMANT
//OWNER_NAME
//FILE_NO
//BOC
//POD_TYPE
//POD_SOURCE
//POD_NAME
//POD_PWRS
//POD_SHARED
//POD_REMARK
//POD_PARCEL
//INSTALLED
//WELL_DEPTH
//WATER_LEVE
//CASING_DEP
//CASING_DIA
//CASING_TYP
//PUMPRATE
//COMPLETION
//WELL_CANCE
//CADASTRAL
//COUNTY
//WATERSHED
//BASIN_NAME
//SUBBASIN_N
//PUMP_CAPAC
//DIV_COMPLE
//DIVERSIO_2
//PUMPED_VOL
//PV_2015_SO
//PUMPED_V_1
//PV_2016_SO
//PUMPED_V_2
//PV_2017_SO
//PUMPED_V_3
//PV_2018_SO
//PUMPED_V_4
//PV_2019_SO
//DWR_ID
//ACTIVE_INACTIVE
