using System;

namespace ProductCatalog.Api.Messaging.Commands
{
    public interface IGetProductCommand
    {
        Guid CorrelationId { get; }
        string ProductId { get; }
    }
}