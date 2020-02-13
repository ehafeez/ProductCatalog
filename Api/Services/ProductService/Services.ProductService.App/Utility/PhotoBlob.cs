using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Services.ProductService.App.Infrastructure.Settings;

namespace Services.ProductService.App.Utility
{
    public class PhotoBlob
    {
        private CloudBlockBlob _blockBlob;
        private readonly CloudBlobClient _blobClient;
        private readonly BlobStorageSettings _blobStorageSettings;
        public PhotoBlob(BlobStorageSettings blobStorageSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(blobStorageSettings.ConnectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
            _blobStorageSettings = blobStorageSettings;
        }

        public async Task DeleteBlob(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return;

                var fileName = Path.GetFileName(path);
                var dbContainer = _blobClient.GetContainerReference(_blobStorageSettings.PhotoContainerReference);
                _blockBlob = dbContainer.GetBlockBlobReference(fileName);
                await _blockBlob.DeleteIfExistsAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<byte[]> DownloadBlob(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path)) return null;
                
                var fileName = Path.GetFileName(path);
                var dbContainer = _blobClient.GetContainerReference(_blobStorageSettings.PhotoContainerReference);
                _blockBlob = dbContainer.GetBlockBlobReference(fileName);

                await _blockBlob.FetchAttributesAsync();
                byte[] byteArray = new byte[_blockBlob.Properties.Length];
                await _blockBlob.DownloadToByteArrayAsync(byteArray, 0);
                return byteArray;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}