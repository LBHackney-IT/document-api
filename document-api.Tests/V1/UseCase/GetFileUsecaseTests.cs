using Bogus;
using document_api.V1.Boundary;
using document_api.V1.Gateways;
using document_api.V1.UseCase;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests.V1.UseCase
{
    [TestFixture]
    public class GetFileUsecaseTests
    {
        private IGetFileUsecase _getFileUsecase;
        private Mock<IS3FileGateway> _mockGateway;

        [SetUp]
        public void SetUp()
        {
            _mockGateway = new Mock<IS3FileGateway>();
            _getFileUsecase = new GetFileUsecase(_mockGateway.Object);
        }

        [Test]
        public async Task When_GetFileUsecase_Execute_method_is_called_it_calls_the_gateway()
        {
            //arrange
            var request = new GetFileRequest();

            //act
            await _getFileUsecase.Execute(request);

            //assert
            _mockGateway.Verify(g => g.DownloadFile(It.IsAny<GetFileRequest>()), Times.Once);
        }

        [Test]
        public async Task Given_a_valid_request_when_The_Execute_method_is_called_it_calls_the_gateway_with_the_correct_data()
        {
            //arrange
            var request = new GetFileRequest
            {
                bucketName = "testbucket",
                fileName = "Test.pdf"
            };

            //act
            await _getFileUsecase.Execute(request);

            //assert
            _mockGateway.Verify(g => g.DownloadFile(It.Is<GetFileRequest>(obj =>
            obj.bucketName == request.bucketName &&
            obj.fileName == request.fileName)), Times.Once);
            
        }
    }

}
