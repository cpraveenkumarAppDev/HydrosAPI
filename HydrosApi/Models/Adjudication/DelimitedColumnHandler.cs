namespace HydrosApi
{
    
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;


    public class DelimitedColumnHandler
    {

        public string PCC { get; set; }

        public string FileType { get; set; }

        public string FileNumber { get; set; }

        public static List<DelimitedColumnHandler> FileInformation(string columnValue)
        {
            var rgx = new Regex(@"[^0-9-,]");

            var delimitedColumnHandler = new List<DelimitedColumnHandler>();
            
            columnValue = rgx.Replace(columnValue, "");

            var splitList = (from f in columnValue.Split(',')
                             select new
                             {
                                 PCC = f,
                                 FileType = f.IndexOf("-") > -1 ? rgx.Replace(f, "").Split('-')[0] : "00",
                                 FileNumber = f.IndexOf("-") > -1 ? rgx.Replace(f, "").Split('-')[1] : rgx.Replace(f, "")

                             }).Distinct();
            
             
            foreach(var s in splitList)
            {

                var columns = new DelimitedColumnHandler();

                columns.PCC = s.PCC;
                columns.FileType = s.FileType;
                columns.FileNumber = s.FileNumber;
                delimitedColumnHandler.Add(columns);
            }

            return delimitedColumnHandler;
        }
    }
}