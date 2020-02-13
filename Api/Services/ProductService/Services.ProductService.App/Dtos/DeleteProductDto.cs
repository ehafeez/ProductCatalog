using System.Collections.Generic;
using Services.ProductService.Core.Models;

namespace Services.ProductService.App.Dtos
{
    public class DeleteProductDto
    {
        public bool State { get; }
        public List<Error> Errors { get; set; }

        public DeleteProductDto(bool state)
        {
            State = state;
        }

        internal static DeleteProductDto PrepareExceptionResponse(Product product, string exception)
        {
            return new DeleteProductDto(false)
            {
                Errors = new List<Error>
                {
                    new Error("Exception", exception)
                }
            };
        }
    }
}