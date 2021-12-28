namespace HydrosApi.Data
{

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Models;

    public class FileFromStringList
    {
        public string Program { get; set; }
        public string FileNo { get; set; }

        public int NumericFileNo { get; set; }

        public string SeqNo { get; set; }

        public string Error { get; set; }

        public static List<FileFromStringList> GetFileFromStringList(string stringList, char[] delimiters=null)
        {
            if (stringList == null)
                return null;


            stringList = Regex.Replace(stringList, @"[^0-9\;\,\.\-]", "");
            if (stringList == "")
                return null;

            if(delimiters==null)
                delimiters= new[] { ','};

           var dashDot = new[] { '-','.' };

            try
            {



                var list = stringList.Split(delimiters).ToList();
                var info = (from s in list
                            select new
                            {
                                hasDash = s.IndexOf("-") > -1,
                                hasDot = s.IndexOf(".") > -1,
                                parts = s.Split(dashDot)
                            }).Select(p => new FileFromStringList()
                            {
                                Program = p.parts != null && p.parts.Length > 1 ? p.parts[0] : null,
                                FileNo = p.parts != null ? p.parts.Length > 1 ? p.parts[1] : p.parts[0] : null,
                                NumericFileNo = int.Parse(p.parts != null ? p.parts.Length > 1 ? p.parts[1] : p.parts[0] : "0"),
                                SeqNo = p.parts != null && p.parts.Length > 2 ? p.parts[2] : null

                            }).Distinct().ToList();

                return info;
            }
            catch(Exception ex)
            {
                var err = new FileFromStringList();
                err.Error = ex.Message;
                var listErr = new List<FileFromStringList>();
                listErr.Add(err);

                return listErr;
            }

        }
    }
}