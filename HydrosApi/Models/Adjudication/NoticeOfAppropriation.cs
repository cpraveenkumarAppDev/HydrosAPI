
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
    /// Notice of Appropriation (NoticeOfAppropriation.cs)
    /// </summary>
    /// <remarks>
    /// <para>       
    /// A collection of classes to populate notice of appropriation and associated data for code lists
    /// </para>  
    /// <para>NoticeOfAppropriation</para>      
    /// <para>NoticeOfAppropriationUseCode</para>      
    /// <para>NoticeOfAppropriationDataSource</para>
    /// <para>NoticeOfAppropriationClaimant</para>    
    /// <para>       
    /// NoticeOfAppropriationView - creates a package of data and code lists for the Notice of Appropriation tab
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
    public class NumericCounty
    {
        
        public string Code { get; set; }

        public string Description { get; set; }

        public static List<NumericCounty> PopulateNumericCounty()
        {
            var numericCounty = new List<NumericCounty>();
            numericCounty.Add(new NumericCounty() { Code = "01", Description = "Apache" });
            numericCounty.Add(new NumericCounty() { Code = "02", Description = "Cochise" });
            numericCounty.Add(new NumericCounty() { Code = "03", Description = "Coconino" });
            numericCounty.Add(new NumericCounty() { Code = "04", Description = "Gila" });
            numericCounty.Add(new NumericCounty() { Code = "05", Description = "Graham" });
            numericCounty.Add(new NumericCounty() { Code = "06", Description = "Greenlee" });
            numericCounty.Add(new NumericCounty() { Code = "07", Description = "Maricopa" });
            numericCounty.Add(new NumericCounty() { Code = "08", Description = "Mojave" });
            numericCounty.Add(new NumericCounty() { Code = "09", Description = "Navajo" });
            numericCounty.Add(new NumericCounty() { Code = "10", Description = "Pima" });
            numericCounty.Add(new NumericCounty() { Code = "11", Description = "Pinal" });
            numericCounty.Add(new NumericCounty() { Code = "12", Description = "Santa Cruz" });
            numericCounty.Add(new NumericCounty() { Code = "13", Description = "Yavapai" });
            numericCounty.Add(new NumericCounty() { Code = "14", Description = "Yuma" });
            numericCounty.Add(new NumericCounty() { Code = "15", Description = "La Paz" });

            return numericCounty;
        }
    }
        /// <summary>
        /// NoticeOfAppropriationView Class inside NoticeofAppropriation.cs
        /// </summary>
        /// <remarks>
        /// View that consolidates data and related code lists for select options    
        /// </remarks>    
        public class NoticeOfAppropriationView
    {
        [Key]
        public int? Id { get; set; }

        public List<NoticeOfAppropriation> NoticeOfAppropriationAll { get; set; }

        public Dictionary<string, object> NoticeOfAppropriationCodeList { get; set; }

        public NoticeOfAppropriation NoticeOfAppropriationSelection { get; set; }

        public static NoticeOfAppropriationView PopulateNoaView(string pgm, string fno, string fex)
        {
            var noaByFile = NoticeOfAppropriation.Get(n => n.Program == pgm && n.FileNo == fno && n.FileExt == fex);

            if(noaByFile != null)
            {
                return PopulateNoaView(noaByFile.Id);
            }

            return null;            
        }
        public static NoticeOfAppropriationView PopulateNoaView(int? Id=null)
        {
            var noaView = new NoticeOfAppropriationView();
            if (Id == null)
            {
                noaView.NoticeOfAppropriationAll = NoticeOfAppropriation.GetAll();

                var noticeOfAppropriationCodeList = new Dictionary<string, object>();
                //var countyList = CdAwCounty.GetAll();

                var countyList = NumericCounty.PopulateNumericCounty();
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

