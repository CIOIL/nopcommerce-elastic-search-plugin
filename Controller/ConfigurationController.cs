using Nop.Core;
using Nop.Core.Caching;
using Nop.Search.Plugin.GSA.Infrastructure.Domain;
using Nop.Search.Plugin.GSA.Infrastructure.Services;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using Nop.Search.Plugin.GSA.Model;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Web.Framework.Mvc;
using System.Web.Mvc;

namespace Nop.Search.Plugin.GSA.Controller
{
    public class ConfigurationController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;
        private readonly ICollectionMappingService _collectionMappingService;
        public ConfigurationController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ICollectionMappingService collectionMappingService,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
            this._collectionMappingService = collectionMappingService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigModel();
            var gsaSetting = _settingService.LoadSetting<GsaSetting>();
            model.AccessType = gsaSetting.AccessType;
            model.Collection = gsaSetting.Collection;
            model.FrontEndClient = gsaSetting.FrontEndClient;
            model.GsaHost = gsaSetting.GsaHost;
            model.OutputOption = gsaSetting.OutputOption;
            model.MaxSearchResults = gsaSetting.MaxSearchResults;
            model.SortFormat = gsaSetting.SortFormat;
            model.ResultsPerPage = gsaSetting.ResultsPerPage;
            model.ImageHost = gsaSetting.ImageHost;
            model.isGsa = gsaSetting.isGsa;
            model.IsElastic = gsaSetting.IsElastic;
            model.ElasticHost = gsaSetting.ElasticHost;
            model.SearchIndex = gsaSetting.SearchIndex;
            model.SearchKey = gsaSetting.SearchKey;
            model.EResultsPerPage = gsaSetting.EResultsPerPage;
            return View("~/Plugins/GSA/Views/Configuration/Configure.cshtml",model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigModel model)
        {

            var gsaSetting = _settingService.LoadSetting<GsaSetting>();
            int storeScope = 0;
             gsaSetting.AccessType = model.AccessType;
             gsaSetting.Collection = model.Collection;
             gsaSetting.FrontEndClient = model.FrontEndClient;
             gsaSetting.GsaHost = model.GsaHost;
             gsaSetting.OutputOption = model.OutputOption;
             gsaSetting.MaxSearchResults = model.MaxSearchResults;
             gsaSetting.SortFormat = model.SortFormat;
             gsaSetting.ResultsPerPage = model.ResultsPerPage;
             gsaSetting.ImageHost = model.ImageHost;
             gsaSetting.isGsa = model.isGsa;
             gsaSetting.IsElastic = model.IsElastic;
             gsaSetting.ElasticHost = model.ElasticHost;
             gsaSetting.SearchIndex = model.SearchIndex;
             gsaSetting.SearchKey = model.SearchKey;
            gsaSetting.EResultsPerPage = model.EResultsPerPage;
            /*Save Configuration Settings*/
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.AccessType, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.Collection, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.FrontEndClient, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.GsaHost, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.OutputOption, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.MaxSearchResults, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.SortFormat, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.ResultsPerPage, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.ImageHost, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.ElasticHost, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.SearchIndex, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.SearchKey, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.isGsa, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.IsElastic, true, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(gsaSetting, x => x.EResultsPerPage, true, storeScope, false);
            /*Clear cache and display successes message*/
            _settingService.ClearCache();
            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        [HttpPost]
        public JsonResult CollectionMappingList()
        {
            var collection = _collectionMappingService.GetCollectionMapping();

            var gridModel = new DataSourceResult
            {
                Data = collection,
                Total = collection.Count
            };
            return Json(gridModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Web.Framework.Security.AdminAntiForgery]
        public ActionResult CollectionMappingUpdate(FormCollection collection)
        {
            var s = collection;
            CollectionMapping upd = new CollectionMapping();
            upd.Id = int.Parse(collection["Id"]);
            upd.Collection = collection["Collection"];
            upd.LanguageCulture = collection["LanguageCulture"];

            var updNew = _collectionMappingService.GetCollectionMappingById(upd.Id);
            updNew.LanguageCulture = upd.LanguageCulture;
            updNew.Collection = upd.Collection;

            _collectionMappingService.UpdateMapping(updNew);
            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CollectionMappingInsert(FormCollection collection)
        {
            var s = collection;
            CollectionMapping upd = new CollectionMapping();
            upd.Collection = collection["Collection"];
            upd.LanguageCulture = collection["LanguageCulture"];
            _collectionMappingService.InsertMapping(upd);
            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult CollectionMappingDelete(FormCollection collection)
        {
            var s = collection;
            CollectionMapping upd = new CollectionMapping();
            upd.Id = int.Parse(collection["Id"]);
            upd.Collection = collection["Collection"];
            upd.LanguageCulture = collection["LanguageCulture"];
            var updNew = _collectionMappingService.GetCollectionMappingById(upd.Id);
            _collectionMappingService.DeleteMapping(updNew);
            return new NullJsonResult();
        }
    }
}
