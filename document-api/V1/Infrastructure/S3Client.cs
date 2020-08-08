using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using document_api.V1.Boundary;
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

            foreach (var file in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = file.OpenReadStream(),
                    Key = file.FileName,
                    CannedACL = S3CannedACL.NoACL
                };

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
    }

}
