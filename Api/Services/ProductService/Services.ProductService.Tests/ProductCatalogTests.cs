using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Implementations;
using Services.ProductService.Core.Models;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;
using Services.ProductService.Repository.Interfaces;
using Xunit;

namespace Services.ProductService.Tests
{
    public class ProductCatalogTests
    {
        //    [Fact]
        //    public void InsertDataTest()
        //    {
        //        var author = new Author { Id = 1, FirstName = "Joydip", LastName = "Kanjilal" };
        //        using (var context = new IDGDbContext(options))
        //        {
        //            context.Authors.Add(author);
        //            context.SaveChanges();
        //        }
        //        Author obj;
        //        using (var context = new IDGDbContext(options))
        //        {
        //            obj = context.Authors.FirstOrDefault(x => x.Id == author.Id);
        //        }
        //        Assert.AreEqual(author.FirstName, obj.FirstName);
        //    }

        //[Fact(DisplayName = "Index should return default view")]
        //public void Index_should_return_default_view()
        //{
        //    using (var context = GetContextWithData())
        //    using (var controller = new HomeController(context))
        //    {
        //        var result = controller.Index() as ViewResult;

        //        Assert.NotNull(result);
        //        Assert.True(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Index");
        //    }
        //}

        [Fact(DisplayName = "Index should return valid model")]
        public async Task Index_should_return_valid_model()
        {
            using (var context = GetContextWithData())
            {
                var repository = new RepositoryWrapper(context);
                //var mockRepository = new Mock<IRepositoryWrapper>();
                //mockRepository.Setup(x => x.Product)
                //var services = new ProductCatalogService(mockRepository.Object);

                var services = new ProductCatalogService(repository);
                //var mockProductObject = new Mock<ICreateProductCommand>();
                //mockProductObject.(x => x = context.Products[0])

                //await services.CreateProduct()


                //var dto = await services.CreateProduct(mockProductObject.Object);
            }
        }

        private ProductDbContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new ProductDbContext(options);

            context.Products.Add(new Product
            {
                Id = Guid.NewGuid(),
                Code = "Code 1",
                Name = "Name 1",
                Description = "No Description",
                Price = 10.55
            });

            context.SaveChanges();
            return context;
        }
    }
}