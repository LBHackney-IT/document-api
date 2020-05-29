using System;
using Microsoft.AspNetCore.Http;

namespace document_api.UseCase.V1
{
    public class UploadFileUsecase : IUploadFileUsecase
    {
        private IFileGateway _fileGateway;

        public UploadFileUsecase(IFileGateway fileGateway)
        {
            _fileGateway = fileGateway;
        }


        public void Execute(string bucketName, IFormFile formFile)
        {
            _fileGateway.UploadFiles(bucketName, formFile);
        }
    }
}
