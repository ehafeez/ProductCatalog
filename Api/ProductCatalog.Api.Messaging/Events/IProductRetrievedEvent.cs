using ProductCatalog.Api.Messaging.Interfaces;

namespace ProductCatalog.Api.Messaging.Events
{
    public interface IProductRetrievedEvent
    {
        IProductDto ProductDto { get; set; }
    }
}