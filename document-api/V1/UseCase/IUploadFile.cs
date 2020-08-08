using Microsoft.AspNetCore.Http;
using document_api.V1.Boundary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_api.V1.UseCase
{
    public interface IUploadFile
    {
        Task<AddFileResponse> Execute(string bucketName, IList<IFormFile> formFile);
    }
}
