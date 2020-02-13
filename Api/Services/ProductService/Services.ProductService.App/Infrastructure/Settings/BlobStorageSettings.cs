namespace Services.ProductService.App.Infrastructure.Settings
{
    public class BlobStorageSettings
    {
        public string AdapterName { get; set; }
        public string ConnectionString { get; set; }
        public string PhotoContainerReference { get; set; }
    }
}