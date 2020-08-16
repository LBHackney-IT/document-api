using NUnit.Framework;
using NUnit.Framework.Internal;
using document_api.V1.Exceptions;

namespace UnitTests.V1.Exceptions
{
    [TestFixture]
    public class ErrorsResponseTests
    {
        [Test]
        public void When_a_new_ErrorResponse_Object_is_created_its_default_status_parameter_is_fail()
        {
            var error = new ErrorsResponse("some kind of error");

            Assert.AreEqual(error.Status, "fail");

        }

        [Test]
        public void When_a_new_ErrorResponse_object_is_created_it_returns_the_same_error_message()
        {
            var errorMessage = "Something Broke";

            var error = new ErrorsResponse(errorMessage);

            Assert.AreEqual(error.Errors, errorMessage);
        }
    }

}
