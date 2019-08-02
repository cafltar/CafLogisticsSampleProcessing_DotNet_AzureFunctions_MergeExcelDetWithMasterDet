using Caf.Projects.CafLogisticsSampleProcessing.Core;
using Caf.Projects.CafLogisticsSampleProcessing.Etl;
using Moq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IngestExcelDetTest.Tests
{
    public class BlobTransformerTests
    {
        [Fact]
        public async void TransformBlobs_NoData_ThrowsException()
        {
            // Arrange
            Mock<IBlobExtractor> extractor = new Mock<IBlobExtractor>();

            extractor.Setup(l => l.ExtractBlobAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<MemoryStream>(new MemoryStream()));

            var blob1 = await extractor.Object.ExtractBlobAsync("");
            var blob2 = await extractor.Object.ExtractBlobAsync("");

            BlobTransformer sut = new BlobTransformer();

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => sut.MergeBlobs(blob1, blob2, 6));

            // Assert
            Assert.Equal(
                "One or more input blobs do not have data",
                ex.Message);
        }

        [Fact]
        public async void MergeBlobs_DetDifferentThanMaster_ThrowsException()
        {
            // Arrange
            Mock<IBlobExtractor> extractor = new Mock<IBlobExtractor>();
            extractor.Setup(l => l.ExtractBlobAsync(It.IsAny<string>()))
                .Returns((string input) => Task.FromResult<MemoryStream>(
                    GetMemoryStreamFromFile(input)));

            var det = await extractor.Object.ExtractBlobAsync(
                "Assets/SoilBulkDensity01_GridPointSurvey_BC_20190201.xlsx");
            var master = await extractor.Object.ExtractBlobAsync(
                "Assets/SoilDryingGrinding01_GridPointSurvey_IN_YYYYMMDD_empty.xlsx");

            BlobTransformer sut = new BlobTransformer();

            // Act
            var ex = Assert.Throws<ArgumentException>(() => sut.MergeBlobs(det, master, 6));

            // Assert
            Assert.Equal("DET file does not match Master file", ex.Message);
        }

        [Fact]
        public async void MergeBlobs_ValidInput_CorrectlyMergeValues()
        {
            // Arrange
            Mock<IBlobExtractor> extractor = new Mock<IBlobExtractor>();
            extractor.Setup(l => l.ExtractBlobAsync(It.IsAny<string>()))
                .Returns((string input) => Task.FromResult<MemoryStream>(
                    GetMemoryStreamFromFile(input)));

            var det = await extractor.Object.ExtractBlobAsync(
                "Assets/SoilDryingGrinding01_GridPointSurvey_BC_20190418_fromSomeValues.xlsx");
            var master = await extractor.Object.ExtractBlobAsync(
                "Assets/SoilDryingGrinding01_GridPointSurvey_IN_YYYYMMDD_someValues.xlsx");

            BlobTransformer sut = new BlobTransformer();

            // Act
            MemoryStream ms = sut.MergeBlobs(det, master, 6);

            // Assert
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                Assert.True(ws.Cells[8, 2].GetValue<string>().Length > 0);
                Assert.True(ws.Cells[8, 10].GetValue<string>().Length > 0);
                Assert.True(ws.Cells[311, 5].GetValue<string>().Length > 0);
            }
        }

        [Fact]
        public async void MergeBlobs_ChangeParameterValue_UpdatesValue()
        {
            // Arrange
            Mock<IBlobExtractor> extractor = new Mock<IBlobExtractor>();
            extractor.Setup(l => l.ExtractBlobAsync(It.IsAny<string>()))
                .Returns((string input) => Task.FromResult<MemoryStream>(
                    GetMemoryStreamFromFile(input)));

            var det = await extractor.Object.ExtractBlobAsync(
               "Assets/Harvest01_2019_GP-ART-Lime_INT_YYYYMMDD_20190802_updateParam.xlsm");
            var master = await extractor.Object.ExtractBlobAsync(
                "Assets/Harvest01_2019_GP-ART-Lime_INT_YYYYMMDD_20190802.xlsm");

            BlobTransformer sut = new BlobTransformer();

            // Act
            MemoryStream ms = sut.MergeBlobs(det, master, 7);

            // Assert
            using (ExcelPackage package = new ExcelPackage(ms))
            {
                ExcelWorksheet ws = package.Workbook.Worksheets[1];
                Assert.True(ws.Cells[2, 2].GetValue<int>() == 4);
            }
            
        }

        private MemoryStream GetMemoryStreamFromFile(string filePath)
        {
            FileStream fileStream = File.OpenRead(filePath);
            MemoryStream memoryStream = new MemoryStream();

            fileStream.CopyTo(memoryStream);

            return memoryStream;
        }
    }
}
