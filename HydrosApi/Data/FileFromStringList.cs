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


        public static List<FileFromStringList> GetFileFromStringList(string stringList, char[] delimiters=null)
        {
            if (stringList == null)
                return null;

            if (Regex.Replace(stringList, @"\s+", "") == "")
                return null;

            if(delimiters==null)
                delimiters= new[] { ','};

           var dashDot = new[] { '-','.' };

            var list = stringList.Split(delimiters).ToList();
            var info = (from s in list
                             select new
                             {
                                 hasDash = s.IndexOf("-") > -1,
                                 hasDot = s.IndexOf(".") > -1,
                                 parts = s.Split(dashDot)
                             }).Select(p => new FileFromStringList()
                             {
                                 Program = p.hasDash ? (p.parts[0]).Replace(" ", "") : "",
                                 FileNo = (p.hasDash ? p.parts[1] : p.parts[0]).Replace(" ","") ,
                                 NumericFileNo = int.Parse((p.hasDash ? p.parts[1] : p.parts[0]).Replace(" ", "") ?? "0"),
                                 SeqNo = p.hasDot ? (p.hasDash ? p.parts[2] : p.parts[1]).Replace(" ", "") : ""

                             }).Distinct().ToList();

            return info;

        }
    }
}