using System;
namespace documentapi.V1.UseCase
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
            _fileGateway.UploadFiles(string bucketName, IFormFile formFile);
        }
    }
}
