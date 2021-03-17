using HydrosApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.Services
{
    public class ConveyanceInfo : IDisposable
    {
        private OracleContext context;

        public ConveyanceInfo()
        {
            this.context = new OracleContext();
        }

        public ConveyanceInfo(OracleContext _context)
        {
            this.context = _context;
        }

        /// <summary>
        /// Get the parent program 28 PCC which conveyed to a 42 by matching the Certificate
        /// </summary>
        /// <param name="pcc"></param>
        /// <returns></returns>
        public PCC Get42Parent(PCC pcc)
        {
            if(pcc.Program != "42")
            {
                throw new Exception($"pcc parameter must be a 42, provided: {pcc}");
            }
            var parent28 = this.context.WTR_RIGHT_FACILITY.Where(x => x.Program == "28" && x.Certificate == pcc.Certificate);
            //var count42 = this.context.WTR_RIGHT_FACILITY.Count(x => x.Program == "42" && x.Certificate == pcc.Certificate);
            return new PCC(parent28.FirstOrDefault().PCC);
        }

        /// <summary>
        /// Get the count of the number of conveyances from a 42 parent
        /// </summary>
        public int Get42ConveyanceCount(PCC pcc28)
        {
            if (pcc28.Program != "28")
                throw new Exception($"pcc parameter must be a 28 (parent of 42), provided {pcc28}");
            var converter = new ConvertWrfPCC(this.context);
            var found28WRF = converter.ConvertPCCToWrf(pcc28);
            var foundEntities = this.context.WRF_WRF.Where(x => x.WRF_ID_FROM == found28WRF).Select(x => x.WRF_ID_TO).ToList();
            var PCCList = foundEntities.Select(x => converter.ConvertWrfToPCC(x)).ToList();
            return PCCList.Count(x => x.Program == "42");
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}