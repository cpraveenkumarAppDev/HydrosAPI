
namespace HydrosApi.Models.Adjudication
{
    using Data;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using ADWR;

    /// <summary>
    /// Notice of Appropriation
    /// </summary>
    /// <remarks>
    /// <para>       
    /// A collection of classes to populate notice of appropriation and its corresponding descriptions for codes
    /// </para>      
    /// </remarks>
    [Table("ADJ_INV.NOTICE_OF_APPROPRIATION")]
    public partial class NoticeOfAppropriation : AdwrRepository<NoticeOfAppropriation>
    {
        [Key, Column("ID")]
        public int? Id { get; set; }

        [Column("PROGRAM")]
        public string Program { get; set; } = "10";

        [Column("FILE_NO")]
        public string FileNo { get; set; }

        [Column("FILE_EXT")]
        public string FileExt { get; set; }

        [Column("DAT_SRC_CODE")]
        public string DataSourceCode { get; set; }

        [Column("CLAIMANT_ID")]
        public int? ClaimantId { get; set; }

        [Column("FILE_DATE")]
        public DateTime? FileDate { get; set; }
        
        [Column("CLAIM_DATE")]
        public DateTime? ClaimDate { get; set; }

        [Column("COUNTY")]
        public string County { get; set; }

        [Column("BOOK")]
        public int? Book { get; set; }

        [Column("PAGE_FR")]
        public int? PageFrom { get; set; }

        [Column("PAGE_TO")]
        public int? PageTo { get; set; }

        [Column("SUB_WATERSHED")]
        public string SubWatershed { get; set; }

        [Column("CLAIM_LOC")]
        public string ClaimLocation { get; set; }

        [Column("CLAIM_TWNSHP")]
        public string ClaimTownship { get; set; }

        [Column("CLAIM_RANGE")]
        public string ClaimRange { get; set; }

        [Column("CLAIM_SEC")]
        public string ClaimSec { get; set; }

        [Column("LOC_LEGAL")]
        public string LocLegal { get; set; }

        [Column("LOC_CAD")]
        public string LocCadastral { get; set; }

        [Column("QUANTITY")]
        public decimal? Quantity { get; set; }

        [Column("UTM_X")]
        public decimal? UtmX { get; set; }

        [Column("UTM_Y")]
        public decimal? UtmY { get; set; }

        [Column("REMARKS")]
        public string Remarks { get; set; }

        [Column("WTR_SRC")]
        public string WaterSource { get; set; }

        [Column("NOA_USE_DESCR")]
        public string UseDescription { get; set; } //multiple, comma delimited

        [Column("CUOM_CODE")]
        public string UnitOfMeasureCode { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("CREATEBY")]
        public string CreateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }

        [Column("CLAIM_TWNSHP_DIR")]
        public string ClaimTownshipDirection { get; set; }

        [Column("CLAIM_RANGE_DIR")]
        public string ClaimRangeDirection { get; set; }

        [Column("QUARTER_160_ACRE")]
        public string Quarter160Acre { get; set; }

        [Column("QUARTER_40_ACRE")]
        public string Quarter40Acre { get; set; }

        [Column("QUARTER_10_ACRE")]
        public string Quarter10Acre { get; set; }


        [NotMapped]
        public bool? DeleteRecord { get; set; }

        [NotMapped]
        public bool? RestoreRecord { get; set; }

        [NotMapped]
        public string DataSourceDescription { 
            get => DataSourceCode != null ? NoticeOfAppropriationDataSource.Get(s=>s.Code==DataSourceCode).Description : null; 
            //set => DataSourceDescription=value; 
        }

        [NotMapped]
        public string UnitOfMeasureDescription
        {
            get => UnitOfMeasureCode != null ? CdUnitOfMeasureSOC.Get(u => u.Code == UnitOfMeasureCode).Description : null;
            //set => UnitOfMeasureDescription = value;
        }

        [NotMapped]
        public string ClaimantNew { get; set; }


