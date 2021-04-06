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
        public List<VAwsSubbasin> SubbasinList { get; set; }
        public List<CD_AMA_INA> AmaIna { get; set; }
        public List<VAwsCountyBasin> CountyBasinList { get; set; }//TODO change this to adwr_admin schema and namespace

        public GeoBoundaryViewModel()
        {
            this.SubbasinList = VAwsSubbasin.GetAll();
            this.AmaIna = CD_AMA_INA.GetAll();
            this.CountyBasinList = VAwsCountyBasin.GetAll();
        }
    }
}