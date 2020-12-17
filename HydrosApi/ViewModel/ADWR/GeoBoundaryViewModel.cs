using HydrosApi.Models;
using HydrosApi.Models.Permitting.AAWS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HydrosApi.ViewModel.ADWR
{
    public class GeoBoundaryViewModel
    {
        public List<V_AWS_SUBBAS> SubbasinList { get; set; }
        public List<CD_AMA_INA> AmaIna { get; set; }
        public List<V_AWS_COUNTY_BASIN> CountyBasinList { get; set; }//TODO change this to adwr_admin schema and namespace

        public GeoBoundaryViewModel()
        {
            this.SubbasinList = V_AWS_SUBBAS.GetAll();
            this.AmaIna = CD_AMA_INA.GetAll();
            this.CountyBasinList = V_AWS_COUNTY_BASIN.GetAll();
        }
    }
}