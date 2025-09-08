using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Identity;

namespace AZ_DemoStorageWithManagedIdentity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageApiController : ControllerBase
    {
        private readonly string blobServiceUri = "https://skzendemostorage.blob.core.windows.net/";
        private readonly string containerName = "demo-container";
        private readonly string blobName = "sample.txt";
        private readonly string userManagedIdentityClientId= "37e2cfeb-d3db-41fd-8b10-05d7f2fe26b0";

        [HttpGet]
        public async Task< IActionResult> Get()
        {
            //Authenticate using User-Assigned Managed Identity
            var credential = new ManagedIdentityCredential(userManagedIdentityClientId);

            //Create a BlobServiceClient using the Managed Identity credential
            var blobServiceClient = new BlobServiceClient(new Uri(blobServiceUri), credential);

            //Get a reference to the container and blob
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            containerClient.CreateIfNotExists();

            var blobClient = containerClient.GetBlobClient(blobName);
            

            //upload a sample text file to the blob
            var content = "Hello, this is a sample text file uploaded using Managed Identity!";
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)))
            {
                blobClient.Upload(stream, overwrite: true);
            }

            //Download & read the blob content
            var downloadInfo = await blobClient.DownloadContentAsync();
            var downloadedContent = downloadInfo.Value.Content.ToString();
            return Ok(new { Message = "File uploaded and downloaded successfully!", Content = downloadedContent });
        }
    }
}
