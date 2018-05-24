using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Models.Catalog;

namespace Nop.Search.Plugin.GSA.Model
{
    public class Product
    {
        public string name { get; set; }
        public string pagetext { get; set; }
        public int nopid { get; set; }
        public int page_number { get; set; }
    }
}
