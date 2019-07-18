using Caf.Projects.CafLogisticsSampleProcessing.Etl;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace IngestExcelDetTest.Tests
{
    public class BlobExtractorTests
    {
        private string validDet = "https://cafltardatastream.blob.core.windows.net/cafsoilgridpointsurvey/Masters/SoilDryingGrinding/SoilDryingGrinding01_GridPointSurvey_IN_YYYYMMDD.xlsx";
        [Fact]
        public void BlobExtractor_InvalidConnString_ThrowsException()
        {
            // Arrange
            string connectionString = "";


            // Act
            var ex = Assert.Throws<ArgumentException>(() => new BlobExtractor(connectionString));

            // Assert
            Assert.Equal(
                "Could not parse storageconnectionstring, please verify the system environment exists and is correct.",
                ex.Message);
        }

        /// Requires a valid blobStorageConnectionString 
        /// (defined in local.settings.json) and valid blob path 
        /// (defined above as private string validDet)
         
        // TODO: Figure out a way to test this without hitting blob storage?
        //[Fact]
        //public async void BlobExtractor_ValidConnectionString_LoadsFiles()
        //{
        //    // Arrange
        //    var config = new ConfigurationBuilder()
        //        .SetBasePath(Environment.CurrentDirectory)
        //        .AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
        //        .Build();
        //
        //    string connectionString = config["BlobStorageConnection"] 
        //        ?? config["Values:BlobStorageConnection"];
        //
        //    BlobExtractor sut = new BlobExtractor(connectionString);
        //
        //    // Act
        //    MemoryStream ms = await sut.ExtractBlobAsync(validDet);
        //
        //    // Assert
        //    Assert.True(ms.Length > 0);
        //}
    }
}
