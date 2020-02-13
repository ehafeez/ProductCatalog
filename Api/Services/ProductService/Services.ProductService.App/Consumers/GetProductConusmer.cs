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
    public class GetProductConusmer : IConsumer<IGetProductCommand>
    {
        private readonly IConfigurationRoot _configurationRoot;

        public GetProductConusmer()
        {
            _configurationRoot = Config.Build();
        }

        public async Task Consume(ConsumeContext<IGetProductCommand> context)
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
                var result = await productService.GetProduct(context.Message.ProductId);
                var getProductEvent = new ProductRetrievedEvent(result);

                if (getProductEvent.ProductDto != null)
                {
                    var phtoBlob = new PhotoBlob(blobStorageSettings);
                    var byteArray = await phtoBlob.DownloadBlob(getProductEvent.ProductDto.BlobName);
                    getProductEvent.ProductDto.Photo = byteArray;
                }

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