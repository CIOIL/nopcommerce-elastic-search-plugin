using Autofac;
using Autofac.Core;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Search.Plugin.GSA.Infrastructure.Data;
using Nop.Search.Plugin.GSA.Infrastructure.Domain;
using Nop.Search.Plugin.GSA.Infrastructure.Services;
using Nop.Web.Framework.Mvc;

namespace Nop.Search.Plugin.GSA.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            /*Register Servises */
            builder.RegisterType<GsaServices>().As<IGsaServices>().InstancePerLifetimeScope();
            builder.RegisterType<CollectionMappingService>().As<ICollectionMappingService>().InstancePerLifetimeScope(); ;
            builder.RegisterType<ElasticSearch>().As<IElasticSearch>().InstancePerLifetimeScope();

            /*Register Context*/
            this.RegisterPluginDataContext<CollectionMappingObjectContext>(builder, "nop_object_context_collection_mapping");
            builder.RegisterType<EfRepository<CollectionMapping>>()
               .As<IRepository<CollectionMapping>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_collection_mapping"))
               .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 2; }
        }
    }
}
