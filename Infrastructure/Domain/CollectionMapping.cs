using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Search.Plugin.GSA.Infrastructure.Domain
{
    public class CollectionMapping : BaseEntity
    {
        public string LanguageCulture { get; set; }
        public string Collection { get; set; }

    }
}
