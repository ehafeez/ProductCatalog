using System;
using ProductCatalog.Api.Messaging.Commands;

namespace ProductCatalog.Api.Messaging
{
    public class GetProductCommand : IGetProductCommand
    {
        public Guid CorrelationId { get; }
        public string ProductId { get; }

        public GetProductCommand(string productId)
        {
            CorrelationId = Guid.NewGuid();
            ProductId = productId;
        }
    }
}