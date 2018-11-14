using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace FilesOnCloud.Handlers
{
    public class AWSHandler : ICloudFileHandler
    {
        private static IAmazonS3 s3Client;
        private string bucketName;
        //private static readonly RegionEndpoint bucketRegion = ~~RegionEndpoint.USWest2~~;
        public AWSHandler(RegionEndpoint region, string bucketName)
        {
            this.bucketName = bucketName;
            s3Client = new AmazonS3Client(region);
            
        }
        public Task<Stream> Download(string folder, string file)
        {
            throw new NotImplementedException();
        }

        public async Task Upload(string folder, string file, byte[] content)
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(new MemoryStream(content),
                                               bucketName, folder+file);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        static async Task ListAsync(string folderName)
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = folderName,
                    MaxKeys = 2
                };
                do
                {
                    ListObjectsResponse response = await s3Client.ListObjectsAsync(request);
                    // Process the response.
                    foreach (Amazon.S3.Model.S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                    }

                    // If the response is truncated, set the marker to get the next 
                    // set of keys.
                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                } while (request != null);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }
    }
}