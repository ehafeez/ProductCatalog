using System.IO;
using Microsoft.Extensions.Configuration;

namespace Services.ProductService.App.Infrastructure
{
    public class Config
    {
        public static IConfigurationRoot Build()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", false, true);
            return configBuilder.Build();
        }
    }
}