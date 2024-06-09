using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Common.Services
{
    public class GoogleService
    {
        public async Task<Google.Apis.Storage.v1.Data.Object> UploadFileToGoogleCloudStorage(IFormFile formFile)
        {
            string bucketName = "nhom14landscape";

            // Path to your service account key file
            string fileJson = @"C:\Users\son van\Documents\daphuongtien\able-coast-422816-v5-9962a913a137.json";
            GoogleCredential googleCredential = GoogleCredential.FromFile(fileJson);

            // Create a Storage client
            var storageClient = StorageClient.Create(googleCredential);

            // File name for the object in the bucket
            string objectName = formFile.FileName;

            // Get the file stream from the form file
            using (var fileStream = formFile.OpenReadStream())
            {
                // Upload the file stream directly to Google Cloud Storage
                var uploadObject = await storageClient.UploadObjectAsync(bucketName, objectName, null, fileStream);
                Console.WriteLine($"Uploaded {objectName} to bucket {bucketName}.");

                // Print the media link of the uploaded object
                Console.WriteLine($"Media link: {uploadObject.MediaLink}");

                return uploadObject;
            }
        }
    }
}
