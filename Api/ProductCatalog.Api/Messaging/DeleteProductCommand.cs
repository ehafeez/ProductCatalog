using System;
using ProductCatalog.Api.Messaging.Commands;

namespace ProductCatalog.Api.Messaging
{
    public class DeleteProductCommand : IDeleteProductCommand
    {
        public Guid CorrelationId { get; }
        public string ProductId { get; }

        public DeleteProductCommand(string productId)
        {
            CorrelationId = Guid.NewGuid();
            ProductId = productId;
        }
    }
}