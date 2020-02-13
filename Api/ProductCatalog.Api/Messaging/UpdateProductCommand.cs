using System;
using ProductCatalog.Api.Messaging.Commands;
using ProductCatalog.Api.Requests.ProductCatalog;

namespace ProductCatalog.Api.Messaging
{
    public class UpdateProductCommand : IUpdateProductCommand
    {
        public Guid CorrelationId { get; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoName { get; set; }
        public string BlobName { get; set; }
        public string Photo { get; set; }
        public double Price { get; set; }

        public UpdateProductCommand(string id, UpdateProductRequest productRequest)
        {
            var photo = productRequest.Photo.Replace("data:image/png;base64,", string.Empty);
            CorrelationId = Guid.NewGuid();
            Id = Guid.Parse(id);
            Code = productRequest.Code;
            Name = productRequest.Name;
            Description = productRequest.Description;
            Photo = Photo = photo;
            PhotoName = productRequest.PhotoName;
            Price = productRequest.Price;
        }
    }
}