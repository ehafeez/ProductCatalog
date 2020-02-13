using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Implementations;
using Services.ProductService.App.Infrastructure;
using Services.ProductService.App.Infrastructure.AzureStorage;
using Services.ProductService.App.Infrastructure.Settings;
using Services.ProductService.App.MessagingFramework;
using Services.ProductService.App.Utility;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;

namespace Services.ProductService.App.Consumers
{
    public class UpdateProductConusmer : IConsumer<IUpdateProductCommand>
    {
        private readonly IConfigurationRoot _configurationRoot;

        public UpdateProductConusmer()
        {
            _configurationRoot = Config.Build();
        }

        public async Task Consume(ConsumeContext<IUpdateProductCommand> context)
        {
            try
            {
                var connectionStringSettings = new ConnectionStringSettings();
                var blobStorageSettings = new BlobStorageSettings();
                _configurationRoot.GetSection("ConnectionStrings").Bind(connectionStringSettings);
                _configurationRoot.GetSection("BlobStorage").Bind(blobStorageSettings);

                var dbContext = ProductDbContext.GetProductDbContext(connectionStringSettings.DefaultConnection);
                var repository = new RepositoryWrapper(dbContext);

                var product = repository.Product.GetProductById(context.Message.Id);
                if (product != null)
                {
                    //delete existing blob
                    var deleteblob = new PhotoBlob(blobStorageSettings);
                    await deleteblob.DeleteBlob(product.BlobName);

                    //upload photo to blob
                    var repo = BlobConfigurator.GetMessageDataRepository(blobStorageSettings);
                    var bytesArray = Convert.FromBase64String(context.Message.Photo);
                    var payload = repo.PutBytes(bytesArray).Result;
                    context.Message.BlobName = payload.Address.AbsolutePath;
                }

                var productService = new ProductCatalogService(repository);
                var result = await productService.UpdateProduct(context.Message);
                var updatedEvent = new ProductUpdatedEvent(result);
                await context.RespondAsync(updatedEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}