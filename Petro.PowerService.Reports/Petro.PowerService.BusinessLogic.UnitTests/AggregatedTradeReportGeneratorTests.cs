using System;
using System.Collections.Generic;
using System.Data;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using Petro.PowerService.BusinessLogic.Configuration;
using Petro.PowerService.BusinessLogic.Core;
using Petro.PowerService.BusinessLogic.Queries;
using Petro.PowerService.BusinessLogic.Queries.GetTrades;
using Petro.PowerService.BusinessLogic.ReportWriter;
using Services;
using Shouldly;

namespace Petro.PowerService.BusinessLogic.UnitTests
{
    internal class AggregatedTradeReportGeneratorTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IClock> _clock = new Mock<IClock>(MockBehavior.Strict);
        private readonly Mock<IServiceConfiguration> _configuration = new Mock<IServiceConfiguration>(MockBehavior.Strict);
        private readonly Mock<IReportWriter> _reportWriter = new Mock<IReportWriter>(MockBehavior.Strict);
        private readonly Mock<IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult>> _queryHandler = new Mock<IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult>>(MockBehavior.Strict);
        private readonly Mock<ILogger> _logger = new Mock<ILogger>(MockBehavior.Loose);
        private AggregatedTradeReportGenerator _reportGenerator;

        [SetUp]
        public void Setup()
        {
            _reportGenerator = new AggregatedTradeReportGenerator(_clock.Object, _configuration.Object, _queryHandler.Object, _reportWriter.Object, _logger.Object);
        }

        [Test]
        public async Task RunsReportForCorrectDate()
        {
            //Arrange
            var reportTime = new DateTime(2022, 12, 23, 16, 30, 0);
            _clock.Setup(x => x.Now).Returns(reportTime);
            _configuration.Setup(x => x.ReportPath).Returns("TestPath");
            var queryResult = new GetAggregatedTradesResult();
            _queryHandler.Setup(x => x.HandleAsync(It.Is<GetAggregatedTradesQuery>(q => q.TradeDate == reportTime.Date)))
                .ReturnsAsync(queryResult);
            _reportWriter.Setup(x => x.WriteAsync("TestPath\\PowerPosition_20221223_1630.csv", It.IsAny<DataTable>())).Returns(Task.CompletedTask);

            //Act
            var result = await _reportGenerator.GenerateAsync();

            result.ShouldBeTrue();
            _queryHandler.VerifyAll();
            _reportWriter.VerifyAll();
        }
    }
}