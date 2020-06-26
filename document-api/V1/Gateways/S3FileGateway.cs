using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Transfer;
using document_api.V1.Boundary;
using Microsoft.AspNetCore.Http;

namespace document_api.V1.Gateways
{
    public class S3FileGateway : IS3FileGateway
    {
        private readonly IAmazonS3 _s3Client;

        public S3FileGateway(IAmazonS3 s3Client)
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
                };
            }

            return new AddFileResponse(response);

        }
    }
}
