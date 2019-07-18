using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Caf.Projects.CafLogisticsSampleProcessing.Etl;

namespace Caf.Projects.CafLogisticsSampleProcessing
{
    /// <summary>
    /// Azure Functions to merge data entry templates with master database
    /// </summary>
    public static class MergeExcelDetWithMasterDet
    {
        /// <summary>
        /// Downloads two data entry templates (DET) to be merged, merges files, uploads and replaces merged DET to "master DET"
        /// </summary>
        /// <param name="req">Assumes the following fields in JSON requestBody: "det" is URI to blob, "master" is URI to blob, "masterBlobContainerName" is blob container where "master" blob is stored, "masterBlobPath" is path to master blob to be replaced</param>
        /// <param name="log">Logger</param>
        /// <returns>OkObjectResult if successful, BadRequestObjectResult if not.</returns>
        [FunctionName("MergeExcelDetWithMasterDet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get Config info
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            string connectionString = config["BlobStorageConnection"] ?? 
                config["Values:BlobStorageConnection"];

            // Get request body, parse json to get paths to blob files
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string uriDet = data?.det;
            string uriMaster = data?.master;
            string headerRow = data?.headerRow;
            string masterBlobContainerName = data?.masterBlobContainerName;
            string masterBlobPath = data?.masterBlobPath;

            log.LogInformation($"det: {uriDet}, master: {uriMaster}");
            log.LogInformation($"headerRow: {headerRow}");
            log.LogInformation($"masterBlobContainerName: {masterBlobContainerName}");
            log.LogInformation($"masterBlobPath: {masterBlobPath}");

            // Get blobs as MemoryStream
            BlobExtractor blobLoader = new BlobExtractor(connectionString);
            MemoryStream detStream =
                await blobLoader.ExtractBlobAsync(uriDet);
            MemoryStream masterStream =
                await blobLoader.ExtractBlobAsync(uriMaster);

            BlobTransformer merger = new BlobTransformer();
            MemoryStream updatedBlob = merger.MergeBlobs(
                detStream, masterStream, 6);

            BlobLoader uploader = new BlobLoader(connectionString);
            bool isSuccess = await uploader.LoadBlobAsync(
                updatedBlob, masterBlobContainerName, masterBlobPath);

            return isSuccess
                ? (ActionResult)new OkObjectResult("Master template updated")
                : new BadRequestObjectResult("Something bad happened");
        }
    }
}
