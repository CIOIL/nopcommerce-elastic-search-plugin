using Nop.Search.Plugin.GSA.Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Search.Plugin.GSA.Infrastructure.Services
{
    public interface ICollectionMappingService
    {
        void InsertMapping(CollectionMapping record);
        void DeleteMapping(CollectionMapping record);
        void UpdateMapping(CollectionMapping record);
        List<CollectionMapping> GetCollectionMapping();
        CollectionMapping GetCollectionMappingById(int id);
        List<CollectionMapping> GetCollectionByLanguageCulture(string langId);
    }
}
