using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using document_api.V1.Boundary;
using document_api.V1.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_api.V1.Infrastructure
{
    public class S3Client : IS3Client
    {
        private readonly IAmazonS3 _s3Client;

        public S3Client(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;

        }
        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
        {
            var response = new List<string>();

            try
            {

                foreach (var file in formFiles)
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        InputStream = file.OpenReadStream(),
                        Key = file.FileName,
                        CannedACL = S3CannedACL.NoACL
                    };

                    uploadRequest.Metadata.Add("test", "testing-metadata");

                    using (var fileTransferUtility = new TransferUtility(_s3Client))
                    {
                        await fileTransferUtility.UploadAsync(uploadRequest);

                        var urlRequest = new GetPreSignedUrlRequest
                        {
                            BucketName = bucketName,
                            Key = file.FileName,
                            Expires = DateTime.Now.AddMinutes(10)
                        };

                        var url = _s3Client.GetPreSignedURL(urlRequest);

                        response.Add(url);
                    };


                }

                return new AddFileResponse
                {
                    PreSignedUrl = response
                };
            }
            catch (AmazonS3Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetFileResponse> DownloadFile(string bucketName, string fileName)
        {
            var response = new List<KeyValuePair<string, string>>();

            try
            {
                var pathAndFileName = $"C:\\S3Test\\{fileName}";

                var downloadRequest = new TransferUtilityDownloadRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    FilePath = pathAndFileName
                };

                using (var transferUtility = new TransferUtility(_s3Client))
                {
       
                    await transferUtility.DownloadAsync(downloadRequest);

                    var metadataRequest = new GetObjectMetadataRequest
                    {
                        BucketName = bucketName,
                        Key = fileName
                    };

                    var metadata = await _s3Client.GetObjectMetadataAsync(metadataRequest);

                    foreach(string key in metadata.Metadata.Keys)
                    {
                        response.Add(new KeyValuePair<string,string>(key, metadata.Metadata[key]));
                    }

                    return new GetFileResponse
                    {
                        Metadata = response
                    };
                }

               
            }
            catch(AmazonS3Exception ex)
            {
                throw ex;
            }
        }
    }

}
