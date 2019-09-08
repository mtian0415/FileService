using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
namespace FileService.Controllers
{
    public class BlobManager
    {
        public BlobManager()
        {
        }

        public CloudStorageAccount InitializeAccount()
        {
            string connectionString = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            return storageAccount;
        }

        public async Task<bool> UploadFile(string containerName, IFormFile file)
        {
            var container = this.GetContainer(containerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
            blockBlob.Properties.ContentType = file.ContentType;
            Stream inputFileStream = file.OpenReadStream();
            using (var fileStream = inputFileStream)
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
                return true;
            }
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobClient blobClient = InitializeAccount().CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExistsAsync();

            return container;
        }

    }
}