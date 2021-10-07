 
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Text.RegularExpressions;
     
    using HydrosApi.Data;
    namespace HydrosApi.Models.Adjudication
    {

        [Table("ADJ_INV.PROPOSED_WATER_RIGHT")]
    public partial class PROPOSED_WATER_RIGHT : AdwrRepository<PROPOSED_WATER_RIGHT>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROPOSED_WATER_RIGHT()
        {
            PWR_POD = new HashSet<PWR_POD>();
        }

        [Key]
        public int ID { get; set; }

        public int? WFR_ID { get; set; }

        [StringLength(2000)]
        public string COMMENTS { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [StringLength(50)]
        public string POU_ID { get; set; }
        [StringLength(50)]
        public string POU_NAME { get; set; }
        [StringLength(50)]
        public string POU_CODE { get; set; }

        public int? POU_SEQ { get; set; }
        [StringLength(50)]
        public string QUANTITY { get; set; }
        [StringLength(50)]
        public string WILDLIFE { get; set; }

        [NotMapped]
        public string StatusMessage { get; set; }

        [NotMapped]
        public List<IrrigationData> Irrigation
        {
            get => IrrigationData.GetList(i => i.ProposedWaterRightId == ID);
            set => Irrigation = value;
        }

        public static PROPOSED_WATER_RIGHT ProposedWaterRight(string pouId)
        {
            Regex rgx = new Regex(@"[^0-9]");
            var pwr= !rgx.IsMatch(pouId) ? ProposedWaterRight(int.Parse(pouId)) : Get(p => p.POU_ID == pouId);
            return pwr;
        }

        public static PROPOSED_WATER_RIGHT ProposedWaterRight(int id)
        { 
            return Get(p => p.ID == id);             
        }

        //public static <List>PROPOSED_WATER_RIGHT WaterRight(string DWR_ID)
        //{
        //    var pwrs = 
        //}
        /* 
       [StringLength(255)]
       public string WATER_STRUCTURE_FAC_TYPE { get; set; }

       public decimal? WATER_STRUCTURE_FAC_CAP { get; set; }

       [StringLength(500)]
       public string WATER_STRUCTURE_FAC_PUR { get; set; }

       [StringLength(255)]
       public string WATER_REUSE_PROGRAM { get; set; }

       [StringLength(255)]
       public string DISCHARGE_OF_WAISTWATER { get; set; }

       [StringLength(255)]
       public string TREATMENT_OF_WAISTWATER { get; set; }

       [StringLength(255)]
       public string PERIODS_OF_INACTIVITY { get; set; }

       public short? INACTIVITY_MONTHS { get; set; }

       public short? PEOPLE { get; set; }

       [StringLength(255)]
       public string EXPANSION_PLANS { get; set; }

       [StringLength(500)]
       public string EXPANSION_EXPLANATION { get; set; }

       [StringLength(20)]
       public string DRILL_DATE { get; set; }

       public decimal? DEPTH { get; set; }

       [StringLength(25)]
       public string ADEQ_ID { get; set; }

       [StringLength(25)]
       public string PWS_ID_NO { get; set; }

       [StringLength(25)]
       public string CWS_ID_NO { get; set; }

       [StringLength(25)]
       public string ACC_DOC_NO { get; set; }

       public short? NO_CONN { get; set; }

       [StringLength(10)]
       public string ACTIVE { get; set; }

       [StringLength(50)]
       public string CTY_DOC_NO { get; set; }

       [StringLength(50)]
       public string OLD_WFR { get; set; }

       [StringLength(50)]
       public string DIVERSION_STATUS { get; set; }

       [StringLength(50)]
       public string STATUS_SOURCE { get; set; }

       */

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        


        public virtual ICollection<PWR_POD> PWR_POD { get; set; }

         
        
    }
}
