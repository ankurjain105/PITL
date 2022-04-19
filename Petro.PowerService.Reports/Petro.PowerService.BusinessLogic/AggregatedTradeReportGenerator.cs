using System;
using System.IO;
using System.Threading.Tasks;
using Petro.PowerService.BusinessLogic.Configuration;
using Petro.PowerService.BusinessLogic.Core;
using Petro.PowerService.BusinessLogic.Queries;
using Petro.PowerService.BusinessLogic.Queries.GetTrades;
using Petro.PowerService.BusinessLogic.ReportWriter;

namespace Petro.PowerService.BusinessLogic
{
    public class AggregatedTradeReportGenerator : IReportGenerator
    {
        private readonly IClock _clock;
        private readonly IServiceConfiguration _serviceConfiguration;
        private readonly IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult> _queryHandler;
        private readonly IReportWriter _reportWriter;
        private readonly ILogger _logger;

        public AggregatedTradeReportGenerator(IClock clock, 
            IServiceConfiguration serviceConfiguration, 
            IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult> queryHandler, 
            IReportWriter reportWriter,
            ILogger logger)
        {
            _clock = clock;
            _serviceConfiguration = serviceConfiguration;
            _queryHandler = queryHandler;
            _reportWriter = reportWriter;
            _logger = logger;
        }

        public async Task<bool> GenerateAsync()
        {
            var instanceId = Guid.NewGuid().ToString();
            var reportDate = _clock.Now;
            _logger.Info($"Start Report Generation {instanceId} {reportDate}");
            try
            {
                var data = await _queryHandler.HandleAsync(new GetAggregatedTradesQuery(ToTradeDate(reportDate)));
                _logger.Info($"Report Query Finished {instanceId}");
                if (!Directory.Exists(_serviceConfiguration.ReportPath))
                {
                    Directory.CreateDirectory(_serviceConfiguration.ReportPath);
                }

                string fileName = Path.Combine(_serviceConfiguration.ReportPath, GetFileName(reportDate));
                await _reportWriter.WriteAsync(fileName, data.ToDataTable());
 
                _logger.Info($"Finish Report Generation {instanceId} {reportDate.ToShortDateString()}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"Report Generation Failed {instanceId}. Error - {ex}");
                return false;
            }
        }

        public string GetFileName(DateTime date)
        {
            return $"PowerPosition_{date:yyyyMMdd_HHmm}.csv";
        }

        private DateTime ToTradeDate(DateTime date)
        {
            if (date.Hour >= 23)
            {
                return date.Date.AddDays(1);
            }

            return date.Date;
        }
    }
}