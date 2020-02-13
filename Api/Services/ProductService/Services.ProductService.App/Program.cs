using System;
using Microsoft.Extensions.Configuration;
using Services.ProductService.App.Infrastructure;
using Services.ProductService.App.Infrastructure.ServiceBusConfigurator;
using Services.ProductService.App.Infrastructure.Settings;

namespace Services.ProductService.App
{
    class Program
    {
        private static readonly string ServiceName = "Services.ProductService";
        private static IConfigurationRoot _config;
        private static InfrastructureSettings _infrastructureSettings;

        static void Main(string[] args)
        {
            try
            {
                Console.Title = ServiceName;
                _config = Config.Build();

                var serviceBusSettings = new ServiceBusSettings();
                var serviceBusQueuesSettings = new ServiceBusQueuesSettings();
                var connectionStringSettings = new ConnectionStringSettings();
                var blobSettings = new BlobStorageSettings();
                
                _config.GetSection("ServiceBus").Bind(serviceBusSettings);
                _config.GetSection("ServiceBusQueues").Bind(serviceBusQueuesSettings);
                _config.GetSection("ConnectionStrings").Bind(connectionStringSettings);
                _config.GetSection("BlobStorage").Bind(blobSettings);

                _infrastructureSettings = InfrastructureSettings.Create(connectionStringSettings,
                    serviceBusQueuesSettings, serviceBusSettings, blobSettings);

                DisplayConfiguration();

                ProductServiceBus.Instance.GetProductServiceBus();
                Console.ReadLine();

                ProductServiceBus.Instance.StopProductServiceBus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }

        public static void DisplayConfiguration()
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine($"ServiceBusConnectionString: {_infrastructureSettings.ServiceBusSettings.ServiceBusConnectionString}");
            Console.WriteLine($"ServiceBusKeyName: {_infrastructureSettings.ServiceBusSettings.ServiceBusKeyName}");
            Console.WriteLine($"ServiceBusNameSpace: {_infrastructureSettings.ServiceBusSettings.ServiceBusNameSpace}");
            Console.WriteLine($"ServiceBusScheme: {_infrastructureSettings.ServiceBusSettings.ServiceBusScheme}");
            Console.WriteLine($"ServiceBusSharedAccessKey: {_infrastructureSettings.ServiceBusSettings.ServiceBusSharedAccessKey}");
            Console.WriteLine("");
            Console.WriteLine($"UploadServiceQueue: {_infrastructureSettings.ServiceBusQueuesSettings.ProductServiceQueue}");
            Console.WriteLine($"DefaultConnection: {_infrastructureSettings.ConnectionStringSettings.DefaultConnection}");
            Console.WriteLine("-----------------------------------------------------------------------");
        }
    }
}