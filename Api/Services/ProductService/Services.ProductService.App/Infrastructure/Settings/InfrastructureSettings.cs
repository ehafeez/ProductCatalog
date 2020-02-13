namespace Services.ProductService.App.Infrastructure.Settings
{
    public class InfrastructureSettings
    {
        public ConnectionStringSettings ConnectionStringSettings { get; private set; }
        public ServiceBusQueuesSettings ServiceBusQueuesSettings { get; private set; }
        public ServiceBusSettings ServiceBusSettings { get; private set; }
        public BlobStorageSettings BlobStorageSettings { get; private set; }

        private InfrastructureSettings()
        {
            ConnectionStringSettings = new ConnectionStringSettings();
            ServiceBusQueuesSettings = new ServiceBusQueuesSettings();
            ServiceBusSettings = new ServiceBusSettings();
            BlobStorageSettings = new BlobStorageSettings();
        }

        public static InfrastructureSettings Create(ConnectionStringSettings connectionStringSettings,
            ServiceBusQueuesSettings serviceBusQueuesSettings, ServiceBusSettings serviceBusSettings,
            BlobStorageSettings blobStorageSettings)
        {
            var infrastructureSettings = new InfrastructureSettings
            {
                ConnectionStringSettings = connectionStringSettings,
                ServiceBusQueuesSettings = serviceBusQueuesSettings,
                ServiceBusSettings = serviceBusSettings,
                BlobStorageSettings = blobStorageSettings
            };

            return infrastructureSettings;
        }
    }
}