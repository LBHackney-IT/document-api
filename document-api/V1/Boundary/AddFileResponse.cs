using System;
using System.Collections.Generic;

namespace document_api.V1.Boundary
{
    public class AddFileResponse
    {
        public readonly IList<string> StatusCode;

        public AddFileResponse(IList<string> statuscode) {
            StatusCode = statuscode;
        }
    }
}
