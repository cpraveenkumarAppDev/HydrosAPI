using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.Models.Adjudication
{
    public class AISPODS
    {
            public string DWR_ID { get; set; }
            public string TYPE { get; set; }
            public string SEQ { get; set; }
            public string NAME { get; set; }
            public string SHARED { get; set; }
            public string SOC { get; set; }
            public string BOC { get; set; }
            public string LAND_OWNER { get; set; }
            public int ID { get; set; }
            //public int? PARENT_ID { get; set; }
            public int? POD_ID { get; set; }
    }
    public class AISSURFACEWATER
    {
        public string LINK { get; set; }
        public string SWR_NUMBER { get; set; }
        public string STATUS { get; set; }
        public string NAME { get; set; }
        public string USE { get; set; }
        public DateTime FILE_DATE { get; set; }
        public string BOC { get; set; }
        public string LAND_OWNER { get; set; }
        public int ID { get; set; }
        //public int? PARENT_ID { get; set; }
        public int? POD_ID { get; set; }
    }
}