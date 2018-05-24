
using Nop.Search.Plugin.GSA.Infrastructure.Domain;
using System.Data.Entity.ModelConfiguration;

namespace Nop.Search.Plugin.GSA.Infrastructure.Data
{
    public class CollectionMappingMap : EntityTypeConfiguration<CollectionMapping>
    {
        public CollectionMappingMap()
        {
            this.ToTable("Gsa_CollectionMapping");
            HasKey(m => m.Id);
            Property(m => m.Collection).HasMaxLength(100);
            Property(m => m.LanguageCulture).HasMaxLength(10);
        }
    }
}
