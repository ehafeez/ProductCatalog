using System;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Services.ProductService.App.Consumers;
using Services.ProductService.App.Infrastructure.Settings;

namespace Services.ProductService.App.Infrastructure.ServiceBusConfigurator
{
    public class ProductServiceBus
    {
        private static ProductServiceBus _instance;
        private static IConfigurationRoot _config;
        private static IBusControl _bus;

        private ProductServiceBus()
        {
        }

        public static ProductServiceBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductServiceBus();
                    _config = Config.Build();
                    ConfigureServiceBus();
                }

                return _instance;
            }
        }

        public IBusControl GetProductServiceBus()
        {
            if (_bus == null)
            {
                ConfigureServiceBus();
            }

            return _bus;
        }

        public void StopProductServiceBus()
        {
            _bus?.Stop();
        }

        private static void ConfigureServiceBus()
        {
            var serviceBusSettings = new ServiceBusSettings();
            _config.GetSection("ServiceBus").Bind(serviceBusSettings);

            var serviceBusQueuesSettings = new ServiceBusQueuesSettings();
            _config.GetSection("ServiceBusQueues").Bind(serviceBusQueuesSettings);

            _bus = BusConfigurator.ConfigureBus(serviceBusSettings,
                (sbc, host) =>
                {
                    sbc.ReceiveEndpoint(serviceBusQueuesSettings.ProductServiceQueue,
                        e =>
                        {
                            e.Consumer<CreateProductConusmer>();
                            e.Consumer<UpdateProductConusmer>();
                            e.Consumer<DeleteProductConusmer>();
                            e.Consumer<GetProductConusmer>();
                            e.Consumer<GetAllProductConusmer>();
                        });
                });
            try
            {
                Console.WriteLine("Starting Service Bus....");
                _bus.Start();
                Console.WriteLine("Service Bus Started....");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}