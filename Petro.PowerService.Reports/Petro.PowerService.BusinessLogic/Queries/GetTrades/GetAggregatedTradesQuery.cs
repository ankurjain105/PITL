using System;

namespace Petro.PowerService.BusinessLogic.Queries.GetTrades
{
    public class GetAggregatedTradesQuery : IQuery
    {
        public GetAggregatedTradesQuery(DateTime tradeDate) => 
            TradeDate = tradeDate;

        public DateTime TradeDate;
    }
}