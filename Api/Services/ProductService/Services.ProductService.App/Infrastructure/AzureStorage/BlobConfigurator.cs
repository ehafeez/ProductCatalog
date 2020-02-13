using MassTransit.MessageData.Enchilada;
using Enchilada.Azure.BlobStorage;
using Services.ProductService.App.Infrastructure.Settings;

namespace Services.ProductService.App.Infrastructure.AzureStorage
{
    public static class BlobConfigurator
    {
        public static EnchiladaMessageDataRepository GetMessageDataRepository(BlobStorageSettings settings)
        {
            var adapter = new BlobStorageAdapterConfiguration
            {
                AdapterName = settings.AdapterName,
                CreateContainer = true,
                ConnectionString = settings.ConnectionString,
                ContainerReference = settings.PhotoContainerReference
            };

            var factory = new EnchiladaMessageDataRepositoryFactory();
            var messageDataRepository = factory.Create(adapter);
            return messageDataRepository;
        }
    }
}