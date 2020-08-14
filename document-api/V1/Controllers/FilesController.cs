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
using Amazon.Runtime.Internal;
using document_api.V1.Exceptions;

namespace document_api.V1.Controllers
{
    [Route("api/v1/files")]
    [ApiController]
    public class FilesController : BaseController
    {
        private readonly IUploadFile _uploadFile;
        private readonly IGetFileUsecase _getFile;
        private readonly ILogger<FilesController> _logger;

        public FilesController(IUploadFile uploadFile, IGetFileUsecase getFile, ILogger<FilesController> logger)
        {
            _uploadFile = uploadFile;
            _getFile = getFile;
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
                return BadRequest("Something went wrong with your request");
            }

            return CreatedAtAction("AddFiles", response);
        }

        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<ActionResult<GetFileResponse>> GetFile([FromRoute] GetFileRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest();
                }

               var response = await _getFile.Execute(request);

                return Ok(response);
            }
            catch(AmazonS3Exception ex)
            {
                if(ex.ErrorCode == "NoSuchKey" || ex.ErrorCode == "NoSuchBucket")
                {
                    return NotFound(new ErrorsResponse(ex.Message));
                }

                return StatusCode(500, new ErrorsResponse(ex.Message));    
            }
           
        }
    }
}
