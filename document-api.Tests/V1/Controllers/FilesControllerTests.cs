using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using document_api.V1.Controllers;
using Microsoft.Extensions.Logging;
using document_api.V1.UseCase;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using System.Threading.Tasks;
using document_api.V1.Boundary;
using document_api.Tests.V1.Helper;

namespace UnitTests.V1.Controllers
{
    [TestFixture]
    public class FilesControllerTests
    {
        private FilesController _filesController;
        private Mock<IUploadFile> _mockUseCase;
        private Mock<ILogger<FilesController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockUseCase = new Mock<IUploadFile>();
            _mockLogger = new Mock<ILogger<FilesController>>();
            _filesController = new FilesController(_mockUseCase.Object, _mockLogger.Object);
        }

        #region Upload S3 File
        [Test]
        public async Task Given_valid_request_when_postFilesController_method_is_called_then_usecase_is_called()
        {
            //arrange
            var testBucket = "test";
            var testFile = TestHelper.Generate_FileMock();
            var testFiles = new List<IFormFile>
            {
                testFile
            };

            //act
            await _filesController.AddFiles(testBucket, testFiles);
            //assert
            _mockUseCase.Verify(u => u.Execute(It.IsAny<string>(), It.IsAny<IList<IFormFile>>()), Times.Once);
        }

        [Test]
        public async Task Given_valid_request_when_postFilesController_method_is_called_then_it_returns_201_Created_response()
        {
            //arrange
            var expectedResponseCode = 201;
            var testBucket = "test";
            var response = new AddFileResponse { PreSignedUrl = new List<string> { "test" } };
            var testFile = TestHelper.Generate_FileMock();
            var testFiles = new List<IFormFile>
            {
                testFile
            };
            _mockUseCase.Setup(x => x.Execute(testBucket, testFiles)).ReturnsAsync(response);

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, testFiles);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
        }

        [Test]
        public async Task Given_an_invalid_request_when_postFilesController_method_is_called_then_it_returns_400_BadRequest_response()
        {
            //arrange
            var expectedResponseCode = 400;
            var testBucket = "test";

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, null);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
            Assert.AreEqual("The request doesn't contain any files to be uploaded.", result.Value);
        }

       [Test]
       public async Task Given_an_valid_request_but_an_invalid_response_when_postFilesController_method_is_called_then_it_returns_400_BadRequest_response()
        {
            //arrange
            var expectedResponseCode = 400;
            var testBucket = "test";
            var testFile = TestHelper.Generate_FileMock();
            var testFiles = new List<IFormFile>
            {
                testFile
            };
            _mockUseCase.Setup(x => x.Execute(testBucket, testFiles)).Returns(Task.FromResult<AddFileResponse>(null));

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, testFiles);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
        }

        #endregion

    }

}
