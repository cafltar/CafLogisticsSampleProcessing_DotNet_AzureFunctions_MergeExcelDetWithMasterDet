using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Caf.Projects.CafLogisticsSampleProcessing.Core
{
    public interface IBlobLoader
    {
        Task<bool> LoadBlobAsync(
            MemoryStream blobStream,
            string blobContainerName,
            string blobPathAndFilename);
    }
}
