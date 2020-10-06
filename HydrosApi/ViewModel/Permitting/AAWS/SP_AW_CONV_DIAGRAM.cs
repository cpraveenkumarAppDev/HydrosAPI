using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HydrosApi.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace HydrosApi.ViewModel
{
    public class SP_AW_CONV_DIAGRAM : Repository<SP_AW_CONV_DIAGRAM>
    {
        public virtual int? CYCLE_LEVEL { get; set; }

        public virtual string PARENT_PCC { get; set; }
        public virtual string CHILD_PCC { get; set; }
        public virtual int? NUMLOTS { get; set; }
        public virtual float? ACREFEET { get; set; }
        public virtual DateTime? DTISSUED { get; set; }


        public virtual string SEARCH_FILE_NO { get; set; }

        public virtual string ORIGINAL_FILE_NO { get; set; }
        public virtual string PRIMARY_PCC { get; set; }


        
        public static List<SP_AW_CONV_DIAGRAM> ConveyanceDiagram(string pcc)
        {
            var parameter = new List<OracleParameter>();
            parameter.Add(new OracleParameter("p_pcc", pcc));

            var emptyCursor = new OracleParameter("conveyanceCur", OracleDbType.RefCursor);
            emptyCursor.Direction = ParameterDirection.ReturnValue;
            emptyCursor.IsNullable = true;
            parameter.Add(emptyCursor);

            var command = "BEGIN aws.sp_aw_conv_diagram(:p_pcc, :conveyanceCur); end;";

            return SP_AW_CONV_DIAGRAM.ExecuteStoredProcedure(command, parameter.ToArray());
        }
    }
}