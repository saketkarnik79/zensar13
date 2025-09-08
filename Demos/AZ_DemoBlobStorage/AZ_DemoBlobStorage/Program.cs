using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AZ_DemoBlobStorage
{
    internal class Program
    {
        //private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=skzendemostorage;AccountKey=Gzw8kK+IT0WY+DxIXMHHKKpwwpWOanHXguiy0vyMMrpucqMyyMeBQHCmDi2G/b1zzt9r5SHb6Wi+AStFqObkw==;EndpointSuffix=core.windows.net";
        private static readonly string containerName = "democontainer";
        private static readonly string blobName = "demoblob.txt";
        private readonly static string localFilePath = "demoblob.txt";

        static async Task Main(string[] args)
        {
            // Create a BlobServiceClient
            var blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container if it doesn't exist
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            Console.WriteLine($"Container '{containerName}' is ready.");

            // Create: Upload a blob
            var blobClient = containerClient.GetBlobClient(blobName);
            await File.WriteAllTextAsync(localFilePath, "Hello, Azure Blob Storage!");
            var uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
            // Set metadata
            var metadata = new Dictionary<string, string>
            {
                {"author", "Saket" },
                {"environment", "production" }
            };
            await blobClient.SetMetadataAsync(metadata);
            Console.WriteLine("Metadata set.");
            Console.WriteLine("Blob uploaded.");

            //Read: Download the blob
            var downloadFilePath = localFilePath.Replace(".txt", "_DOWNLOADED.txt");
            var downloadStream=File.OpenWrite(downloadFilePath);
            await blobClient.DownloadToAsync(downloadStream);
            downloadStream.Close();

            BlobProperties properties=await blobClient.GetPropertiesAsync();
            Console.WriteLine("Retrieved metdata:");
            foreach (var item in properties.Metadata)
            {
                Console.WriteLine($"  {item.Key}: {item.Value}");
            }

            //List blobs in the container
            Console.WriteLine("Listing blobs in container:");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                Console.WriteLine($"  {blobItem.Name}");
            }

            //list blobs with criteria
            Console.WriteLine("Listing blobs with criteria (prefix 'demo'):");
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync(prefix: "demo"))
            {
                Console.WriteLine($"  {blobItem.Name}");
            }

            // Delete: Delete the blob
            await blobClient.DeleteIfExistsAsync();
            Console.WriteLine("Blob deleted.");

            // Clean up local files
            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
            if (File.Exists(downloadFilePath))
            {
                File.Delete(downloadFilePath);
            }

            Console.ReadKey();


        }
    }
}
