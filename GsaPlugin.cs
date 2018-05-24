using Nop.Core;
using Nop.Core.Plugins;
using Nop.Search.Plugin.GSA.Infrastructure.Data;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Web.Framework.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Search.Plugin.GSA
{
    class GsaPlugin : BasePlugin, IMiscPlugin , IAdminMenuPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly CollectionMappingObjectContext _context;
        public GsaPlugin(ISettingService settingService, IWebHelper webHelper, CollectionMappingObjectContext context)
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
            _context = context;
        }
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "Configuration";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Search.Plugin.GSA.Controllers" }, { "area", null } };
        }
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "SearchEngine",
                Title = "Search Engine configuration",
                Url = "/Admin/Plugin/ConfigureMiscPlugin?systemName=SearchEngine",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },

            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");

            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }
        public override void Install()
        {
            /*Set Default Settings data*/
            var settings = new GsaSetting
            {
                GsaHost = "",
                Collection = "",
                FrontEndClient = "",
                AccessType = "p",
                MaxSearchResults = 1000,
                OutputOption = "xml",
                ResultsPerPage = 15,
                SortFormat = "Default",
                ImageHost = "",
                isGsa = false,
                IsElastic = true,
                ElasticHost = "",
                SearchIndex = "",
                SearchKey = "",
                EResultsPerPage = 10

            };
            _settingService.SaveSetting(settings);

            /*Set Resource */
            /* Display Resource*/

            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.GsaHost", "Host Name");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.DefaultCollection", "Default Collection Name");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.FrontEndClient", "Frontend client");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.AccessType", "Access Type");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.MaxSearchResults", "Max Search Results");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.OutputOption", "Output Option");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.ResultsPerPage", "Results Per Page");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.SortFormat", "SortFormat");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Result", "Search Results");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.TotalResults", "Total Results For", "en-US");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.TotalResults", "סה''כ תוצאות ", "he-IL");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.LastUpdate", "Last update");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Photographer", "Photographer");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.City","City");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Region", "Region");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Today", "Today");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Humidity", "Humidity");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.WindAndSpeed", "Wind direction and speed");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Tommorow", "Tommorow");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.ThreeDaysFrom", "3 days from");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Two_days_from", "Two days from");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.DisplayResult", "Display Results {0} of {1}");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.ExchangeRate", "Exchange rate");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.ImageHost", "Hostname for Images");

            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Collection", "Collection");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.LanguageCulture", "Language Culture");

            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.UseGsa", "Gsa Search engine");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.UseElastic", "Elastic Search engine (with default search)");

            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.ElasticHost", "Elastic Host");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.SearchIndex", "Search Index");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.SearchKey","Search Key");
            /*Tab Settings*/
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Tab.General", "General");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Tab.GsaConfig", "Gsa Configuration");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Tab.ElasticSearchConfig", "Elastic Search Configuration");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Config.SearchEngine", "Select Search Engine");
            /*Error Resource*/
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.GsaHostError", "Host Name Required");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.CollectionError", "Collection Name Required");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.FrontEndClientError", "Frontend client Required");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.EventType", "Event Type");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Time", "Time");
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Adress", "Address");
            /*Hint Resorses*/
            this.AddOrUpdatePluginLocaleResource("Nop.Search.Plugin.GSA.Collection.Hint", @"You can search multiple collections by separating collection names with the OR character, which is notated as the pipe symbol, or the AND character, which is notated as a period.
                The following example uses the AND character: &site = col1.col2
                The following example uses the OR character: &site = col1 | col2");
            try
            {
                _context.Install();
            }
            catch { }
            base.Install();
        }

        public override void Uninstall()
        {
            _settingService.DeleteSetting<GsaSetting>();
   
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.GsaHost");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Collection");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.FrontEndClient");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.AccessType");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.AccessType");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.OutputOption");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.ResultsPerPage");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.SortFormat");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.GsaHostError");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.CollectionError");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.FrontEndClientError");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Collection.Hint");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Result");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.TotalResults");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.LastUpdate");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Photographer");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.EventType");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Adress");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Time");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.City");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Region");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Today");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Tommorow");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.ThreeDaysFrom");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Two_days_from");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Search.Humidity");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.ImageHost");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.Collection");
            this.DeletePluginLocaleResource("Nop.Search.Plugin.GSA.LanguageCulture");
            _context.Uninstall();
            base.Uninstall();
        }
    }
}

/*
You can search multiple collections by separating collection names with the OR character, which is notated as the pipe symbol, or the AND character, which is notated as a period.
The following example uses the AND character: &site=col1.col2
The following example uses the OR character: &site=col1|col2
*/
