using System.Collections.Generic;
using ProductCatalog.Api.Messaging.Errors;
using ProductCatalog.Api.Messaging.Events;
using Services.ProductService.App.Dtos;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.MessagingFramework
{
    public class ProductDeletedEvent : IProductDeletedEvent
    {
        public bool State { get; }
        public List<IError> Errors { get; set; }

        public ProductDeletedEvent(DeleteProductDto product)
        {
            State = product.State;

            if (product.Errors != null && product.Errors.Count > 0)
            {
                Errors = new List<IError>();
                product.Errors?.ForEach(e =>
                {
                    var createError = new Error(e.Field, e.Message);
                    Errors.Add(createError);
                });
            }
        }
    }
}