using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
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

            return new AddFileResponse();

        }
    }
}
