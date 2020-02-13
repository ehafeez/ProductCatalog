using System;

namespace ProductCatalog.Api.Messaging.Interfaces
{
    public interface IProductDto
    {
        Guid Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        byte[] Photo { get; set; }
        string BlobName { get; set; }
        string PhotoName { get; set; }
        double Price { get; set; }
        string LastUpdated { get; set; }
    }
}