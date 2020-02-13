using System.Collections.Generic;
using ProductCatalog.Api.Messaging.Events;
using ProductCatalog.Api.Messaging.Interfaces;
using Services.ProductService.App.Dtos;

namespace Services.ProductService.App.MessagingFramework
{
    public class AllProductRetrievedEvent : IAllProductRetrievedEvent
    {
        public List<IProductDto> Products { get; set; }

        public AllProductRetrievedEvent(List<GetProductDto> response)
        {
            Products = new List<IProductDto>();
            if (response != null && response.Count > 0)
            {
                response.ForEach(p =>
                {
                    Products.Add(new GetProductDto(p.Id, p.Code, p.Name, p.Description, p.Photo, p.PhotoName,
                        p.Price, p.LastUpdated, p.BlobName));
                });
            }
        }
    }
}