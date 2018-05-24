using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Search.Plugin.GSA.Model;
using Nop.Search.Plugin.GSA.Infrastructure.Settings;
using System.Net;
using System.IO;
using System.Xml.Linq;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog;
using Nop.Core;
using Nop.Web.Models.Catalog;

namespace Nop.Search.Plugin.GSA.Infrastructure.Services
{
    public class GsaServices : IGsaServices
    {

        public SearchResultModel Search(string request, GsaSetting gsaSetting, string langId, CatalogPagingFilteringModel command)
        {

            /*Create Search Request*/
            var query = CreateRequest(request, gsaSetting, langId, command.PageNumber,command.PageIndex);
            /*Create Request*/
            var req = (HttpWebRequest)WebRequest.Create("http://" + gsaSetting.GsaHost + "/search");
            var data = Encoding.GetEncoding("UTF-8").GetBytes(query);
            req.Method = "POST";
            req.ContentType = "text/xml";
            req.ContentLength = data.Length;
            using (var stream = req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)req.GetResponse();

            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var searchResult = GenerateResultModel(responseString, request,command,gsaSetting.ResultsPerPage);



            return searchResult;
        }

        private SearchResultModel GenerateResultModel(string responseString, string request, CatalogPagingFilteringModel command, int pageSize)
        {
            SearchResultModel model = new SearchResultModel();
            XDocument doc = XDocument.Parse(responseString);
            var rootDoc = doc.Descendants("RES").FirstOrDefault();
            if (rootDoc == null) return model;
            model.StartResultNum = Convert.ToInt32(rootDoc.Attribute("SN").Value);
            model.EndResultNum = Convert.ToInt32(rootDoc.Attribute("EN").Value);
            model.TotalResults = Math.Min(Convert.ToInt32(rootDoc.Element("M").Value),1000);
            model.SearchKeywords = request;
            if (rootDoc.Element("NB") != null)
            {
                model.NextSearchReques = rootDoc.Element("NU") != null ? rootDoc.Element("NU").Value : String.Empty;
                model.PrevSearchRequet = rootDoc.Element("PU") != null ? rootDoc.Element("PU").Value : String.Empty;
            }
            //Fill root params

            model.GsaResult = from r in doc.Descendants("R")
                              select new SearchResultModel.Results
                              {
                                  Id = r.Attribute("N").Value,
                                  ResuslAttributes = from ra in r.Descendants("MT")
                                                     select new SearchResultModel.Attributes
                                                     {
                                                         Attribute = ra.FirstAttribute.Value,
                                                         Value = ra.LastAttribute.Value
                                                     },
                              };

            IPagedList<SearchResultModel.Results> result = new PagedList<SearchResultModel.Results>(model.GsaResult.ToList(), command.PageIndex, pageSize, model.TotalResults);
            model.PagingFilteringContext.PageNumber = command.PageNumber == 0 ? 1 : command.PageNumber;
            model.PagingFilteringContext.AllowCustomersToSelectPageSize = false;
            model.PagingFilteringContext.LoadPagedList(result);

            return model;
            //var categories = EngineContext.Current.Resolve<ICategoryService>().GetAllCategories();
            //foreach (var r in model.GsaResult)
            //{
            //    /*Display Site Link*/

            //    /*Display Type*/
            //    var name = categories.Where(x => x.Id.ToString() == r.ResuslAttributes.Where(y => y.Attribute.Equals("C_Id")).FirstOrDefault().Value).FirstOrDefault().Name;
            //    switch (name)
            //    {
            //        case "Hotels":
            //            r.Type = Enum.DisplayType.Hotels;
            //                break;
            //        case "B&B":
            //            r.Type = Enum.DisplayType.BB;
            //            break;
            //        case "Events":
            //            r.Type = Enum.DisplayType.Events;
            //            break;
            //        case "Pictures":
            //            r.Type = Enum.DisplayType.Pictures;
            //            break;
            //        case "Forecast":
            //            r.Type = Enum.DisplayType.Forecast;
            //            break;
            //        case "Exchange rates":
            //            r.Type = Enum.DisplayType.Change;
            //            break;
            //    }
            //    model.GsaResult.Where(x => x.Id == r.Id).FirstOrDefault().Site = r.Site;
            //    model.GsaResult.Where(x => x.Id == r.Id).FirstOrDefault().Type = r.Type;

            //}
        }

        private string CreateRequest(string request, GsaSetting gsaSetting, string langId, int pageNumber, int pageIndex)
        {
            var context = EngineContext.Current.Resolve<ICollectionMappingService>();

            string query = String.Empty;
            var collection = context.GetCollectionByLanguageCulture(langId);
            string collectionList = gsaSetting.Collection;
            if (collection != null)
            {
                collectionList = String.Empty;
                foreach (var c in collection)
                    collectionList = c.Collection + "|";
                if (collectionList.EndsWith("|"))
                    collectionList = collectionList.Substring(0, collectionList.Length - 1);
            }
            string ie = "utf8";
            string oe = "utf8";
            int nextItems = pageIndex * gsaSetting.ResultsPerPage;

            /*Removed &lr={3}*/
            query = String.Format("q={0}&num={1}&site={2}&ie={9}&client={3}&access={4}&sort={5}&getfields={6}&start={7}&sa=N&filter=0&entqr=3&output=x{8}&entqrm=3o&oe={10}",
                request, (1000- nextItems) >= gsaSetting.ResultsPerPage ? gsaSetting.ResultsPerPage : 1000 - nextItems, collectionList, gsaSetting.FrontEndClient, gsaSetting.AccessType, "date:D:L:d1", GetFields(), nextItems, gsaSetting.OutputOption, ie,oe);
            return query;
        }

        private string GetFields()
        {
            return @"WeatherCode1.WeatherCode2.WeatherCode3.WeatherCode4.Currency_Type.Amount_for_Exchange.Last_updated.CategoryNameDefault.SourceUrl.ProductId.Name.DisplayType.Accommodation_Type.Address.C_Id.C_Name.C_ParentId.C_ParentName.C_ParentPicUrl.C_ParentUrl.C_PicUrl.C_Url.City.City_ID.Country.FullDescription.Hotel_Classification.Humidity_Today.Number_of_Rooms.Phone.Pic_Url.Picture_Category.Place_Description.Product_Url.Region.ShortDescription.Temperature_Today.Temperature_Tomorrow.Temperature_day_after_Tomorrow.Temperature_two_days_after_Tomorrow.URL.UpdatedOnUtc.Wind_direction_and_speed_Today.Zimmer_ser_ribon_field.Language.Photographer.Ascription.Theme.Event_Name.Event_Type.Date_and_Time.Year.Date.Last_updated.Exchange_rate_in_NIS.Day.Month";
        }
    }
}
