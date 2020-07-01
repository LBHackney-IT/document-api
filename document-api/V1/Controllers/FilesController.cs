using System;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using document_api.V1.Boundary;
using document_api.Controllers.V1;
using Microsoft.AspNetCore.Http;
using document_api.V1.UseCase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace document_api.V1.Controllers
{
    [Route("api/v1/files")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IUploadFile _uploadFile;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IUploadFile uploadFile, ILogger<FilesController> logger)
        {
            _uploadFile = uploadFile;
            _logger = logger;
        }


        [HttpPost]
        [Route("{bucketName}/add")]
        public async Task<ActionResult<AddFileResponse>> AddFiles(string bucketName, IList<IFormFile> formFiles)
        {
            if (formFiles == null)
            {
                return BadRequest("The request doesn't contain any files to be uploaded.");
            }

            var response =  await _uploadFile.Execute(bucketName, formFiles);

            if (response == null)
            {
                return BadRequest();
            }

            return CreatedAtAction("AddFiles", response);
        }
    }
}
