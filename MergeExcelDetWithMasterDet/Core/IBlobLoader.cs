using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Caf.Projects.CafLogisticsSampleProcessing.Core
{
    public interface IBlobExtractor
    {
        Task<MemoryStream> ExtractBlobAsync(string blobPath); 
    }
}
