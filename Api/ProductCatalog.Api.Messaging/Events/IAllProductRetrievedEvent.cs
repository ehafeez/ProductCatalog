using System.Collections.Generic;
using ProductCatalog.Api.Messaging.Interfaces;

namespace ProductCatalog.Api.Messaging.Events
{
    public interface IAllProductRetrievedEvent
    {
        List<IProductDto> Products { get; set; }
    }
}