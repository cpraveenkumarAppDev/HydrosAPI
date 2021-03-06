namespace HydrosApi.Data
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Text.RegularExpressions;
    using System;
    using System.Globalization;
    using Models.ADWR;

    public partial class QueryResult : Repository<QueryResult>
    {
        /// <summary>
        /// The QueryResult Class
        /// </summary>
        /// <remarks>
        /// <para>       
        /// A collection of utilities for:
        /// 
        /// Running custom queries
        /// Getting an ID from the sequencer
        /// rgr_rpt
        /// Returing exceptions
        /// </para>      
        /// </remarks>

        public static List<dynamic> RunAnyQuery(string sql)
        {
            return RunAnyQuery(sql, new OracleContext());
        }

        /// <summary>
        /// Use this when you need to run an inline SQL query.
        /// </summary>
        /// <param name="sql">Provide an SQL Statement (ideally you should only use this for select statements) </param>
        /// <param name="ctx">Tge database context for the query</param>
        /// <returns>Returns the result with the database column names or aliases provided in the query</returns>
        /// <remarks>
        /// <para>
        /// Note: A header row is created with a rowindex of -1 that describes the data types for each column.                    
        /// You can add functionality to use the data types header row to create fields above each column with search criteria options, 
        /// for example, above a date column, you may want to allow a user to select a date range with a calendar popup.
        /// This was a proof of concept for recreating ARM and reusing the existing dataabase structure
        /// </para>      
        /// </remarks>

        public static List<dynamic> RunAnyQuery(string sql, DbContext ctx=null, bool addHeader=true)
        {
            var result = new List<dynamic>();

            if (ctx == null)
            {
                ctx = new OracleContext();
            }

            using (ctx)
             
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
                           
                            if (rowIndex < 2 && addHeader==true)
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

                        if (rowIndex == 1 && addHeader==true)
                        {
                            result.Add(header);
                        }
                        result.Add(data);
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Use this when you need to run an inline SQL query.
        /// </summary>
        /// <param name="id">Use the existing oracle function, get_pcc </param>
        /// <returns>Scalar value PCC</returns>
        
        public static string RgrRptGet(int id)
        {
            using (var ctx = new OracleContext())
            using (var cmd = ctx.Database.Connection.CreateCommand())
            {
                ctx.Database.Connection.Open();
                cmd.CommandText = string.Format("select rgr_rpt.get_pcc({0}) as pcc from dual ", id);
                var pcc= cmd.ExecuteScalar();

                if (pcc == null)
                    return null;

                return pcc.ToString();
            }
        }


        public static int? RgrRptSurface(string pcc)
        {
            if (pcc == null)
                return null;

            Regex regex = new Regex(@"(\d{2})\D?(\d{6})\D?(\d{1,4})");
            pcc = regex.Replace(pcc, "$1-$2.$3");

            if(pcc.Length < 10)
            {
                return null;
            }

            using (var ctx = new OracleContext())
            using (var cmd = ctx.Database.Connection.CreateCommand())
            {
                ctx.Database.Connection.Open();
                cmd.CommandText = string.Format("select t.art_idno id from ADWR.SW_APPL_REGRY t " +
                                                " where t.art_program || '-' || t.art_appli_no || '.' || t.art_convy_no = '{0}'", pcc);
                var id = cmd.ExecuteScalar();
                if (id != null)
                    return Convert.ToInt32(id);
                else
                    return null;

            }

        }

        /// <summary>
        /// Use this when you need to run an inline SQL query.
        /// </summary>
        /// <param name="pcc">Use the existing oracle function, get_wrf_id </param>
        /// <returns>Scalar value wrf_id</returns>

        public static int? RgrRptGet(string pcc)
        {
            try
            {
                if (pcc == null)
                    return null;               

                Regex regex = new Regex(@"(\d{2})\D?(\d{6})\D?(\d{4})");
                pcc = regex.Replace(pcc, "$1-$2.$3");

                if (pcc.Length != 14)
                    return null;


                using (var ctx = new OracleContext())
                using (var cmd = ctx.Database.Connection.CreateCommand())
                {
                    ctx.Database.Connection.Open();
                    cmd.CommandText = string.Format("select rgr_rpt.get_wrf_id('{0}') as WaterRightFacilityId from dual", pcc);
                    var WaterRightFacilityId = cmd.ExecuteScalar();

                    if (WaterRightFacilityId == null)
                        return null;

                    return int.Parse(WaterRightFacilityId.ToString());
                }
            }
            catch(Exception exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Use this when you need to run an inline SQL query.
        /// </summary>      
        /// <returns>Return a new OracleID sequence number</returns>
        public static int? NextVal()
        {
            
            using (var ctx = new OracleContext())
            using (var cmd = ctx.Database.Connection.CreateCommand())
            {
                ctx.Database.Connection.Open();
                cmd.CommandText = "select rgr_id_seq.nextval as WaterRightFacilityId from dual";
                var WaterRightFacilityId = cmd.ExecuteScalar();

                if (WaterRightFacilityId == null)
                    return null;

                return int.Parse(WaterRightFacilityId.ToString());
            }
        }

        public static string BundleExceptions(Exception exception)
        {
            string fullException = exception.Message;
            if (exception.InnerException != null)
            {
                fullException += BundleExceptions(exception.InnerException);
            }

            return fullException;
        }


        public static string TitleFormat(string val)
        {
            TextInfo ti = new CultureInfo("en-US", true).TextInfo;

            if (val != null)
            {
                return ti.ToTitleCase(val.ToLower());

            }
            return null;
        }

      
         
        public static WaterRightFacility GetWrfRecord(string id)
        {
            try
            {

               // if (id == null)
                   // return BadRequest("Please enter a PCC or WaterRightFacilityId");

                Regex regex = new Regex(@"([1-9][0-9])[^0-9]?([0-9]{6})[^0-9]?([0-9]{4})");
                var pcc = regex.Replace(id, "$1-$2.$3");

                if (pcc.Length == 14)
                {
                    var program = regex.Replace(pcc, "$1");
                    var certificate = regex.Replace(pcc, "$2");
                    var conveyance = regex.Replace(pcc, "$3");
                    return WaterRightFacility.Get(w => w.Program == program && w.Certificate == certificate && w.Conveyance == conveyance);
                }
                else
                {
                    int wrfId;
                    bool validId = Int32.TryParse(id, out wrfId);

                    if (validId)
                    {
                       return WaterRightFacility.Get(w => w.Id == wrfId);
                    }

                }
                // var info = new GeoBoundaryViewModel();
                //return Ok(info);

                return null;
            }
            catch (Exception exception)
            {
                return null;
            }


        }
      
    }
}