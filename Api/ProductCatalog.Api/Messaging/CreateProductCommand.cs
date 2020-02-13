using System;
using ProductCatalog.Api.Messaging.Commands;
using ProductCatalog.Api.Requests.ProductCatalog;

namespace ProductCatalog.Api.Messaging
{
    public class CreateProductCommand : ICreateProductCommand
    {
        public Guid CorrelationId { get; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BlobName { get; set; }
        public string Photo { get; set; }
        public string PhotoName { get; set; }
        public double Price { get; set; }

        public CreateProductCommand(CreateProductRequest productRequest)
        {
            var photo = productRequest.Photo.Replace("data:image/png;base64,", string.Empty);
            CorrelationId = Guid.NewGuid();
            Code = productRequest.Code;
            Name = productRequest.Name;
            Description = productRequest.Description;
            PhotoName = productRequest.PhotoName;
            Photo = photo;
            Price = productRequest.Price;
        }
    }
}