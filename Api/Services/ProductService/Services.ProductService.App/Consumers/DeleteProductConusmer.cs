using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using ProductCatalog.Api.Messaging.Commands;
using Services.ProductService.App.Implementations;
using Services.ProductService.App.Infrastructure;
using Services.ProductService.App.Infrastructure.Settings;
using Services.ProductService.App.MessagingFramework;
using Services.ProductService.App.Utility;
using Services.ProductService.Data;
using Services.ProductService.Repository.Implementations;

namespace Services.ProductService.App.Consumers
{
    public class DeleteProductConusmer : IConsumer<IDeleteProductCommand>
    {
        private readonly IConfigurationRoot _configurationRoot;

        public DeleteProductConusmer()
        {
            _configurationRoot = Config.Build();
        }

        public async Task Consume(ConsumeContext<IDeleteProductCommand> context)
        {
            try
            {
                var connectionStringSettings = new ConnectionStringSettings();
                var blobStorageSettings = new BlobStorageSettings();

                _configurationRoot.GetSection("ConnectionStrings").Bind(connectionStringSettings);
                _configurationRoot.GetSection("BlobStorage").Bind(blobStorageSettings);

                var dbContext = ProductDbContext.GetProductDbContext(connectionStringSettings.DefaultConnection);
                var repository = new RepositoryWrapper(dbContext);

                var product = repository.Product.GetProductById(Guid.Parse(context.Message.ProductId));
                if (product != null)
                {
                    var deleteblob = new PhotoBlob(blobStorageSettings);
                    await deleteblob.DeleteBlob(product.BlobName);
                }

                var productService = new ProductCatalogService(repository);
                var result = await productService.DeleteProduct(context.Message.ProductId);
                var deletedEvent = new ProductDeletedEvent(result);
                await context.RespondAsync(deletedEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}