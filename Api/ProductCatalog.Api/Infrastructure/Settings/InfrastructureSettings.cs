namespace ProductCatalog.Api.Infrastructure.Settings
{
    public class InfrastructureSettings
    {
        public ConnectionStringSettings ConnectionStringSettings { get; private set; }
        public ServiceBusQueuesSettings ServiceBusQueuesSettings { get; private set; }
        public ServiceBusSettings ServiceBusSettings { get; private set; }

        private InfrastructureSettings()
        {
            ConnectionStringSettings = new ConnectionStringSettings();
            ServiceBusQueuesSettings = new ServiceBusQueuesSettings();
            ServiceBusSettings = new ServiceBusSettings();
        }

        public static InfrastructureSettings Create(ConnectionStringSettings connectionStringSettings,
            ServiceBusQueuesSettings serviceBusQueuesSettings, ServiceBusSettings serviceBusSettings)
        {
            var infrastructureSettings = new InfrastructureSettings
            {
                ConnectionStringSettings = connectionStringSettings,
                ServiceBusQueuesSettings = serviceBusQueuesSettings,
                ServiceBusSettings = serviceBusSettings,
            };
            return infrastructureSettings;
        }
    }
}