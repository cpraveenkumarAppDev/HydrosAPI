using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HydrosApi.Services
{
    public class PCC
    {
        public string Program { get; set; }
        public string Certificate { get; set; }
        public string Conveyance { get; set; }

        public PCC(string pcc)
        {
            if(pcc.Contains('.') || pcc.Contains('-'))
            {
                Regex regex = new Regex(@"(\d{2})\D(\d{6})\D(\d{4})");
                var match = regex.Match(pcc);
                var groups = match.Groups;
                if(groups.Count > 0)
                {
                    this.Program = groups[1] != null ? groups[1].Value : null;
                    this.Certificate = groups[2] != null ? groups[2].Value : null;
                    this.Conveyance = groups[3] != null ? groups[3].Value : null;
                }
            }
            else if(pcc.Length == 12)
            {
                try
                {
                    this.Program = pcc.Substring(0, 2);
                    this.Certificate = pcc.Substring(2, 6);
                    this.Conveyance = pcc.Substring(8);
                }
                catch(Exception exception)
                {
                    //log exception
                    throw exception;
                }
            }
        }
    }
}