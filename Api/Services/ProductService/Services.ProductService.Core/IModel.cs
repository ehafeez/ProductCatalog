using System;

namespace Services.ProductService.Core
{
    public interface IModel
    {
        Guid Id { get; set; }
    }
}