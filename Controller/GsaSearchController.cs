using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Search.Plugin.GSA.Infrastructure.Services;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using Nop.Search.Plugin.GSA.Model;
using Nop.Services.Catalog;
using Nop.Services.Configuration;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Catalog;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;
using Nop.Search.Plugin.GSA.Extensions;

namespace Nop.Admin.Plugin.CustomTabs.Controller
{
    public class GsaSearchController : BasePluginController
    {

        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IGsaServices _gsaService;
        private readonly IElasticSearch _elasticSearch;
        private readonly IProductService _productService;
        private ElasticSearchResultModel _elasticSearchResult = null;
        //   private readonly ICategoryService _categoryService;
        //private readonly CatalogSettings _catalogSettings;
        public GsaSearchController(ISettingService settingService, IWebHelper webHelper, IWorkContext workContext, IGsaServices gsaService, IElasticSearch elasticSearch, IProductService productService)
        {
            _settingService = settingService;
            _webHelper = webHelper;
            _workContext = workContext;
            _gsaService = gsaService;
            _elasticSearch = elasticSearch;
            _productService = productService;
        }

        public ActionResult Search(CatalogPagingFilteringModel command)
        {
            var request = Request.Params["q"];
            var gsaSetting = _settingService.LoadSetting<GsaSetting>();
            var langId = _workContext.WorkingLanguage.LanguageCulture;

            if (gsaSetting.isGsa)
            {
                var searchResult = _gsaService.Search(request, gsaSetting, langId, command);
                return View("~/Plugins/GSA/Views/GsaSearch/Search.cshtml", searchResult);
            }
            else if(gsaSetting.IsElastic)
            {
                //if (_elasticSearchResult == null || !_elasticSearchResult.SearchKeyword.Equals(request))
                //{
                //    if (Session["elasticSearchResult"] == null)
                //    {
                //        _elasticSearchResult = _elasticSearch.Search(request, gsaSetting, langId, command);
                //        Session["elasticSearchResult"] = _elasticSearchResult;
                //    }
                //    else
                //        _elasticSearchResult = Session["elasticSearchResult"] as ElasticSearchResultModel;
                //}

                if (Session["elasticSearchResult"] != null && Session["SearchReques"].ToString().Equals(request))
                {
                    _elasticSearchResult = Session["elasticSearchResult"] as ElasticSearchResultModel;
                }
                else
                {
                    _elasticSearchResult = _elasticSearch.Search(request, gsaSetting, langId, command);
                    Session["elasticSearchResult"] = _elasticSearchResult;
                    Session["SearchReques"] = request;
                }


                _elasticSearchResult.Data.Results = GetResults(command,gsaSetting);

                return View("~/Plugins/GSA/Views/GsaSearch/ElasticSearch.cshtml", _elasticSearchResult.Data);
            }
            else
            {
                return View("~/Plugins/GSA/Views/GsaSearch/NoSearchEngine.cshtml");
            }
        }

        private List<ProductOverviewModel> GetResults(CatalogPagingFilteringModel command, GsaSetting gsaSetting)
        {
            /*Get Total */
            

            var searchResult = new List<ProductOverviewModel>();
            var skip = command.PageIndex * gsaSetting.EResultsPerPage;
            var take = gsaSetting.EResultsPerPage;
            foreach (var r in _elasticSearchResult.ElastigRequestResult.Buckets.Skip(skip).Take(take))
            {

                var topHits = r.TopHits("top_unique_result");
                if (topHits == null || topHits.Total == 0) continue;
                var hit = topHits.Hits<Search.Plugin.GSA.Model.Product>().FirstOrDefault();
                if (hit != null)
                {
                    var data = hit.Source.ToProductOverviewModel();
                    if (data != null)
                        searchResult.Add(data);
                }
            }

            if(searchResult.Count == 0 || searchResult.Count < gsaSetting.EResultsPerPage)
            {
                int count = (_elasticSearchResult.TotalResult + _elasticSearchResult.NopSearchResults) < gsaSetting.EResultsPerPage ? 0 : searchResult.Count;
                searchResult.AddRange(PopulateFromNop(command, gsaSetting,count));
            }

            /*Paging Calculation*/
            IPagedList<ProductOverviewModel> result = new PagedList<ProductOverviewModel>(_elasticSearchResult.Data.Results, command.PageIndex, gsaSetting.EResultsPerPage, (_elasticSearchResult.TotalResult + _elasticSearchResult.NopSearchResults));
            _elasticSearchResult.Data.PagingFilteringContext.PageNumber = command.PageNumber == 0 ? 1 : command.PageNumber;
            _elasticSearchResult.Data.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            _elasticSearchResult.Data.PagingFilteringContext.LoadPagedList(result);
            _elasticSearchResult.Data.TotalResults = _elasticSearchResult.TotalResult + _elasticSearchResult.NopSearchResults;
            return searchResult;
        }

        private List<ProductOverviewModel> PopulateFromNop(CatalogPagingFilteringModel command, GsaSetting gsaSetting, int count)
        {
            int diff = count == 0 ? 0 : gsaSetting.EResultsPerPage - count;
            int pageSize = diff == 0 ? _elasticSearchResult.NopProductsId.Count + gsaSetting.EResultsPerPage : diff;
            var products = _productService.SearchProducts(
                  keywords: _elasticSearchResult.SearchKeyword,
                  searchDescriptions: true,
                  languageId: _workContext.WorkingLanguage.Id,
                  orderBy: ProductSortingEnum.NameAsc,
                  pageIndex: (command.PageIndex == 0 ? 0 : command.PageIndex - _elasticSearchResult.TotalPages),
                  pageSize: pageSize
                 );
            if(_elasticSearchResult.NopProductsId.Count > 0)
            {
                foreach(var i in _elasticSearchResult.NopProductsId)
                {
                    var item = products.Where(x => x.Id == i).FirstOrDefault();
                    if (item != null)
                        products.Remove(item);
                }
                _elasticSearchResult.NopProductsId = new List<int>();
            }
            else if(diff > 0)
            {
                _elasticSearchResult.NopProductsId.AddRange(products.Select(x => x.Id));
            }
            return products.ToListProductOverviewModel();
        }
    }
}
