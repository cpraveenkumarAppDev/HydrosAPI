namespace HydrosApi.Data
{
    using System.Collections.Generic;

    public partial class QueryResult : Repository<QueryResult>
    {
        /// <summary>
        /// Use this when you need to run an inline SQL query.
        /// </summary>
        /// <param name="sql">Provide an SQL Statement (ideally you should only use this for select statements) </param>
        /// <returns>Returns the result with the database column names or aliases provided in the query</returns>
        /// <remarks>
        /// <para>
        /// Note: A header row is created with a rowindex of -1 that describes the data types for each column.                    
        /// You can add functionality to use the data types header row to create fields above each column with search criteria options, 
        /// for example, above a date column, you may want to allow a user to select a date range with a calendar popup.
        /// This was a proof of concept for recreating ARM and reusing the existing dataabase structure
        /// </para>      
        /// </remarks>

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
                     
                    while (reader.Read())
                    {
                        var data = new Dictionary<string, object>();

                        //for a header row with the names of the fields and the data types
                        var header = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            //create a header row with a rowindex of -1 that describes the data types of each column
                           
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