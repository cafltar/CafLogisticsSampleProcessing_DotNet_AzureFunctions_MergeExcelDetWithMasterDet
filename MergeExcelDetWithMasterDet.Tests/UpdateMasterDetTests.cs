using System;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Moq;
using Microsoft.Extensions.Logging.Internal;

namespace IngestExcelDetTest.Tests
{
    public class UpdateMasterDetTests
    {
        private string fileWithRealData =
            @"Assets/LTARcafSoilDryingGrindingV1_GP_BC_20190405.xlsx";

        //[Fact]
        //public async void Run_CorrectJson_ParsesCorrectly()
        //{
        //    // Arrange
        //    string json = getHttpRequestBodyJson();
        //    Mock<ILogger> logger = new Mock<ILogger>();
        //    
        //    // Act
        //    var response = await UpdateMasterDet.Run(CreateMockRequest(json).Object, logger.Object);
        //
        //    // Assert
        //    logger.Verify(l => l.Log(
        //        LogLevel.Information,
        //        It.IsAny<EventId>(),
        //        It.Is<FormattedLogValues>(v => v.ToString().Contains("det: https://cafltardatastream.blob.core.windows.net/cafsoilgridpointsurvey/Masters/SoilDryingGrinding/SoilDryingGrinding01_GridPointSurvey_IN_YYYYMMDD.xlsx, master: https://cafltardatastream.blob.core.windows.net/cafsoilgridpointsurvey/SoilDryingGrinding/LTARcafSoilDryingGrindingV1_GP_BC_20190409_132.xlsx")), 
        //        It.IsAny<Exception>(),
        //        It.IsAny<Func<object, Exception, string>>()), 
        //        Times.Exactly(1));
        //}

        private string getHttpRequestBodyJson()
        {
            return "{\"det\": \"https://cafltardatastream.blob.core.windows.net/cafsoilgridpointsurvey/Masters/SoilDryingGrinding/SoilDryingGrinding01_GridPointSurvey_IN_YYYYMMDD.xlsx\", \"master\":\"https://cafltardatastream.blob.core.windows.net/cafsoilgridpointsurvey/SoilDryingGrinding/LTARcafSoilDryingGrindingV1_GP_BC_20190409_132.xlsx\"}";
        }

        private static Mock<HttpRequest> CreateMockRequest(string json)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);

            streamWriter.Write(json);
            streamWriter.Flush();

            memoryStream.Position = 0;

            var mockRequest = new Mock<HttpRequest>();

            mockRequest.Setup(x => x.Body).Returns(memoryStream);

            return mockRequest;
        }
    }
}
