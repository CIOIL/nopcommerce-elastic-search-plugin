using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Search.Plugin.GSA.Model
{
    public class ConfigModel : BaseNopModel
    {


        public ConfigModel()
        {
            AcessTypes = new List<SelectListItem>();
            SortFormats = new List<SelectListItem>();
            OutputOptions = new List<SelectListItem>();

            AcessTypes.Add(new SelectListItem { Text = "public", Selected = true, Value = "p" });
            AcessTypes.Add(new SelectListItem { Text = "private", Selected = false, Value = "s" });
            AcessTypes.Add(new SelectListItem { Text = "All", Selected = false, Value = "a" });

            SortFormats.Add(new SelectListItem { Text = "Sort By Relevance", Selected = false, Value = "Default" });
            SortFormats.Add(new SelectListItem { Text = "Sort By Date", Selected = false, Value = "Date" });

            OutputOptions.Add(new SelectListItem { Text = "xml_no_dtd", Selected = false, Value = "xml_no_dtd" });
            OutputOptions.Add(new SelectListItem { Text = "xml", Selected = false, Value = "xml" });
        }

        [Required]
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$",ErrorMessage = "Nop.Search.Plugin.GSA.GsaHostError")]
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.GsaHost")]
        public string GsaHost { get; set; }
       
        [Required]
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.Collection")]
        [UIHint("Nop.Search.Plugin.GSA.Collection.Hint")]
        public string Collection { get; set; }
        [Required]
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.FrontEndClient")]
        public string FrontEndClient { get; set; }

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.AccessType")]
        public string AccessType { get; set; }  // = "a";

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.OutputOption")]
        public string OutputOption { get; set; }    // = "xml";


        [NopResourceDisplayName("Nop.Search.Plugin.GSA.MaxSearchResults")]
        public int MaxSearchResults { get; set; }   // = 1000;

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.SortFormat")]
        public string SortFormat { get; set; }  // = "Default"; /* Sort By Relevance */

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.ResultsPerPage")]
        public int ResultsPerPage { get; set; } // = 10;

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.ImageHost")]
        [Required]
        public string ImageHost { get; set; } // info.goisrael.com

        public IList<SelectListItem> AcessTypes { get; set; }
        public IList<SelectListItem> SortFormats { get; set; }
        public IList<SelectListItem> OutputOptions { get; set; }

        [NopResourceDisplayName("Nop.Search.Plugin.GSA.UseGsa")]
        public bool isGsa { get; set; }
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.UseElastic")]
        public bool IsElastic { get; set; }

        /* Elastic Search Config*/
        [Required]
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.ElasticHost")]
        public string ElasticHost { get; set; }
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.SearchIndex")]
        public string SearchIndex { get; set; }
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.SearchKey")]
        public string SearchKey { get; set; }
        [NopResourceDisplayName("Nop.Search.Plugin.GSA.ResultsPerPage")]
        public int EResultsPerPage { get; set; }
        public partial class CollectionMapping : BaseNopModel
        {
            public string LanguageCulture { get; set; }
            public string Collection { get; set; }
            public int Id { get; set; }
        }
    }
}
