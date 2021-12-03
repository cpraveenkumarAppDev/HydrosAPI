
namespace HydrosApi.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Dynamic;
    using System.Configuration;


    public class MapUriConfig
    {
        public string appId => ConfigurationManager.AppSettings["appId"]; //may change in the future
        public string portalUrl => ConfigurationManager.AppSettings["portalUrl"]; //changes for test and production
        public string logo { get; set; }
        public string agency => "Arizona Department of Water Resources";
        public string title => "AIS";
        public string tabName => "AIS";
        public bool active => true;

        public List<string> bufferSources { get; set; }

        public List<SearchSources> searchSources => setSearchSources();
        public List<Query> query => setQuery();

        //changes for test and production
        public Dictionary<string, string> layers => ConfigurationManager.AppSettings.AllKeys
            .Where(key => key.StartsWith("gisAdjUrl"))
            .Select(key => new { key = key.Split('-')[1], value = ConfigurationManager.AppSettings[key] })
            .ToDictionary(x => x.key, x => x.value);
                                                       
        public Dictionary<string, string> links =>  new Dictionary<string, string> { 
            { "detailsGWSI", "http://gisweb.azwater.gov/GWSI/Detail.aspx?SiteID=" },
            { "detailsWellReg", "http://gisweb.azwater.gov/WellRegistry/Detail.aspx?RegID=" },
            { "imagedRecordWellReg", "https://dwrsrvc.azwater.gov/dsapi/api/WellRegDoc/AllDocs/?registryNum=" },
            { "imagedRecordWells35", "https://dwrsrvc.azwater.gov/dsapi/api/WellRegDoc/getwell35docs/?registry="},
            { "imagedRecordSOC", "https://dwrsrvc.azwater.gov/dsapi/api/SOC/?fileNumber="}};

        public List<SearchSources> setSearchSources()
        {
            var search = new List<SearchSources>();
            search.Add(new SearchSources() { title = "Watershed File Report", searchFields = new[] { "DWR_ID" }.ToList() });
            search.Add(new SearchSources() { title = "Point of Diversion", searchFields = new[] { "DWR_ID" }.ToList() });
            search.Add(new SearchSources() { title = "County", searchFields = new[] { "NAME" }.ToList() });
            search.Add(new SearchSources() { title = "ArcGIS World Geocoding Service" });
            return search;
        }

        public List<Query> setQuery()
        {
            var q = new List<Query>();
            q.Add(new Query() { title = "Watershed File Report", urlKey = "wfr-layer", configKey ="wfrUrl"});
            q.Add(new Query() { title = "Domestic", urlKey = "domestic-layer", configKey= "domesticUrl" });
            q.Add(new Query() { title = "Point of Diversion", urlKey = "pod-layer", configKey= "podsUrl" });
            q.Add(new Query() { title = "Irrigation", urlKey = "irrigation-layer", configKey= "irrigationUrl" });
            q.Add(new Query() { title = "StockWater", urlKey = "stockwater-layer", configKey = "stockWaterUrl" });
            q.Add(new Query() { title = "Stockpond", urlKey = "stock-layer", configKey = "stockPondUrl" });
            q.Add(new Query() { title = "Municipal", urlKey = "municipal-layer", configKey = "municipalUrl" });
            q.Add(new Query() { title = "Industrial", urlKey = "industrial-layer", configKey = "industrialUrl" });

            return q;
        }
    }

    public class SearchSources
    {
        public string title { get; set; }
        public List<string> searchFields { get; set; }
    }
    public class Query
    {
        public string title { get; set; }
        public string urlKey { get; set; }

        public string configKey { get; set; }
    }
}