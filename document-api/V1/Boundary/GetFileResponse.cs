using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_api.V1.Boundary
{
    public class GetFileResponse
    {
        public IList<KeyValuePair<string, string>> Metadata { get; set; }
    }

}
