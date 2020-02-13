using System.Collections.Generic;
using System.Threading.Tasks;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Dtos;

namespace Services.ProductService.App.Interfaces
{
    internal interface IProductCatalog
    {
        Task<CreateProductDto> CreateProduct(ICreateProductCommand product);
        Task<UpdateProductDto> UpdateProduct(IUpdateProductCommand product);
        Task<DeleteProductDto> DeleteProduct(string productId);
        Task<GetProductDto> GetProduct(string productId);
        Task<List<GetProductDto>> GetAllProducts(IGetAllProductsCommand productCommand);
    }
}