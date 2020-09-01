using AdwrApi.Controllers.Permitting.AAWS;
using HydrosApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HydrosApi.Controllers.Permitting.AAWS
{

    public class AWS_OVER_VIEW 
    {
        public string PrimaryProviderName { get; set; }
        public string SecondaryProviderName { get; set; }
        public bool Application_Form_Complete { get; set; }
        public bool Correct_Fees_Received { get; set; }
        public bool Application_Signed { get; set; }
        public bool Physical_Availability { get; set; }
        public bool Hydrology { get; set; }
        public bool Continous_Availability { get; set; }
        public bool Legal_Availability { get; set; }
        public bool Consistency_with_Management_Plan { get; set; }
        public bool Consistency_with_Management_Goal { get; set; }
        public bool Water_Quality { get; set; }
        public bool Financial_Capability { get; set; }
        public bool Ownership_Documents { get; set; }
        public bool Demand_Calculator { get; set; }
        public bool Other { get; set; }
        public string Water_Provider_Name { get; set; }
        public DateTime? Date_Reviewed { get; set; }
        public DateTime? Date_Issued { get; set; }
        public string PWS_ID_Number { get; set; }
        public string Subbasin { get; set; }
        public DateTime? Date_Accepted { get; set; }
        public string AMA { get; set; }
        public DateTime? Date_Declared_Complete { get; set; }
        public DateTime? Complete_Correct { get; set; }
        public DateTime? First_Notice { get; set; }
        public DateTime? Second_Notice { get; set; }
        public DateTime? Final_Date { get; set; }
    }
}