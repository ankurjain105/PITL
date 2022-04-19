using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Petro.PowerService.BusinessLogic.Queries.GetTrades;
using Services;
using AutoFixture;
using System.Threading.Tasks;
using Shouldly;

namespace Petro.PowerService.BusinessLogic.UnitTests
{
    public class GetAggregatedTradesQueryHandlerTests
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly Mock<IPowerService> _powerService = new Mock<IPowerService>(MockBehavior.Strict);
        private GetAggregatedTradesQueryHandler _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new GetAggregatedTradesQueryHandler(_powerService.Object);
        }

        [Test]
        public async Task Handler_AggregatesDataCorrectly()
        {
            //Arrange
            var tradeDate = _fixture.Create<DateTime>().Date;
            var trades = MockPowerTrades(tradeDate, 3).ToArray();
            _powerService.Setup(x => x.GetTradesAsync(tradeDate)).ReturnsAsync(trades);

            //Act
            var result = await _handler.HandleAsync(new GetAggregatedTradesQuery(tradeDate));

            //Assert
            result.Trades.Length.ShouldBe(24);
            foreach (var periodResult in result.Trades)
            {
                var period = ((periodResult.TradeTime.Hour + 1) % 24) + 1;
                var expectedVolume = trades.SelectMany(x => x.Periods).Where(x => x.Period == period).Sum(x => x.Volume);
                periodResult.AggregatedVolume.ShouldBe(expectedVolume);
            }
        }

        private IEnumerable<PowerTrade> MockPowerTrades(DateTime tradeDate, int count)
        {
            foreach (var index in Enumerable.Range(1, count))
            {
                var trade = PowerTrade.Create(tradeDate, 24);
                foreach (var period in trade.Periods)
                {
                    period.Volume = _fixture.Create<double>();
                }
                yield return trade;
            }
        }
    }
}