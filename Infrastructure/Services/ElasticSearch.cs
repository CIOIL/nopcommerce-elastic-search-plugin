using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using Nop.Search.Plugin.GSA.Model;
using Nop.Web.Models.Catalog;
using System.Net;
using System.IO;
using System.Web.Helpers;
using Nest;
using Nop.Search.Plugin.GSA.Extensions;

namespace Nop.Search.Plugin.GSA.Infrastructure.Services
{
    public class ElasticSearch : IElasticSearch
    {
        private const int SIZE = 100;
        public ElasticSearchResultModel Search(string request, GsaSetting gsaSetting, string langId, CatalogPagingFilteringModel command)
        {
            var model = new ElasticSearchResultModel();
            GetJsonResult(gsaSetting, request, command, out model);
            var productService = Core.Infrastructure.EngineContext.Current.Resolve<Nop.Services.Catalog.IProductService>();
            var workContext = Core.Infrastructure.EngineContext.Current.Resolve<Core.IWorkContext>();

            var products = productService.SearchProducts(
                 keywords: request,
                 searchDescriptions: true,
                 languageId: workContext.WorkingLanguage.Id,
                 orderBy: Core.Domain.Catalog.ProductSortingEnum.NameAsc,
                 pageIndex: command.PageIndex,
                 pageSize: gsaSetting.EResultsPerPage
                );
            model.NopSearchResults = products.TotalCount;
            model.NopProductsId = new List<int>();
            return model;
        }

        private void GetJsonResult(GsaSetting gsaSetting, string request, CatalogPagingFilteringModel command, out ElasticSearchResultModel modelRes)
        {
            var model = new ElasticSearchResultModel();
            modelRes = model;
            
            var settings = new ConnectionSettings(new Uri(gsaSetting.ElasticHost)).DefaultIndex(gsaSetting.SearchIndex);
            var client = new ElasticClient(settings);
            var searchResponse = client.Search<Product>(s => s
            .Size(0)
                    .Query(q => q
                          .MultiMatch(m => m
                            .Fields(f => f.Field(p => p.pagetext).Field(p => p.name))
                            .Query(request)
                           )
                           )
                     .Aggregations(a => a
                       .Terms("unique_results", c => c
                           .Field(f => f.nopid)
                           .Aggregations(ag => ag
                               .TopHits("top_unique_result", th => th
                                   .Source(src => src
                                       .Includes(fs => fs
                                           .Field(f => f.name)
                                           .Field(f => f.nopid)
                                           .Field(f => f.pagetext)
                                           .Field(f => f.page_number)
                                        )
                                    )
                                    .Size(1)
                                    .TrackScores()
                                )
                            )
                        )
                    )
                    );

            model.SearchKeyword = request;
            model.ElastigRequestResult = null;
            model.TotalResult = 0;
            model.TotalPages = 0;

            if (searchResponse.IsValid)
            {
                var results = searchResponse.Aggs.Terms("unique_results");
                model.ElastigRequestResult = results;
              
                if (results == null || results == null) return;
                model.TotalResult = results.Buckets.Count;
                model.TotalPages = (results.Buckets.Count <= gsaSetting.EResultsPerPage ? 1 : (results.Buckets.Count / gsaSetting.EResultsPerPage) + 1);
            }
        }

        #region Old Version
        //private List<ElasticSearchResultModel.Result> GetElasticResult(dynamic jsonObj)
        //{
        //    var model = new List<ElasticSearchResultModel.Result>();
        //    var data = jsonObj.hits.hits;
        //    var total = jsonObj.hits.total;
        //    return model;
        //}

        ///*generate elastic search result*/
        //private WebRequest GenerateRequest(GsaSetting gsaSetting, int pageNumber, string request)
        //{
        //    string paging = "size=" + gsaSetting.EResultsPerPage;
        //    if (pageNumber > 0)
        //        paging = paging + "&from=" + (pageNumber + 1) * gsaSetting.EResultsPerPage;
        //    var uri = gsaSetting.ElasticHost + @"/" + (gsaSetting.SearchIndex == String.Empty ? "" : gsaSetting.SearchIndex) + @"/" + gsaSetting.SearchKey;
        //    var req = (HttpWebRequest)WebRequest.Create(gsaSetting.ElasticHost + @"/" + (gsaSetting.SearchIndex == String.Empty ? "" : gsaSetting.SearchIndex) + @"/" + gsaSetting.SearchKey + "?q=" + request + "&" + paging);
        //    req.Method = "POST";
        //    req.ContentType = "text/xml";
        //    req.Accept = "application/json";
        //    return req;
        //}
        #endregion
    }
}
