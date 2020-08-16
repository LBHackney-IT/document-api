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
using Amazon.S3;
using document_api.V1.Exceptions;
using System.Linq;

namespace UnitTests.V1.Controllers
{
    [TestFixture]
    public class FilesControllerTests
    {
        private FilesController _filesController;
        private Mock<IUploadFile> _mockPostUseCase;
        private Mock<IGetFileUsecase> _mockGetUseCase;
        private Mock<ILogger<FilesController>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockPostUseCase = new Mock<IUploadFile>();
            _mockGetUseCase = new Mock<IGetFileUsecase>();
            _mockLogger = new Mock<ILogger<FilesController>>();
            _filesController = new FilesController(_mockPostUseCase.Object, _mockGetUseCase.Object, _mockLogger.Object);
        }

        #region Upload S3 File
        [Test]
        public async Task Given_a_valid_request_when_postFilesController_method_is_called_then_usecase_is_called()
        {
            //arrange
            var testBucket = "test";
            var testFiles = TestHelper.Generate_FileMock();
           
            //act
            await _filesController.AddFiles(testBucket, testFiles);

            //assert
            _mockPostUseCase.Verify(u => u.Execute(It.IsAny<string>(), It.IsAny<IList<IFormFile>>()), Times.Once);
        }

        [Test]
        public async Task Given_a_valid_request_when_postFilesController_method_is_called_then_controller_calls_the_usecase_with_the_same_request()
        {
            //arrange
            var testBucket = "test";
            var testFiles = TestHelper.Generate_FileMock();

            //act
            await _filesController.AddFiles(testBucket, testFiles);

            //assert
            _mockPostUseCase.Verify(u => u.Execute(It.Is<string>(str => str == testBucket), It.Is<IList<IFormFile>>(f => f.First().FileName == "test.pdf")), Times.Once);
        }

