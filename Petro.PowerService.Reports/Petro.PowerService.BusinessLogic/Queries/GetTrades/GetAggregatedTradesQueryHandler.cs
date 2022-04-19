using System;
using System.Linq;
using System.Threading.Tasks;
using Services;

namespace Petro.PowerService.BusinessLogic.Queries.GetTrades
{
    public class GetAggregatedTradesQueryHandler : IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult>
    {
        private readonly IPowerService _powerService;

        public GetAggregatedTradesQueryHandler(IPowerService powerService)
        {
            _powerService = powerService;
        }
        public  async Task<GetAggregatedTradesResult> HandleAsync(GetAggregatedTradesQuery query)
        {
            var tradeDate = query.TradeDate.Date;
            var trades = await _powerService.GetTradesAsync(tradeDate);
            var aggregatedTrades = trades.SelectMany(x => x.Periods)
                .GroupBy(x => x.Period)
                .Select(x => new { Period = x.Key, AggregatedVolume = x.Sum(p => p.Volume)})
                .OrderBy(x => x.Period)
                .Select(x => new AggregatedTrade() { TradeTime = ToDateTime(tradeDate, x.Period), AggregatedVolume = x.AggregatedVolume})
                .ToArray();
            return new GetAggregatedTradesResult()
            {
                Trades = aggregatedTrades
            };
        }

        private DateTime ToDateTime(DateTime tradeDate, int period)
        {
            //Baseline DateTime from 23:00:00 hours
            return tradeDate.AddHours(-1).AddHours(period - 1);
        }
    }
}