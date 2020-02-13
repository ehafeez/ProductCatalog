using System;
using System.Collections.Generic;
using Services.ProductService.Core;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.Dtos
{
    public class CreateProductDto
    {
        public Guid Id { get; }
        public List<Error> Errors { get; set; }

        public CreateProductDto(IModel product)
        {
            Id = product.Id;
        }

        internal static CreateProductDto PrepareExceptionResponse(Product product, string exception)
        {
            return new CreateProductDto(product)
            {
                Errors = new List<Error>
                {
                    new Error("ERROR_SYSTEM", exception)
                }
            };
        }
    }
}