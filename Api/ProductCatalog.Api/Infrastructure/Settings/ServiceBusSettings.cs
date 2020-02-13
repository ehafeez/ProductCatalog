namespace ProductCatalog.Api.Infrastructure.Settings
{
    public class ServiceBusSettings
    {
        public string ServiceBusScheme { get; set; }
        public string ServiceBusNameSpace { get; set; }
        public string ServiceBusKeyName { get; set; }
        public string ServiceBusSharedAccessKey { get; set; }
        public string ServiceBusConnectionString { get; set; }

    }
}