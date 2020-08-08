using System.Collections.Generic;
using System.Threading.Tasks;
using document_api.V1.Boundary;
using document_api.V1.Gateways;
using Microsoft.AspNetCore.Http;

namespace document_api.V1.UseCase
{
    public class UploadFileUsecase : IUploadFile
    {
        private IS3FileGateway _fileGateway;

        public UploadFileUsecase(IS3FileGateway fileGateway)
        {
            _fileGateway = fileGateway;
        }


        public Task<AddFileResponse> Execute(string bucketName, IList<IFormFile> formFiles)
        {
           return _fileGateway.UploadFiles(bucketName, formFiles);
        }
    }
}
