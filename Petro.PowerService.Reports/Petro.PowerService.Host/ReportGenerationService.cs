using log4net;
using log4net.Config;
using System;
using System.Threading;
using System.Threading.Tasks;
using Petro.PowerService.BusinessLogic;
using Petro.PowerService.BusinessLogic.Configuration;

namespace Petro.PowerService.Host
{
    public class ReportGenerationService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReportGenerationService));
        private Timer _heartbeatTimer;
        private readonly long _heartbeatIntervalInMins = 10;

        public ReportGenerationService()
        {
            XmlConfigurator.Configure();
            var serviceConfig = ServiceLocator.Resolve<IServiceConfiguration>();
            _heartbeatIntervalInMins = serviceConfig.RunIntervalInMinutes;
        }

        public void Start()
        {
            
            _heartbeatTimer = new Timer(OnTimerCallback, null, TimeSpan.Zero, TimeSpan.FromMinutes(_heartbeatIntervalInMins));
        }

        public void Stop()
        {
            _heartbeatTimer.Dispose();
            _heartbeatTimer = null;
        }

        private void OnTimerCallback(object state)
        {
            try
            {
                // Log heartbeat start message
                Log.Info($"Heartbeat Callback Start \t{DateTime.Now}");

                // Create instance of report generator service and kick of generation process async
                var service = ServiceLocator.Resolve<IReportGenerator>();
                Task.Run(() => service.GenerateAsync());

                // Log heartbeat end message
                Log.Info($"Heartbeat Callback finish \t{DateTime.Now}");
            }
            catch (Exception ex)
            {
                Log.Error($"Heartbeat Callback failed \t{DateTime.Now}", ex);
            }
        }
    }
}