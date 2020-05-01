using System;
namespace documentapi.V1.Gateways
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

            return new AddFileResponse;

        }
    }
}
