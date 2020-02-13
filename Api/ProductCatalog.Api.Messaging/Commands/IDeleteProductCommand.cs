using System;

namespace ProductCatalog.Api.Messaging.Commands
{
    public interface IDeleteProductCommand
    {
        Guid CorrelationId { get; }
        string ProductId { get; }
    }
}