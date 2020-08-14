using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.Exceptions
{
    public class ErrorsResponse
    {
        public string Status { get; set; } = "fail";
        public string Errors { get; set; }

        public ErrorsResponse(string error)
        {
            Errors = error;
        }
    }
}
