using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.Boundary
{
    public class GetFileRequest
    {
        public string bucketName { get; set; }
        public string fileName { get; set; }
    }


}
