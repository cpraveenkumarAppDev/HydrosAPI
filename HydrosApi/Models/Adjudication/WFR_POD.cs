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
    using HydrosApi.Models.Adjudication;

    [Table("ADJ_INV.WFR_POD")]

    public partial class WFR_POD : AdwrRepository<WFR_POD>
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? WFR_ID { get; set; }

        public int? POD_ID { get; set; }

        [StringLength(20)]
        public string CREATEBY { get; set; }

        public DateTime? CREATEDT { get; set; }

        [StringLength(20)]
        public string UPDATEBY { get; set; }

        public DateTime? UPDATEDT { get; set; }

        [NotMapped]
        public virtual WATERSHED_FILE_REPORT WATERSHED_FILE_REPORT { get; set; }
        [NotMapped]
        public string DWR_ID {
            get
            {
                var PodView = POINT_OF_DIVERSION_VIEW.Get(p => p.ID == this.POD_ID);
                var pod = POINT_OF_DIVERSION.Get(p => p.OBJECTID == PodView.OBJECTID);
                if (pod == null)
                {
                    return null;
                }
                else
                {
                    //add vars in pod class
                    //pod.WFR_POD_ID = this.ID;
                    //pod.WFR_ID = this.WFR_ID;
                    return pod.DWR_ID;
                }
            }

            set
            {
                this.DWR_ID = value;
            }
        }

        [NotMapped]
        public AISPODS PointOfDiversion
        {
            get
            {
                var PodView = POINT_OF_DIVERSION_VIEW.Get(p => p.ID == this.POD_ID);
                var pod = POINT_OF_DIVERSION.Get(p => p.OBJECTID == PodView.OBJECTID);
                if (pod == null)
                {
                    return new AISPODS { };
                }
                else
                {
                    var newPOD = new AISPODS
                    {
                        DWR_ID = pod.DWR_ID,
                        TYPE = pod.Type,
                        SEQ = pod.POD_SEQ,
                        NAME = pod.Name,
                        SHARED = pod.SHARED_POD,
                        SOC = pod.SOC,
                        BOC = pod.BOC,
                        LAND_OWNER = pod.LAND_OWNER,
                        ID = this.ID,
                        //PARENT_ID = this.WFR_ID,
                        POD_ID = this.POD_ID
                    };

                    return newPOD;
                }
            }

            set
            {
                this.PointOfDiversion = value;
            }

        }

    }

}

