using document_api.V1.Boundary;
using document_api.V1.Gateways;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.UseCase
{
    public class GetFileUsecase : IGetFileUsecase
    {
        private IS3FileGateway _fileGateway;

        public GetFileUsecase(IS3FileGateway fileGateway)
        {
            _fileGateway = fileGateway;
        }

        public Task<GetFileResponse> Execute(GetFileRequest request)
        {
           return  _fileGateway.DownloadFile(request);
        }
    }

}
