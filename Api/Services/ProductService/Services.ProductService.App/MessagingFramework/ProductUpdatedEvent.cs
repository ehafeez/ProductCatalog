using System;
using System.Collections.Generic;
using ProductCatalog.Api.Messaging.Errors;
using ProductCatalog.Api.Messaging.Events;
using Services.ProductService.App.Dtos;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.MessagingFramework
{
    public class ProductUpdatedEvent : IProductUpdatedEvent
    {
        public Guid Id { get; }
        public List<IError> Errors { get; set; }
        public ProductUpdatedEvent(UpdateProductDto product)
        {
            Id = product.Id;

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