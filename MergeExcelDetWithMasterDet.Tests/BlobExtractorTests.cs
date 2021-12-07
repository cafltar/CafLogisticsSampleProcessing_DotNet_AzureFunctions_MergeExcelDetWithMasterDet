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
    }
}
