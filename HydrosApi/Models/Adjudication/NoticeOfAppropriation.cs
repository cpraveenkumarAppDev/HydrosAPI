
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

        [Column("WRF_ID")]
        public int? WaterRightFacilityId { get; set; }


        [Column("PROGRAM_CODE")]
        public string Program { get; set; }

        [Column("CERT_NO")]
        public string Certificate { get; set; }

        [Column("CONV_NO")]
        public string Conveyance { get; set; }

        [Column("CNDS_CODE")]
        public string DataSourceCode { get; set; }

        [Column("CLAIMANT_ID")]
        public int? ClaimantId { get; set; }

        [Column("FILE_DATE")]
        public DateTime? FileDate { get; set; }

        [Column("CLAIM_DATE")]
        public DateTime? ClaimDate { get; set; }

        [Column("UTM_X")]
        public decimal? UtmX { get; set; }

        [Column("UTM_Y")]
        public decimal? UtmY { get; set; }

        [Column("REMARKS")]
        public string Remarks { get; set; }

        [Column("CREATEDT")]
        public DateTime? CreateDt { get; set; }

        [Column("CREATEBY")]
        public string CreateBy { get; set; }

        [Column("UPDATEDT")]
        public DateTime? UpdateDt { get; set; }

        [Column("UPDATEBY")]
        public string UpdateBy { get; set; }



        //public NoticeOfAppropriationView NoaDescription { get; set; }


        //public List<NoticeOfAppropriationUse> NoaUse { get; set; }
        [NotMapped]
        public List<NoticeOfAppropriationUseCode> NoaUseCode { get; set; }

        [NotMapped]
        public string PCC
        {
            get => string.Format("{0}-{1}.{2}", Program, Certificate, Conveyance);
            set
            {

                PCC = value;

                if (value != null)
                {

                    Regex regex = new Regex(@"([1-9][0-9])\D?([0-9]{6})\D?([0-9]{4})");

                    if (Program == null)
                    {
                        Program = regex.Replace(value, "$1");
                    }

                    if (Certificate == null)
                    {
                        Certificate = regex.Replace(value, "$2");
                    }

                    if (Conveyance == null)
                    {
                        Conveyance = regex.Replace(value, "$3");
                    }

                    WaterRightFacilityId = QueryResult.RgrRptGet(value);

                }

            }
        }
        [NotMapped]

        public string Errors { get; set; }
        
    }
          /*  /// <summary>
            /// Get data, including code descriptions for Notices of Appropriation
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static NoticeOfAppropriation NoticeOfAppropriationData(int? id)
        {
            var errorNoa= new NoticeOfAppropriation();
            if(id==null)
            {
                errorNoa.Errors = "id is empty";
                return errorNoa;
            }

          //  Regex regex = new Regex(@"(\d{2})\D?(\d{6})\D?(\d{1,4})");
           // var pcc = regex.Replace(id, "$1-$2.$3");

            var noa = Get(n => n.Id == id);

            if(noa==null)
            {
                errorNoa.Errors = "id not found";
                return errorNoa;
            }

            //var noaDescr = new NoticeOfAppropriationView();           
            var noaUse = NoticeOfAppropriationUse.GetList(u => u.NoticeOfAppropriationId == id); 
             
            //noa.NoaDescription = noaDescr;
            noa.NoaUse = noaUse;
             
            return noa;
        }

        //
        public static NoticeOfAppropriation NoticeOfAppropriationData(NoticeOfAppropriation noa)
        {
            return noa;

        }
    }*/

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
    }
    /// <summary>
    /// NoticeOfAppropriationUse
    /// </summary>
    /// <remarks>
    /// Table (ADJ_INV.NOA_USE) containing the many to one relationship for NoticeOfAppropriation uses for Notice of Appropriation Use
    /// Contains an unmapped column that assigns the description from the corresponding code table (ADJ_INV.CD_NOA_USE)
    /// </remarks>

    [Table("ADJ_INV.NOA_USE")]
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
    }


    //set the codes here with a custom view

    public class NoticeOfAppropriationView
    {
        [Key]
        public int? Id { get; set; }
        public List<NoticeOfAppropriationUseCode> NoticeOfAppropriationUseList {
            get => NoticeOfAppropriationUseCode.GetAll();
        }

        public List<NoticeOfAppropriationDataSource> NoticeOfAppropriationDataSourceList
        { 
            get => NoticeOfAppropriationDataSource.GetAll();
        }

        public List<CdAwCounty> CountyList
        {
            get => CdAwCounty.GetAll();
            set => CountyList = value;
        }

        public List<NoticeOfAppropriationClaimant> NoticeOfAppropriationClaimantList
        {
            get => NoticeOfAppropriationClaimant.GetAll();
        }

        public List<NoticeOfAppropriation> NoticeOfAppropriationAll
        {
            get; set;
        }

        public NoticeOfAppropriation NoticeOfAppropriationSelection
        {
            get; set;
        }

        public static NoticeOfAppropriationView PopulateNoaView(int? Id=null)
        {
            var noaView = new NoticeOfAppropriationView();
            if(Id==null)
            {
                noaView.NoticeOfAppropriationAll = NoticeOfAppropriation.GetAll();
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
