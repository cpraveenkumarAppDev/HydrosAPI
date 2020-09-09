using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdwrApi.Models.Permitting.AAWS
{
    [Table("AWS.AW_VERIFIED_OAWS")]
    public class AW_VERIFIED_OAWS
    {
        [Key]
        [Column("WRF_ID")]
        public int WRF_ID { get; set; }

        [Column("APP_COMPLETE")]
        public string APP_COMPLETE { get; set; }

        [Column("APP_ACCEPTED")]
        public string APP_ACCEPTED { get; set; }

        //hydro_study_contracts varchar2(1)
        //contracts_attached varchar2(1)
        //plat_submitted varchar2(1)
        //fees_included varchar2(1)
        //titledeed_verified varchar2(1)
        //grd_owner_verified varchar2(1)
        //owner_name_match varchar2(1)
        //construction_assurance varchar2(1)
        //pot_del_sys varchar2(1)
        //pot_stor_fac varchar2(1)
        //pot_treat_fac varchar2(1)
        //offsite_pipeline varchar2(1)
        //effl_del_sys varchar2(1)
        //effl_treat_fac varchar2(1)
        //other_infrastruct varchar2(1)
        //other_infrastruct_desc varchar2(500)
        //signed_noi varchar2(1)
        //signed_app varchar2(1)
        //plat_change varchar2(1)
        //material_plat_change varchar2(1)
        //plat_active varchar2(1)
        //gw_serv_existing_right varchar2(1)
        //gw_serv_pending_right varchar2(1)
        //cap_serv_ltc varchar2(1)
        //sw_serv_sw_right varchar2(1)
        //effl_serv_municipal_prov varchar2(1)
        //type1_gfr varchar2(1)
        //assign_part_typea varchar2(1)
        //retain_part_typea varchar2(1)
        //assign_retain_part_typea varchar2(1)
        //assign_full_typea varchar2(1)
        //createby varchar2(30)
        //createdt date
        //updateby varchar2(30)
        //updatedt date
        //impact_analysis_cd_submit varchar2(1)

        //[Column("CMT_DATE")]
        //public DateTime CMT_DATE { get; set; }

    }
}