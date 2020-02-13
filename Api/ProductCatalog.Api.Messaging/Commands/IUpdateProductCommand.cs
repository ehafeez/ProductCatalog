using System;

namespace ProductCatalog.Api.Messaging.Commands
{
    public interface IUpdateProductCommand
    {
        Guid CorrelationId { get; }
        Guid Id { get; set; }
        string Code { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string PhotoName { get; set; }
        string BlobName { get; set; }
        string Photo { get; set; }
        double Price { get; set; }
    }
}