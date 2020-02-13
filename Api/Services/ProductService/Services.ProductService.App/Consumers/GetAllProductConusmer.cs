using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Implementations;
using Services.ProductService.App.Infrastructure;
using Services.ProductService.App.Infrastructure.Settings;
using Services.ProductService.App.MessagingFramework;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;

namespace Services.ProductService.App.Consumers
{
    public class GetAllProductConusmer : IConsumer<IGetAllProductsCommand>
    {
        private readonly IConfigurationRoot _configurationRoot;

        public GetAllProductConusmer()
        {
            _configurationRoot = Config.Build();
        }

        public async Task Consume(ConsumeContext<IGetAllProductsCommand> context)
        {
            try
            {
                var connectionStringSettings = new ConnectionStringSettings();
                _configurationRoot.GetSection("ConnectionStrings").Bind(connectionStringSettings);
                var dbContext = ProductDbContext.GetProductDbContext(connectionStringSettings.DefaultConnection);
                var repository = new RepositoryWrapper(dbContext);

                var productService = new ProductCatalogService(repository);
                var result = await productService.GetAllProducts(context.Message);
                var getProductEvent = new AllProductRetrievedEvent(result);
                await context.RespondAsync(getProductEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}