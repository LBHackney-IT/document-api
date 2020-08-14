using document_api.V1.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.UseCase
{
    public interface IGetFileUsecase
    {
        Task<GetFileResponse> Execute(GetFileRequest request);
    }

}
