using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Implementations;
using Services.ProductService.App.Infrastructure;
using Services.ProductService.App.Infrastructure.AzureStorage;
using Services.ProductService.App.Infrastructure.Settings;
using Services.ProductService.App.MessagingFramework;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;

namespace Services.ProductService.App.Consumers
{
    public class CreateProductConusmer : IConsumer<ICreateProductCommand>
    {
        private readonly IConfigurationRoot _configurationRoot;

        public CreateProductConusmer()
        {
            _configurationRoot = Config.Build();
        }

        public async Task Consume(ConsumeContext<ICreateProductCommand> context)
        {
            try
            {
                var connectionStringSettings = new ConnectionStringSettings();
                var blobStorageSettings = new BlobStorageSettings();
                _configurationRoot.GetSection("ConnectionStrings").Bind(connectionStringSettings);
                _configurationRoot.GetSection("BlobStorage").Bind(blobStorageSettings);

                var dbContext = ProductDbContext.GetProductDbContext(connectionStringSettings.DefaultConnection);
                var repository = new RepositoryWrapper(dbContext);
                var productService = new ProductCatalogService(repository);

                var product = repository.Product.GetByCondition(x => x.Code.Equals(context.Message.Code) || x.Name.Equals(context.Message.Name));
                if (product?.Count() == 0)
                {
                    //upload photo to blob
                    var repo = BlobConfigurator.GetMessageDataRepository(blobStorageSettings);
                    var bytesArray = Convert.FromBase64String(context.Message.Photo);
                    var payload = repo.PutBytes(bytesArray).Result;
                    context.Message.BlobName = payload.Address.AbsolutePath;
                }

                //create product
                var result = await productService.CreateProduct(context.Message);
                var createdEvent = new ProductCreatedEvent(result);
                await context.RespondAsync(createdEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}