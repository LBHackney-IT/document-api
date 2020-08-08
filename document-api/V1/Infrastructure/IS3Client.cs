using document_api.V1.Boundary;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.Infrastructure
{
    public interface IS3Client
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);
    }

}
