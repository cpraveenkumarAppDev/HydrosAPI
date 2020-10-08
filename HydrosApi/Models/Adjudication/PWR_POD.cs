namespace HydrosApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;
    using HydrosApi.Data;

    [Table("ADJ_INV.PWR_POD")]

    public partial class PWR_POD : AdwrRepository<PWR_POD>
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

        [NotMapped]
        public virtual POINT_OF_DIVERSION PointOfDiversion
        {
            get
            {
                var pod = POINT_OF_DIVERSION.Get(p => p.OBJECTID == this.POD_ID);
                if(pod == null)
                {
                    return null;
                }
                else
                {
                    pod.PWR_POD_ID = this.ID;
                    pod.PWR_ID = this.PWR_ID;
                    return pod;
                }            
            }

            set
            {
                this.PointOfDiversion = value;
            }

        }

        /*public static List<PWR_POD> ProposedWaterRightToPoint(int podobjectid, int pwrId) //gets (what should be single) record for the specified pwr/pod
        {
            return PWR_POD.GetList(p => (p.POD_ID ?? -1) == podobjectid && (p.PWR_ID ?? -1) == pwrId);
        }

        public static List<PWR_POD> ProposedWaterRightAllPoint(int pwrId) //gets a list of the objectid(s) associated with the proposed water right
        {             
            return PWR_POD.GetList(p => (p.PWR_ID ?? -1) == pwrId);           
        }

        public static PWR_POD ProposedWaterRightToPoint(int pwrPodId) ///the ID in the PWR_POD table (should return a single record)
        {             
            return PWR_POD.Get(i => i.ID == pwrPodId);             
        }*/

    }
}
