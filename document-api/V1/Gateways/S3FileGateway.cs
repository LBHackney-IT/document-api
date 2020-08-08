using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using document_api.V1.Boundary;
using document_api.V1.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace document_api.V1.Gateways
{
    public class S3FileGateway : IS3FileGateway
    {
        private readonly IS3Client _s3Client;

        public S3FileGateway(IS3Client s3Client)
        {
            _s3Client = s3Client;

        }

        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
        {
             return await  _s3Client.UploadFiles(bucketName, formFiles);

        }
    }
}
