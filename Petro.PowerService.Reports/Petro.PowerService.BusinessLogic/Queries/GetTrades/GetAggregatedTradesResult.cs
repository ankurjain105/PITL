using System;
using System.Data;

namespace Petro.PowerService.BusinessLogic.Queries.GetTrades
{
    public class GetAggregatedTradesResult
    {
        public AggregatedTrade[] Trades { get; set; }

        public DataTable ToDataTable()
        {
            var dataTable = new DataTable("AggregatedTradesResult");
            dataTable.Columns.Add("Local Time", typeof(DateTime));
            dataTable.Columns.Add("Volume", typeof(double));
            if (Trades != null)
            {
                foreach (var trade in Trades)
                {
                    dataTable.Rows.Add(trade.TradeTime, trade.AggregatedVolume);
                }
            }

            return dataTable;
        }
    }

    public class AggregatedTrade
    {
        public DateTime TradeTime { get; set; }
        public double? AggregatedVolume { get; set; }
    }
}