using ProductCatalog.Api.Messaging.Events;
using ProductCatalog.Api.Messaging.Interfaces;
using Services.ProductService.App.Dtos;

namespace Services.ProductService.App.MessagingFramework
{
    public class ProductRetrievedEvent : IProductRetrievedEvent
    {
        public IProductDto ProductDto { get; set; }

        public ProductRetrievedEvent(GetProductDto product)
        {
            if (product != null)
            {
                ProductDto = new GetProductDto(product.Id, product.Code, product.Name, product.Description,
                    product.Photo, product.PhotoName, product.Price, product.LastUpdated, product.BlobName);
            }
        }
    }
}