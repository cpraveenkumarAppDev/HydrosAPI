namespace HydrosApi.Data
{

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    public partial class QueryResult : Repository<QueryResult>
    {       
        public static List<dynamic> RunAnyQuery(string sql)
        {
            var result = new List<dynamic>();

            using (var ctx = new OracleContext())
            using (var cmd = ctx.Database.Connection.CreateCommand())
            {
                ctx.Database.Connection.Open();
                cmd.CommandText = sql;

                using (var reader = cmd.ExecuteReader())
                {
                    var rowIndex = 0;

                    //var schemaTable = reader.GetSchemaTable();
                    while (reader.Read())
                    {
                        var data = new Dictionary<string, object>();

                        //for a header row with the names of the fields and the data types
                        var header = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            //create a header row with a rowindex of -1 and the data types
                            //use the data types to determine what type of field to use for 
                            //parameters or search.  For example search by a daterange and 
                            //provide a calendar popup

                            if (rowIndex < 2)
                            {
                                if (i == 0)
                                {
                                    header.Add("RowIndex", -1);
                                }
                                header.Add(reader.GetName(i), reader.GetFieldType(i).ToString());
                            }

                            if (i == 0)
                            {
                                data.Add("RowIndex", rowIndex);
                                rowIndex++;
                            }

                            var value = reader.GetValue(i);

                            //when the type is DateTime, the value must be converted to a string
                            if (reader.GetFieldType(i).ToString() == "System.DateTime")
                            {
                                value = value.ToString();
                            }

                            data.Add(reader.GetName(i), value);
                        }

                        if (rowIndex == 1)
                        {
                            result.Add(header);
                        }
                        result.Add(data);
                    }
                }
            }

            return result;
        }


    }
}