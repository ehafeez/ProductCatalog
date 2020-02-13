using System;
using System.Collections.Generic;
using System.Linq;
using Services.ProductService.Core.Models;
using Services.ProductService.Data;
using Services.ProductService.Repository.Interfaces;

namespace Services.ProductService.Repository.Implementations
{
    public class ProductCatalogRepository : RepositoryBase<Product>, IProductCatalogRepository
    {
        public ProductCatalogRepository(ProductDbContext productDbContext) : base(productDbContext)
        {
        }

        public List<Product> GetAllProducts()
        {
            return GetAll().OrderBy(p => p.Name).ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return GetByCondition(p => p.Id.Equals(productId)).FirstOrDefault();
        }

        public void CreateProduct(Product product)
        {
            product.Id = Guid.NewGuid();
            Create(product);
        }

        public void UpdateProduct(Product old, Product change)
        {
            old.Code = change.Code;
            old.Name = change.Name;
            old.BlobName = change.BlobName;
            old.Price = change.Price;
            old.Description = change.Description;
            old.LastUpdated = DateTime.Now;

            Update(old);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }
    }
}