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

        public string FileNo6 { get => FileNo?.PadLeft(6, '0'); set => FileNo6 = value; }

        public int NumericFileNo { get; set; }


        public bool NumericPart { get; set; }

        public string SeqNo { get; set; }

        public string Error { get; set; }

        public string UserValue { get; set; }

        public static List<FileFromStringList> GetFileFromStringList(string stringList, char[] delimiters=null)
        {
            

            if (stringList == null)
                return null;

           

            stringList = Regex.Replace(stringList, @"[^0-9A-Za-z\;\,\.\-]", "");

            if (stringList == "")
                return null;

            if(delimiters==null)
                delimiters= new[] { ','};

           var dashDot = new[] { '-','.' };
            var notNumber = @"[^0-9]";

         

            try
            {
                int number;

                var list = stringList.Split(delimiters).ToList();
                var stringInfo = (from s in list
                                  select new
                                  {
                                      hasDash = s.IndexOf("-") > -1,
                                      hasDot = s.IndexOf(".") > -1,
                                      parts = s.Split(dashDot),
                                      val = s
                                  }).ToList();
                
                
                var info=stringInfo.Select(p => new FileFromStringList()
                            {
                                Program = p.parts != null && p.parts.Length > 1 ? p.parts[0] : null,
                                FileNo = p.parts != null ? p.parts.Length > 1 ? p.parts[1] : p.parts[0] : null,
                                NumericPart = int.TryParse(p.parts != null ? p.parts.Length > 1 ? p.parts[1] : p.parts[0] : "0", out number),
                                NumericFileNo=number,
                                SeqNo = p.parts != null && p.parts.Length > 2 ? p.parts[2] : null,
                                UserValue = p.val
                            }).Distinct().ToList();

                return info;
            }
            catch(Exception ex)
            {
                var err = new FileFromStringList();
                var listErr = new List<FileFromStringList>();

                err.Error = ex.Message;
                
                listErr.Add(err);

                return listErr;
            }

        }
    }
}