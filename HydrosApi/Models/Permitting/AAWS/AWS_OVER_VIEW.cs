using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.Controllers.Permitting.AAWS
{
    public class AWS_OVER_VIEW
    {
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
        public DateTime Date_Reviewed { get; set; }
        public string PWS_ID_Number { get; set; }
        public string Subbasin { get; set; }
        public DateTime Date_Accepted { get; set; }
        public string AMA { get; set; }
        public DateTime Date_Issued { get; set; }
    }
}