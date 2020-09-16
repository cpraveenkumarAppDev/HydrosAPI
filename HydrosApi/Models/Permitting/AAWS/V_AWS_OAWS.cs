namespace HydrosApi.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    [Table("AWS.V_AWS_OAWS")]
    public class V_AWS_OAWS : Repository<V_AWS_OAWS>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public V_AWS_OAWS()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("WRFID")]
        public int? WaterRightFacilityId { get; set; }

        [Column("PCC")]
        public string ProgramCertificateConveyance { get; set; }

        [Column("APP_COMPLETE")]
        public string APP_COMPLETE { get; set; }

        [Column("APP_ACCEPTED")]
        public string APP_ACCEPTED { get; set; }

        [Column("CORRECT_DT")]
        public DateTime? CORRECT_DT { get; set; }

        //hydro_study_contracts varchar2(1)
        //impact_analysis_cd_submit varchar2(1)
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
        //post_website date
        //fee_refunded varchar2(4000)
        //refund_amount number
        //refund_date date
        //fiscal_year number
        //protests varchar2(4000)
        //hearing varchar2(4000)
        //conveying_file varchar2(4000)
        //original_file varchar2(4000)
        //original_date date
        //plat_authority_code varchar2(4)
        //platting_authority varchar2(80)
        //num_active_days number
        //update_study varchar2(1)
        //wtr_inadequacy varchar2(1)
        //performance_bond varchar2(1)
        //constructed_infrastruct varchar2(1)
        //assign_part_typea varchar2(1)
        //retain_part_typea varchar2(1)
        //assign_retain_part_typea varchar2(1)
        //assign_full_typea varchar2(1)
        //gw_serv_existing_right varchar2(1)
        //gw_serv_pending_right varchar2(1)
        //cap_serv_ltc varchar2(1)
        //sw_serv_sw_right varchar2(1)
        //effl_serv_municipal_prov varchar2(1)
        //type1_gfr varchar2(1)

    }
}