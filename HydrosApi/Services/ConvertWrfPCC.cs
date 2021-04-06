using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.Services
{
    public class ConvertWrfPCC
    {
        private OracleContext context;

        public ConvertWrfPCC(OracleContext _context)
        {
            this.context = _context;
        }

        /// <summary>
        /// Convert an integer WRF id to a PCC
        /// </summary>
        /// <param name="wrf"></param>
        /// <returns></returns>
        public PCC ConvertWrfToPCC(int wrf)
        {
            var newPCC = new PCC(this.context.WTR_RIGHT_FACILITY.Where(x => x.Id == wrf).FirstOrDefault().PCC);
            return newPCC;
        }

        /// <summary>
        /// Convert a string PCC to a wrf
        /// </summary>
        /// <param name="PCC">string PCC representation</param>
        /// <returns></returns>
        public int ConvertPCCToWrf(string inPCC)
        {
            var pccObject = new PCC(inPCC);
            var found = this.context.WTR_RIGHT_FACILITY.Where(x => x.PCC == pccObject.ToString()).FirstOrDefault().Id;
            return found;
        }

        public int ConvertPCCToWrf(PCC PCC)
        {
            var found = this.context.WTR_RIGHT_FACILITY
                .Where(x => x.Program == PCC.Program && x.Certificate == PCC.Certificate && x.Conveyance == PCC.Conveyance)
                .FirstOrDefault();
            if (found != null)
                return found.Id;
            else
                return 0;
        }
    }
}