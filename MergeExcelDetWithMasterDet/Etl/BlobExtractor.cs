using Caf.Projects.CafLogisticsSampleProcessing.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Caf.Projects.CafLogisticsSampleProcessing.Etl
{
    /// <summary>
    /// Uses Azure Storage API to download blob files
    /// </summary>
    public class BlobExtractor: IBlobExtractor
    {
        private readonly CloudStorageAccount storageAccount;
        public BlobExtractor(string storageConnectionString)
        {
            if(!CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                throw new ArgumentException("Could not parse storageconnectionstring, please verify the system environment exists and is correct.");
            }
        }

        /// <summary>
        /// Downloads blob into a memorystream
        /// </summary>
        /// <param name="blobPath">A valid URI to the blob to be downloaded</param>
        /// <returns></returns>
        public async Task<MemoryStream> ExtractBlobAsync(string blobPath)
        {
            Uri blobUri = new Uri(blobPath);
            CloudBlockBlob blob = new CloudBlockBlob(blobUri);

            MemoryStream ms = new MemoryStream();

            await blob.DownloadToStreamAsync(ms);

            return ms;
        }
    }
}