        [NotMapped]
        public string FormattedClaimDate {
            get => ClaimDate != null ? ClaimDate.Value.ToShortDateString() : null;
        }
        [NotMapped]
        public string Claimant
        {
            get => ClaimantId != null ?  NoticeOfAppropriationClaimant.Get(s => s.Id == ClaimantId).Claimant : null;
            
        }

        [NotMapped]
        public string BookInfo
        {
            get => Book != null ? string.Format("Book {0} Page {1}-{2}",Book, PageFrom, PageTo) : null;
            //set => BookInfo = value;
        }

        [NotMapped]
        public string FileNumber
        {
            get => string.Format("{0}-{1}.{2}", Program, FileNo, FileExt);
            /*set
            {

                FileNumber = value;

                if (value != null)
                {

                    Regex regex = new Regex(@"([1-9][0-9])\D?([0-9]{6})\D?([0-9]{4})");

                    if (Program == null)
                    {
                        Program = regex.Replace(value, "$1");
                    }

                    if (FileNo == null)
                    {
                        FileNo = regex.Replace(value, "$2");
                    }

                    if (FileExt == null)
                    {
                        FileExt = regex.Replace(value, "$3");
                    }

                    //WaterRightFacilityId = QueryResult.RgrRptGet(value);

                }

            }*/
        }
        [NotMapped]

        public string Message { get; set; } //return error messages here       


        [NotMapped]
        public List<NoticeOfAppropriationClaimant> UpdatedClaimantList { get; set; } //return error messages here       

    }          

    /// <summary>
    /// NoticeOfAppropriationUseDescr
    /// </summary>
    /// <remarks>
    /// Code/Description table (ADJ_INV.CD_NOA_USE) for Notice of Appropriation Use
    /// </remarks>
    [Table("ADJ_INV.CD_NOA_USE")]
    public partial class NoticeOfAppropriationUseCode : AdwrRepository<NoticeOfAppropriationUseCode>
    {

        [Key, Column("CODE")]
        public string Code { get; set; }

        [Column("DESCR")]
        public string Description { get; set; }
    }

    [Table("ADJ_INV.CD_NOA_DAT_SRC")]
    public partial class NoticeOfAppropriationDataSource : AdwrRepository<NoticeOfAppropriationDataSource>
    {

        [Key, Column("CODE")]
        public string Code { get; set; }

        [Column("DESCR")]
        public string Description { get; set; }
    }
        

    [Table("ADJ_INV.NOA_CLAIMANT")]
    public partial class NoticeOfAppropriationClaimant: AdwrRepository<NoticeOfAppropriationClaimant>
    {

        [Key, Column("ID")]
        public int? Id { get; set; }

        [Column("CLAIMANT")]
        public string Claimant{ get; set; }

        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }

        [Column("CREATEBY")]
        public string CreateBy { get; set; }


    }
    /// <summary>
    /// NoticeOfAppropriationUse
    /// </summary>
    /// <remarks>
    /// Table (ADJ_INV.NOA_USE) containing the many to one relationship for NoticeOfAppropriation uses for Notice of Appropriation Use
    /// Contains an unmapped column that assigns the description from the corresponding code table (ADJ_INV.CD_NOA_USE)
    /// </remarks>

    /*[Table("ADJ_INV.NOA_USE")]
    public partial class NoticeOfAppropriationUse : AdwrRepository<NoticeOfAppropriationUse>
    {

        [Key, Column("ID")]
        public int? Id { get; set; }

        [Column("NOA_ID")]
        public int? NoticeOfAppropriationId { get; set; }

        [Column("CNUS_CODE")]
        public string UseCode { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("CREATEBY")]
        public string CreateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }

        [NotMapped]
        public string UseDescription
        {
            get => UseCode != null ? NoticeOfAppropriationUseCode.Get(d => d.Code == UseCode).Description : null;
            set => UseDescription = value;
        }
    }*/

    //set the codes here with a custom view
    public class NoticeOfAppropriationView
    {
        [Key]
        public int? Id { get; set; }

        public List<NoticeOfAppropriation> NoticeOfAppropriationAll { get; set; }

        public Dictionary<string, object> NoticeOfAppropriationCodeList { get; set; }

        public NoticeOfAppropriation NoticeOfAppropriationSelection { get; set; }




        public static NoticeOfAppropriationView PopulateNoaView(int? Id=null)
        {
            var noaView = new NoticeOfAppropriationView();
            if(Id==null)
            {
                noaView.NoticeOfAppropriationAll = NoticeOfAppropriation.GetAll();

                var noticeOfAppropriationCodeList = new Dictionary<string, object>();
                var countyList = CdAwCounty.GetAll();
                var claimantList = NoticeOfAppropriationClaimant.GetAll().OrderBy(c=>c.Claimant);
                var dataSourceList = NoticeOfAppropriationDataSource.GetAll();
                var useList = NoticeOfAppropriationUseCode.GetAll();
                var unitOfMeasureList = CdUnitOfMeasureSOC.GetAll();
                var subWatershedList = WatershedView.GetAll();              

                noticeOfAppropriationCodeList.Add("CountyList", countyList != null && countyList.Count() > 0 ? countyList : null);               
                noticeOfAppropriationCodeList.Add("ClaimantList", claimantList != null && claimantList.Count() > 0 ? claimantList : null); 
                noticeOfAppropriationCodeList.Add("DataSourceList", dataSourceList != null && dataSourceList.Count() > 0 ? dataSourceList : null);               
                noticeOfAppropriationCodeList.Add("UseList", useList != null && useList.Count() > 0 ? useList : null);
                noticeOfAppropriationCodeList.Add("UnitOfMeasureList", unitOfMeasureList != null && unitOfMeasureList.Count() > 0 ? unitOfMeasureList : null);
                noticeOfAppropriationCodeList.Add("SubWatershedList", subWatershedList != null && subWatershedList.Count() > 0 ? subWatershedList : null);

                if (noticeOfAppropriationCodeList != null)
                    noaView.NoticeOfAppropriationCodeList = noticeOfAppropriationCodeList;

                //if(unitOfMeasure != null)
                    //noaView.NoticeOfAppropriationCodeList.Add(unitOfMeasure);

               // if(subWatershed != null && subWatershed.Count() > 0)
                    //noaView.NoticeOfAppropriationCodeList.Add(subWatershed);
            }

            else
            {
                noaView.Id = Id;
                noaView.NoticeOfAppropriationSelection = NoticeOfAppropriation.Get(n => n.Id == Id);
            }

            return noaView;

        }
    }
    
}


