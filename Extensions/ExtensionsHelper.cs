using Nop.Search.Plugin.GSA.Model;
using Nop.Web.Models.Catalog;
using Nop.Services.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Infrastructure;
using Nop.Services.Seo;
using Nop.Web.Models.Media;
using Nop.Services.Media;
using Nop.Core.Domain.Media;
using Nop.Services.Localization;
using SevenSpikes.Nop.Plugins.Attachments.Services;
using Nop.Core;

namespace Nop.Search.Plugin.GSA.Extensions
{
    public static class ExtensionsHelper
    {
        private const int LANGUAGE_SPECIFICATION_ID = 15;
        public static ProductOverviewModel ToProductOverviewModel(this Product product)
        {
            var productService = EngineContext.Current.Resolve<IProductService>();
            var productModel = new ProductOverviewModel();
            var pr = productService.GetProductById(product.nopid);
            if (pr == null) return null;
            productModel.Id = product.nopid;
            productModel.Name = pr.Name;
            productModel.ShortDescription = product.pagetext;
            productModel.CustomProperties.Add("page_number", product.page_number);
            productModel.CustomProperties.Add("link_to_documnet", GetLinkToDocumnet(product.page_number,product.nopid));
            productModel.SeName = pr.GetSeName();

            productModel.DefaultPictureModel = PrepareProductOverviewPictureModel(product.nopid);
            productModel.SpecificationAttributeModels = PrepareProductSpecificationModel(product.nopid);
            /*Prepare specifications*/
            /*Prepare image for product*/

            return productModel;
        }

        public static List<ProductOverviewModel> ToListProductOverviewModel(this IPagedList<Core.Domain.Catalog.Product> product)
        {
            var results = new List<ProductOverviewModel>();
            foreach(var p in product)
            {
                results.Add(new ProductOverviewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    ShortDescription = p.ShortDescription,
                    SeName = p.GetSeName(),
                    DefaultPictureModel = PrepareProductOverviewPictureModel(p.Id),
                    SpecificationAttributeModels = PrepareProductSpecificationModel(p.Id),
                    CustomProperties = new Dictionary<string, object>() { { "page_number", 1 }, { "link_to_documnet", GetLinkToDocumnet(1, p.Id) } }
            });
            }
            return results;
        }


        private static string GetLinkToDocumnet(int page_number, int productId)
        {
            var attService = EngineContext.Current.Resolve<IAttachmentService>();
            var attDownloadService = EngineContext.Current.Resolve<IAttachmentDownloadService>();
            var currentAttachment = attService.GetNonSharedAttachmentsAndEntityMappingsByProductId(productId);
            if (currentAttachment.Count == 0) return String.Empty;
            try
            {
                var path = currentAttachment.FirstOrDefault().Item1.AttachmentDownload.Id + "_" + currentAttachment.FirstOrDefault().Item1.AttachmentDownload.Filename + "." + currentAttachment.FirstOrDefault().Item1.AttachmentDownload.Extension;
                return path;
            }
            catch { return String.Empty; }

    }

    private static IList<ProductSpecificationModel> PrepareProductSpecificationModel(int productId)
        {
            var specificationService = EngineContext.Current.Resolve<ISpecificationAttributeService>();
            var productSpecificationModel = new List<ProductSpecificationModel>();
            specificationService.GetProductSpecificationAttributes(productId, 0, null, true).ToList().Where(x=>x.SpecificationAttributeOption.SpecificationAttributeId == LANGUAGE_SPECIFICATION_ID).ToList().ForEach(psa => {
                productSpecificationModel.Add(new ProductSpecificationModel
                {
                    SpecificationAttributeId = psa.SpecificationAttributeOption.SpecificationAttributeId,
                    SpecificationAttributeName = psa.SpecificationAttributeOption.SpecificationAttribute.GetLocalized(x => x.Name),
                    ValueRaw = psa.CustomValue
                });
            });

            return productSpecificationModel;
        }

        private static PictureModel PrepareProductOverviewPictureModel(int productId)
        {
            var pictureService = EngineContext.Current.Resolve<IPictureService>();
            var mediaSettings = EngineContext.Current.Resolve<MediaSettings>();
            int pictureSize = mediaSettings.ProductThumbPictureSize;
            PictureModel defaultPictureModel = new PictureModel();
            var picture = pictureService.GetPicturesByProductId(productId, 1).FirstOrDefault();
            defaultPictureModel.ImageUrl = pictureService.GetPictureUrl(picture, pictureSize);
            defaultPictureModel.FullSizeImageUrl = pictureService.GetPictureUrl(picture);

            /*
                //"title" attribute
                pictureModel.Title = (picture != null && !string.IsNullOrEmpty(picture.TitleAttribute))
                    ? picture.TitleAttribute
                    : string.Format(_localizationService.GetResource("Media.Product.ImageLinkTitleFormat"), productName);
                //"alt" attribute
                pictureModel.AlternateText = (picture != null && !string.IsNullOrEmpty(picture.AltAttribute))
                    ? picture.AltAttribute
                    : string.Format(_localizationService.GetResource("Media.Product.ImageAlternateTextFormat"),
                        productName);
            */
            return defaultPictureModel;
        }
    }
}
