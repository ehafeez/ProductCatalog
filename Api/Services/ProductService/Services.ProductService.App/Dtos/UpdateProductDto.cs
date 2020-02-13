using System;
using System.Collections.Generic;
using Services.ProductService.Core;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.Dtos
{
    public class UpdateProductDto
    {
        public Guid Id { get; }
        public List<Error> Errors { get; set; }

        public UpdateProductDto(IModel product)
        {
            Id = product.Id;
        }

        internal static UpdateProductDto PrepareExceptionResponse(Product product, string exception)
        {
            return new UpdateProductDto(product)
            {
                Errors = new List<Error>
                {
                    new Error("ERROR_SYSTEM", exception)
                }
            };
        }
    }
}