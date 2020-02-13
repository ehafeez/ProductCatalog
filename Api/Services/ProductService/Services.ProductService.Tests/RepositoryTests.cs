using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.ProductService.Core.Models;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;
using Xunit;

namespace Services.ProductService.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Create_New_Product()
        {
            //Arrange
            var dbContext = await SetUpDatabaseContext();
            var repository = new RepositoryWrapper(dbContext);

            //Act
            repository.Product.CreateProduct(new Product
            {
                Code = "Code 2",
                Name = "Product 2",
                BlobName = "ASDS-SDSD-SDSD-SDSDS.file",
                PhotoName = "test.png",
                Price = 155,
                Description = "",
                LastUpdated = DateTime.Now,
                CreatedAt = DateTime.Now
            });
            repository.Save();

            //Assert
            Assert.Equal(2, repository.Product.GetAllProducts().Count);
        }

        [Fact]
        public async Task Get_Product_By_Name()
        {
            //Arrange
            var dbContext = await SetUpDatabaseContext();
            var repository = new RepositoryWrapper(dbContext);

            //Act
            var product = repository.Product.GetByCondition(x => x.Name == "Name 1");

            //Assert
            Assert.NotNull(product);
            Assert.Equal(1, product.Count());
            Assert.Equal("Name 1", product.First().Name);
        }

        [Fact]
        public async Task Get_All_Products()
        {
            //Arrange
            var dbContext = await SetUpDatabaseContext();
            var repository = new RepositoryWrapper(dbContext);

            //Act
            var products = repository.Product.GetAllProducts();

            //Assert
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task Get_Product_By_Id()
        {
            //Arrange
            var dbContext = await SetUpDatabaseContext();
            var repository = new RepositoryWrapper(dbContext);

            //Act
            var product = repository.Product.GetProductById(Guid.Parse("2b8f9b85-b04d-4df3-9ab7-c69f44341dc7"));

            //Assert
            Assert.NotNull(product);
            Assert.Equal("Name 1", product.Name);
        }

        [Fact]
        public async Task Get_Invalid_Product_By_Id()
        {
            //Arrange
            var dbContext = await SetUpDatabaseContext();
            var repository = new RepositoryWrapper(dbContext);

            //Act
            var product = repository.Product.GetProductById(Guid.NewGuid());

            //Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task Delete_Product()
        {
            //Arrange
            using (var dbContext = await SetUpDatabaseContext())
            {
                var repository = new RepositoryWrapper(dbContext);

                //Act
                var product = repository.Product.GetProductById(Guid.Parse("2b8f9b85-b04d-4df3-9ab7-c69f44341dc7"));
                repository.Product.DeleteProduct(product);
                repository.Save();

                var products = repository.Product.GetAllProducts();
                //Assert
                Assert.NotNull(product);
                Assert.Empty(products);
            }
        }
        
        [Fact]
        public async Task Update_Product()
        {
            //Arrange
            using (var dbContext = await SetUpDatabaseContext())
            {
                var repository = new RepositoryWrapper(dbContext);

                //Act
                var product = repository.Product.GetProductById(Guid.Parse("2b8f9b85-b04d-4df3-9ab7-c69f44341dc7"));
                var changeProduct = new Product
                {
                    Id = Guid.Parse("2b8f9b85-b04d-4df3-9ab7-c69f44341dc7"),
                    Description = "description added",
                };
                repository.Product.UpdateProduct(product, changeProduct);
                repository.Save();
                var products = repository.Product.GetAllProducts();

                //Assert
                Assert.NotNull(product);
                Assert.NotEmpty(products);
                Assert.Equal("description added", products[0].Description);
            }
        }

        private async Task<ProductDbContext> SetUpDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var context = new ProductDbContext(options);
            context.Products.Add(new Product
            {
                Id = Guid.Parse("2b8f9b85-b04d-4df3-9ab7-c69f44341dc7"),
                Code = "Code 1",
                Name = "Name 1",
                Description = "No Description",
                Price = 10.55
            });

            await context.SaveChangesAsync();
            return context;
        }
    }
}