using System;
using System.Collections.Generic;
using Services.ProductService.Core.Models;

namespace Services.ProductService.Repository.Interfaces
{
    public interface IProductCatalogRepository : IRepositoryBase<Product>
    {
        List<Product> GetAllProducts();
        Product GetProductById(Guid productId);
        void CreateProduct(Product product);
        void UpdateProduct(Product old, Product change);
        void DeleteProduct(Product product);
    }
}