using Nest;
using Nop.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Search.Plugin.GSA.Model
{
    public class ElasticSearchResultModel
    {
        public ElasticSearchResultModel()
        {
            Data = new ElasticResult();
            NopProductsId = new List<int>();
        }
        public string SearchKeyword { get; set; }
        public List<int> NopProductsId { get; set; }
        public int TotalResult { get; set; }
        public int TotalPages { get; set; }
        public int NopSearchResults { get; set; }
        public TermsAggregate<string> ElastigRequestResult { get; set; }
        public const string TOP_UNIQUE_RESULT= "top_unique_result";
        public ElasticResult Data { get; set; }

        public partial class ElasticResult
        {

            public ElasticResult()
            {
                this.PagingFilteringContext = new CatalogPagingFilteringModel();
                this.Results = new List<ProductOverviewModel>();
            }
            public CatalogPagingFilteringModel PagingFilteringContext { get; private set; }
            public List<ProductOverviewModel> Results { get; set; }
            public int TotalResults { get; set; }
        }


    }
}
