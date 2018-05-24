using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Search.Plugin.GSA.Infrastructure.Domain;
using Nop.Core.Data;
using Nop.Search.Plugin.GSA.Infrastructure.Data;

namespace Nop.Search.Plugin.GSA.Infrastructure.Services
{
    public class CollectionMappingService : ICollectionMappingService
    {
        private readonly IRepository<CollectionMapping> _collectionMappingRepository;
        public CollectionMappingService(IRepository<CollectionMapping> collectionMappingRepository)
        {
            _collectionMappingRepository = collectionMappingRepository;
        }
        public void DeleteMapping(CollectionMapping record)
        {
            _collectionMappingRepository.Delete(record);
        }

        public List<CollectionMapping> GetCollectionMapping()
        {
            return _collectionMappingRepository.Table.ToList();
        }

        public void InsertMapping(CollectionMapping record)
        {
            _collectionMappingRepository.Insert(record);
        }

        public void UpdateMapping(CollectionMapping record)
        {
            _collectionMappingRepository.Update(record);
            
        }

        public CollectionMapping GetCollectionMappingById(int id)
        {
            return _collectionMappingRepository.GetById(id);
        }

        public List<CollectionMapping> GetCollectionByLanguageCulture(string langId)
        {
            var collections = _collectionMappingRepository.Table.Where(x => x.LanguageCulture == langId).ToList();
            if (collections.Count > 0)
                return collections; 
            return null;
        }
    }
}
