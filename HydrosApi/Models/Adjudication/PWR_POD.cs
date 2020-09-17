namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;

    [Table("ADJ_INV.PWR_POD")]

    public partial class PWR_POD 
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? PWR_ID { get; set; }

        public int? POD_ID { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [NotMapped]
        public virtual PROPOSED_WATER_RIGHT PROPOSED_WATER_RIGHT { get; set; }

        public static List<PWR_POD> ProposedWaterRightToPoint(int podobjectid, int pwrId) //gets (what should be single) record for the specified pwr/pod
        {
            using (var db = new ADWRContext())
            {
                return db.PWR_POD.Where(p => (p.POD_ID ?? -1) == podobjectid && (p.PWR_ID ?? -1) == pwrId).ToList();
            }
        }

        public static List<PWR_POD> ProposedWaterRightAllPoint(int pwrId) //gets a list of the objectid(s) associated with the proposed water right
        {
            using (var db = new ADWRContext()) { 
                return db.PWR_POD.Where(p => (p.PWR_ID ?? -1) == pwrId).ToList();
            }
        }

        public static PWR_POD ProposedWaterRightToPoint(int pwrPodId) ///the ID in the PWR_POD table (should return a single record)
        {
            using (var db = new ADWRContext())
            {
                return db.PWR_POD.Where(i => i.ID == pwrPodId).FirstOrDefault();
            }
        }
                 
    }
}
