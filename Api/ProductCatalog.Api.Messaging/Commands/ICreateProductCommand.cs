using System;

namespace ProductCatalog.Api.Messaging.Commands
{
    public interface ICreateProductCommand
    {
        Guid CorrelationId { get; }
        string Code { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string BlobName { get; set; }
        string PhotoName { get; set; }
        string Photo { get; set; }
        double Price { get; set; }
    }
}