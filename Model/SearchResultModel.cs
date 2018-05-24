using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Nop.Search.Plugin.GSA.Enum;

namespace Nop.Search.Plugin.GSA.Model
{
    public class SearchResultModel : BaseNopEntityModel
    {

        public SearchResultModel()
        {
            this.PagingFilteringContext = new CatalogPagingFilteringModel();
        }
        //Total Results for current Request
        public int TotalResults { get; set; }
        public int StartResultNum { get; set; }
        public int EndResultNum { get; set; }
        public string PrevSearchRequet { get; set; }
        public string NextSearchReques { get; set; }
        public IEnumerable<Results> GsaResult { get; set; }



        public string SearchKeywords { get; set; }
        public CatalogPagingFilteringModel PagingFilteringContext { get; private set; }

        #region Nested Classes
        public partial class Results
        {

            public string Id { get; set; }
            public DisplayType Type { get; set; }
            public string Site { get; set; }
            public IEnumerable<Attributes> ResuslAttributes { get; set; }
        }

        public partial class Attributes
        {
            public string Attribute { get; set; }
            public string Value { get; set; }
        }





        //public partial class Results : BaseNopEntityModel
        //{
        //    public string DisplayType { get; set; }
        //    public string Accommodation_Type { get; set; }
        //    public string Address { get; set; }
        //    public int? C_Id { get; set; }
        //    public int? C_Name { get; set; }
        //    public int? C_ParentId { get; set; }
        //    public string C_ParentName { get; set; }
        //    public string C_ParentPicUrl { get; set; }
        //    public string C_ParentUrl { get; set; }
        //    public string C_PicUrl { get; set; }
        //    public string C_Url { get; set; }
        //    public string City { get; set; }
        //    public int? City_ID { get; set; }
        //    public string Country { get; set; }
        //    public string FullDescription { get; set; }
        //    public string Hotel_Classification { get; set; }
        //    public string Humidity_Today { get; set; }
        //    public int? Number_of_Rooms { get; set; }
        //    public string Phone { get; set; }
        //    public string Pic_Url { get; set; }
        //    public string Picture_Category { get; set; }
        //    public string Place_Description { get; set; }
        //    public string Product_Url { get; set; }
        //    public string Region { get; set; }
        //    public string ShortDescription { get; set; }
        //    public string Temperature_Today { get; set; }
        //    public string Temperature_Tomorrow { get; set; }
        //    public string Temperature_day_after_Tomorrow { get; set; }
        //    public string Temperature_two_days_after_Tomorrow { get; set; }
        //    public string URL { get; set; }
        //    public string UpdatedOnUtc { get; set; }
        //    public string Wind_direction_and_speed_Today { get; set; }
        //    public string Zimmer_ser_ribon_field { get; set; }
        //    public string Language { get; set; }
        //    public string Photographer { get; set; }
        //    public string Ascription { get; set; }
        //    public string Theme { get; set; }
        //    public string Event_Name { get; set; }
        //    public string Event_Type { get; set; }
        //    public string Date_and_Time { get; set; }
        //    public string Date { get; set; }
        //    public string Last_updated { get; set; }
        //    public string Exchange_rate_in_NIS { get; set; }
        //}
        #endregion
    }
}
