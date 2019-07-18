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
    /// Uses Azure Storage API to upload data into blob storage
    /// </summary>
    public class BlobLoader : IBlobLoader
    {
        private readonly CloudStorageAccount storageAccount;

        public BlobLoader(string storageConnectionString)
        {
            if (!CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                throw new ArgumentException("Could not parse storageconnectionstring, please verify the system environment exists and is correct.");
            }
        }

        /// <summary>
        /// Uploads a memorystream into specified blob container
        /// </summary>
        /// <param name="blobStream">MemoryStream of file to be uploaded</param>
        /// <param name="blobContainerName">Blob container for file to be uploaded to</param>
        /// <param name="blobPathAndFilename">Path and filename (including extension) of file within the Blob Container</param>
        /// <returns></returns>
        public async Task<bool> LoadBlobAsync(
            MemoryStream blobStream,
            string blobContainerName,
            string blobPathAndFilename)
        {
            CloudBlobClient blobClient = 
                storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container =
                blobClient.GetContainerReference(blobContainerName);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobPathAndFilename);

            blobStream.Position = 0;
            await blockBlob.UploadFromStreamAsync(blobStream);

            return true;
        }
    }
}
