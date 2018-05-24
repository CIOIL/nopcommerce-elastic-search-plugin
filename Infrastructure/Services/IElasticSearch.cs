using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using Nop.Web.Models.Catalog;
using Nop.Search.Plugin.GSA.Model;

namespace Nop.Search.Plugin.GSA.Infrastructure.Services
{
    public interface IElasticSearch
    {
        ElasticSearchResultModel Search(string request, GsaSetting gsaSetting, string langId, CatalogPagingFilteringModel command);
    }
}