/*
"ID","NUMBER(10)"
"WRF_ID","NUMBER(10)"
"PROGRAM_CODE","VARCHAR2(3)"
"CERT_NO","VARCHAR2(6)"
"CONV_NO","VARCHAR2(4)"
"CNDS_CODE","VARCHAR2(4)"
"CLAIMANT_ID","NUMBER(10)"
"FILE_DATE","DATE"
"CLAIM_DATE","DATE"
"COUNTY","VARCHAR2(30)"
"BOOK","NUMBER(10)"
"PAGE_FR","NUMBER(10)"
"PAGE_TO","NUMBER(10)"
"WS_CODE","VARCHAR2(2)"
"SWS_CODE","NUMBER(1)"
"CLAIM_LOC","VARCHAR2(20)"
"CLAIM_TWNSHP","VARCHAR2(20)"
"CLAIM_RANGE","VARCHAR2(20)"
"CLAIM_SEC","VARCHAR2(20)"
"LOC_LEGAL","VARCHAR2(20)"
"LOC_CAD","VARCHAR2(15)"
"UTM_X","NUMBER(38,8)"
"UTM_Y","NUMBER(38,8)"
"REMARKS","VARCHAR2(2000)"
"QUANTITY","NUMBER(20,3)"
"WTR_SRC","VARCHAR2(20)"
"CREATEDT","DATE"
"CREATEBY","VARCHAR2(30)"
"UPDATEDT","DATE"
"UPDATEBY","VARCHAR2(30)"
*/
