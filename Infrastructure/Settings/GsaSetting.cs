using Nop.Core.Configuration;

namespace Nop.Search.Plugin.GSA.Infrastructure.Settings
{
    //Gsa Configuration Settings
    /*Con*/
    public class GsaSetting : ISettings
    {
        public string GsaHost { get; set; }
        public string Collection { get; set; }
        public string FrontEndClient { get; set; }
        public string AccessType { get; set; }  // = "a";
        public string OutputOption { get; set; }    // = "xml";
        public int MaxSearchResults { get; set; }   // = 1000;
        public string SortFormat { get; set; }  // = "Default"; /* Sort By Relevance */
        public int ResultsPerPage { get; set; } // = 10;
        public string ImageHost { get; set; }
        public bool isGsa { get; set; }
        public bool IsElastic { get; set; }
        public string ElasticHost { get; set; }
        public string SearchIndex { get; set; }
        public string SearchKey { get; set; }

        public int EResultsPerPage { get; set; }
    }
}
