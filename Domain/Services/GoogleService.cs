using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class GoogleService
    {
       

      
            public async Task<Google.Apis.Storage.v1.Data.Object> CreateBucket(string projectId = "able-coast-422816-v5", string bucketName = "nhom14landscape")
            {
                

                
                
                string fileJson = @"C:\Users\son van\Documents\daphuongtien\able-coast-422816-v5-9962a913a137.json";

                GoogleCredential google = GoogleCredential.FromFile(fileJson);

                var storage = StorageClient.Create(google);

                string objectName = "OIP (23).jpg";

                var fileStream = File.OpenRead(@"C:\Users\son van\Documents\daphuongtien\OIP (23).jpg");

                var url = await storage.UploadObjectAsync(bucketName, objectName, null, fileStream);

                Console.WriteLine($"link laf : {url.MediaLink}");


                Console.WriteLine($"Uploaded {objectName}.");

                Console.WriteLine($"Creaet {bucketName}");

                return url;


            }





        





    }
}
