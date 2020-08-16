using NUnit.Framework;
using Moq;
using document_api.V1.UseCase;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using System.Threading.Tasks;
using document_api.V1.Gateways;
using document_api.Tests.V1.Helper;
using System.Linq;

namespace UnitTests.V1.UseCase
{ 
    [TestFixture]
    public class UploadFileUsecaseTests
    {
        private IUploadFile _uploadFileUsecase;
        private Mock<IS3FileGateway> _mockGateway;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IS3FileGateway>();
            _uploadFileUsecase = new UploadFileUsecase(_mockGateway.Object);
        }

        [Test]
        public async Task When_UploadFileUsecase_Execute_method_is_called_then_it_calls_the_gateway()
        {
            //arrange
            var testBucket = "test";
            var testFiles = TestHelper.Generate_FileMock();

            //act
           await _uploadFileUsecase.Execute(testBucket, testFiles);

            //assert
            _mockGateway.Verify(g => g.UploadFiles(It.IsAny<string>(), It.IsAny<IList<IFormFile>>()), Times.Once);
        }

        [Test]
        public async Task Given_a_request_when_UploadFileUsecase_Execute_method_is_called_it_calls_the_gateway_with_the_correct_data()
        {
            //arrange
            var testBucket = "test";
            var testFiles = TestHelper.Generate_FileMock();
        

            //act
            await _uploadFileUsecase.Execute(testBucket, testFiles);

            //assert
            _mockGateway.Verify(x => x.UploadFiles(It.Is<string>(str => str == testBucket), It.Is<IList<IFormFile>>(f => f.First().FileName == "test.pdf")));
        }
    }
   
}