        [Test]
        public async Task Given_a_valid_request_when_postFilesController_method_is_called_then_it_returns_201_Created_response()
        {
            //arrange
            var expectedResponseCode = 201;
            var testBucket = "test";
            var response = new AddFileResponse { PreSignedUrl = new List<string> { "test" } };
            var testFiles = TestHelper.Generate_FileMock();
            _mockPostUseCase.Setup(x => x.Execute(testBucket, testFiles)).ReturnsAsync(response);

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, testFiles);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
        }

        [Test]
        public async Task Given_a_invalid_request_when_postFilesController_method_is_called_then_it_returns_400_BadRequest_response()
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
       public async Task Given_a_invalid_bucket_when_postFilesController_method_is_called_then_it_returns_404_NotFound_response()
        {
            //arrange
            var expectedResponseCode = 404;
            var testBucket = "wrong-bucket";
            var testFiles = TestHelper.Generate_FileMock();
            var responseException = new AmazonS3Exception("The specified bucket does not exist.");
            responseException.ErrorCode = "NoSuchBucket";
            _mockPostUseCase.Setup(x => x.Execute(testBucket, testFiles)).Throws(responseException);

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, testFiles);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
        }

        [Test]
        public async Task Given_a_request_when_postFilesController_method_is_called_but_returns_an_exception_other_than_NoSuchBucket_it_returns_500StatusCode_response()
        {
            //arrange
            var expectedResponseCode = 500;
            var testBucket = "bucket";
            var testFiles = TestHelper.Generate_FileMock();
            var responseException = new AmazonS3Exception("The provided token is malformed or otherwise invalid.");
            responseException.ErrorCode = "InvalidToken";
            _mockPostUseCase.Setup(x => x.Execute(testBucket, testFiles)).Throws(responseException);

            //act
            var controllerResponse = await _filesController.AddFiles(testBucket, testFiles);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<ObjectResult>(result);
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
        }

        [Test]
        public async Task Given_a_invalid_bucket_when_postFilesController_method_is_called_then_404_NotFound_response_returns_formatted_ErrorResponse()
        {
            //arrange
            var testBucket = "wrong-bucket";
            var testFiles = TestHelper.Generate_FileMock();
            var responseException = new AmazonS3Exception("The specified bucket does not exist.");
            responseException.ErrorCode = "NoSuchBucket";
            _mockPostUseCase.Setup(u => u.Execute(testBucket,testFiles)).Throws(responseException);

            //act
            var response = await _filesController.AddFiles(testBucket,testFiles);
            var responseResult = (response.Result as ObjectResult).Value;
            var errorResponse = responseResult as ErrorsResponse;

            //assert
            Assert.IsInstanceOf<ErrorsResponse>(errorResponse);
            Assert.AreEqual(errorResponse.Status, "fail");
            Assert.AreEqual(errorResponse.Errors, responseException.Message);
        }

        [Test]
        public async Task Given_a_request_when_postFilesController_method_is_called_but_returns_an_exception_other_than_NoSuchBucket_500StatusCode_response_returns_formatted_ErrorResponse()
        {
            //arrange
            var testBucket = "wrong-bucket";
            var testFiles = TestHelper.Generate_FileMock();
            var responseException = new AmazonS3Exception("The provided token is malformed or otherwise invalid.");
            responseException.ErrorCode = "InvalidToken";
            _mockPostUseCase.Setup(u => u.Execute(testBucket, testFiles)).Throws(responseException);

            //act
            var response = await _filesController.AddFiles(testBucket,testFiles);
            var responseResult = (response.Result as ObjectResult).Value;
            var errorResponse = responseResult as ErrorsResponse;

            //assert
            Assert.IsInstanceOf<ErrorsResponse>(errorResponse);
            Assert.AreEqual(errorResponse.Status, "fail");
            Assert.AreEqual(errorResponse.Errors, responseException.Message);
        }
        #endregion

        #region Get S3 File

        [Test]
        public async Task Given_a_valid_request_when_getFilesController_method_is_called_then_usecase_is_called()
        {
            //arrange
            var request = TestHelper.Generate_ValidGetRequest(); ;

            //act
            await _filesController.GetFile(request);
            //assert
            _mockGetUseCase.Verify(u => u.Execute(It.IsAny<GetFileRequest>()), Times.Once);
        }

        [Test]
        public async Task Given_a_valid_request_when_getFileController_method_is_called_then_it_returns_200_Ok_response()
        {
            //arrange
            var expectedResponseCode = 200;
            var request = TestHelper.Generate_ValidGetRequest();
            var response = new GetFileResponse();
            _mockGetUseCase.Setup(u => u.Execute(request)).ReturnsAsync(response);

            //act
            var controllerResponse = await _filesController.GetFile(request);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.AreEqual(expectedResponseCode, result.StatusCode);
            Assert.IsInstanceOf<OkObjectResult>(result);

        }

        [Test]
        public async Task Given_a_successful_request_when_getFileController_method_is_called_then_it_returns_a_GetFileResponse()
        {
            //arrange
            var request = new GetFileRequest();
            var metadata = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string,string>("testMetadata", "metadataKeyValue")
            };
            var response = new GetFileResponse
            {
                Metadata = metadata
            };
            _mockGetUseCase.Setup(u => u.Execute(request)).ReturnsAsync(response);

            //act
            var controllerResponse = await _filesController.GetFile(request);
            var result = controllerResponse.Result as ObjectResult;

            //assert
            Assert.AreSame(response, result.Value);
            Assert.IsInstanceOf<GetFileResponse>(result.Value);
        }

        [Test]
        public async Task Given_a_invalid_request_when_getFileController_method_is_called_than_the_usecase_isnt_called()
        {
            //arrange
             
            //act
            await _filesController.GetFile(null);
            //assert
            _mockGetUseCase.Verify(u => u.Execute(It.IsAny<GetFileRequest>()), Times.Never);
        }

        [Test]
        public async Task Given_a_request_with_no_valid_bucket_when_getFileController_method_is_called_then_it_returns_a_404_response_error()
        {
            //arrange
            var expectedResponseCode = 404;
            var request = TestHelper.Generate_WrongBucketGetRequest();
            var responseException = new AmazonS3Exception("The specified bucket does not exist.");
            responseException.ErrorCode = "NoSuchBucket";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = response.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<NotFoundObjectResult>(responseResult);
            Assert.AreEqual(expectedResponseCode, responseResult.StatusCode);
        }

        [Test]
        public async Task Given_a_request_with_no_valid_bucket_when_getFileController_method_is_called_then_404NotFoundRequestObject_returns_formatted_ErrorResponse()
        {
            //arrange
            var request = TestHelper.Generate_WrongBucketGetRequest();
            var responseException = new AmazonS3Exception("The specified bucket does not exist.");
            responseException.ErrorCode = "NoSuchBucket";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = (response.Result as ObjectResult).Value;
            var errorResponse = responseResult as ErrorsResponse;

            //assert
            Assert.IsInstanceOf<ErrorsResponse>(errorResponse);
            Assert.AreEqual(errorResponse.Status, "fail");
            Assert.AreEqual(errorResponse.Errors, responseException.Message);
        }

        [Test]
        public async Task Given_a_request_with_no_valid_filename_when_getFileController_method_is_called_then_it_returns_a_404_response_error()
        {
            //arrange
            var expectedResponseCode = 404;
            var request = TestHelper.Generate_WrongFilenameGetRequest();
            var responseException = new AmazonS3Exception("The specified key does not exist.");
            responseException.ErrorCode = "NoSuchKey";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = response.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<NotFoundObjectResult>(responseResult);
            Assert.AreEqual(expectedResponseCode, responseResult.StatusCode);
        }

        [Test]
        public async Task Given_a_request_with_no_valid_filename_when_getFileController_method_is_called_then_404NotFoundRequestObject_returns_formatted_ErrorResponse()
        {
            //arrange
            var request = TestHelper.Generate_WrongFilenameGetRequest();
            var responseException = new AmazonS3Exception("The specified key does not exist.");
            responseException.ErrorCode = "NoSuchKey";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = (response.Result as ObjectResult).Value;
            var errorResponse = responseResult as ErrorsResponse;

            //assert
            Assert.IsInstanceOf<ErrorsResponse>(errorResponse);
            Assert.AreEqual(errorResponse.Status, "fail");
            Assert.AreEqual(errorResponse.Errors, responseException.Message);
        }

        [Test]
        public async Task Given_a_valid_request_when_getFileController_method_is_called_but_returns_an_excepetion_other_than_NoSuchKey_or_NoSuchBucket_it_returns_500StatusCode_response()
        {
            //arrange
            var expectedResponseCode = 500;
            var request = TestHelper.Generate_ValidGetRequest();
            var responseException = new AmazonS3Exception("The provided token is malformed or otherwise invalid.");
            responseException.ErrorCode = "InvalidToken";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = response.Result as ObjectResult;

            //assert
            Assert.IsInstanceOf<ObjectResult>(responseResult);
            Assert.AreEqual(expectedResponseCode, responseResult.StatusCode);
        }

        [Test]
        public async Task Given_a_valid_request_when_getFileController_method_is_called_but_returns_an_execpetion_other_than_NoSuchKey_or_NoSuchBucket_then_500StatusCode_returns_formatted_ErrorResponse()
        {
            //arrange
            var request = TestHelper.Generate_ValidGetRequest();
            var responseException = new AmazonS3Exception("The provided token is malformed or otherwise invalid.");
            responseException.ErrorCode = "InvalidToken";
            _mockGetUseCase.Setup(u => u.Execute(request)).Throws(responseException);

            //act
            var response = await _filesController.GetFile(request);
            var responseResult = (response.Result as ObjectResult).Value;
            var errorResponse = responseResult as ErrorsResponse;

            //assert
            Assert.IsInstanceOf<ErrorsResponse>(errorResponse);
            Assert.AreEqual(errorResponse.Status, "fail");
            Assert.AreEqual(errorResponse.Errors, responseException.Message);
        }


        #endregion
    }

}
