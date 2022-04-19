using System.Configuration;

namespace Petro.PowerService.BusinessLogic.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
            ReportPath = ConfigurationManager.AppSettings["ReportPath"] ?? throw new ConfigurationErrorsException("ReportPath AppSetting not configured");
            var runInterval = ConfigurationManager.AppSettings["RunIntervalInMinutes"] ?? throw new ConfigurationErrorsException("RunIntervalInMinutes AppSetting not configured");
            if (!int.TryParse(runInterval, out int val))
            {
                throw new ConfigurationErrorsException("RunIntervalInMinutes AppSetting cannot be parsed to a int value");
            }

            RunIntervalInMinutes = val;
        }

        public string ReportPath { get; }
        public int RunIntervalInMinutes { get; }
    }
}