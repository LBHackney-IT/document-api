using document_api.V1.Boundary;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.Tests.V1.Helper
{
    public static class TestHelper
    {
        public static IList<IFormFile> Generate_FileMock()
        {
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var testFiles = new List<IFormFile>
            {
                fileMock.Object
            };

            return testFiles;
        }

        public static GetFileRequest Generate_ValidGetRequest()
        {
            return new GetFileRequest
            {
                bucketName = "testbucket",
                fileName = "Test.pdf"
            };

        }

        public static GetFileRequest Generate_WrongBucketGetRequest()
        {
            return new GetFileRequest
            {
                bucketName = "wrongbucket",
                fileName = "Test.pdf"
            };

        }

        public static GetFileRequest Generate_WrongFilenameGetRequest()
        {
            return new GetFileRequest
            {
                bucketName = "testbucket",
                fileName = "noFile"
            };

        }
    }

}
