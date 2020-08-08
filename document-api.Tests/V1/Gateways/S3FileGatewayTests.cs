using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using System.Threading.Tasks;
using document_api.V1.Boundary;
using document_api.V1.Gateways;
using document_api.V1.Infrastructure;
using document_api.Tests.V1.Helper;

namespace UnitTests.V1.Gateways
{
    [TestFixture]
    public class S3FileGatewayTests
    {
        private IS3FileGateway classUnderTest;
        private Mock<IS3Client> mockS3Client;

        [SetUp]
        public void SetUp()
        {
            mockS3Client = new Mock<IS3Client>();
            classUnderTest = new S3FileGateway(mockS3Client.Object);
        }

        [Test]
        public async Task Given_a_successful_request_when_UploadFiles_is_called_it_returns_a_successful_response_object()
        {
            //arrange
            var testBucket = "test";
            var testFile = TestHelper.Generate_FileMock();
            var testFiles = new List<IFormFile>
            {
                testFile
            };
            var testUrlResponse = new List<string>
            {
                "testURL"
            };
            var expectedResponse = new AddFileResponse { PreSignedUrl = testUrlResponse };
            mockS3Client.Setup(x => x.UploadFiles(It.IsAny<string>(), It.IsAny<IList<IFormFile>>())).ReturnsAsync(expectedResponse);
            //act
            var response =  await classUnderTest.UploadFiles(testBucket, testFiles);
            //assert
            Assert.AreSame(response, expectedResponse);
        }

    }

}

