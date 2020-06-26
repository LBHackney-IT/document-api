using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using document_api.V1.Boundary;
using System.Threading.Tasks;

namespace document_api.V1.Gateways
{
    public interface IS3FileGateway
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);
    }
}
